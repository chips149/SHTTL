using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using Object = UnityEngine.Object;


namespace Framework
{
    /// <summary>
    /// 你就学吧
    /// </summary>
    public static class ModulesManager
    {
        private static readonly Dictionary<string, object> MODULS = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize(){
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var targetAssemblies = assemblies.Where(_assembly =>
                                                        _assembly.FullName.StartsWith("Assembly-CSharp") ||
                                                        _assembly.FullName.StartsWith("Assembly-CSharp-firstpass"));

            var types = targetAssemblies
                       .SelectMany(_assembly => _assembly.GetTypes())
                       .Where(Predicate);

            foreach (var type in types){
                MODULS.TryAdd(type.Name, CreateInstance(type));
            }

            return;

            bool Predicate(Type _type){
                return _type is not null &&
                       Attribute.IsDefined(_type, typeof(RegisterBeforeSceneLoad)) &&
                       !_type.IsAbstract &&
                       _type.IsClass &&
                       _type.FullName != null &&
                       !_type.FullName.Contains("+");
            }
        }


        private static object CreateInstance(Type _type){
            if (_type.IsSubclassOf(typeof(MonoBehaviour)))
                return CreateMonoInstance(_type);

            if (_type.IsSubclassOf(typeof(ScriptableObject)))
                return CreatScriptableInstance(_type);

            //     return Activator.CreateInstance(type) as IModul;
            return _type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
        }

        private static object CreateMonoInstance(Type _type){
            var comp = Object.FindObjectOfType(_type,true);

            if (comp != null) return comp;

            var loader = _type.GetCustomAttribute<LoadInsteadOf>();
            if (loader is null)
                return new GameObject(_type.Name).AddComponent(_type);

            var prefab = Resources.Load(loader.PATH, _type);
            var obj    = Object.Instantiate(prefab);
            obj.name = prefab.name;
            return obj;
        }

        private static object CreatScriptableInstance(Type _type){
            var loader = _type.GetCustomAttribute<LoadInsteadOf>();
            if (loader is null)
                return ScriptableObject.CreateInstance(_type);

            return Resources.Load(loader.PATH, _type);
        }


        public static T Get<T>() where T : class{
            var type = typeof(T);
            if (MODULS.TryGetValue(type.Name, out var modul)){
                if (modul == null){
                    MODULS[type.Name] = modul = CreateInstance(type);
                }

                return modul as T;
            }


            var newModul = CreateInstance(type);
            MODULS.Add(type.Name, newModul);

            return newModul as T;
        }

        public static void Dispose<T>() where T : class{
            var type = typeof(T);
            if (!MODULS.Remove(type.Name, out var modul)) return;
            if (modul is MonoBehaviour mono){
                if(mono)
                    Object.Destroy(mono.gameObject);
                return;
            }

            type.GetMethod("OnDestroy")?.Invoke(modul, null);
        }
    }


    /// <summary>
    /// a flag to mark which class should be created before scene has been loaded/
    /// 一上来就需要加载的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RegisterBeforeSceneLoad : Attribute
    { }

    /// <summary>
    /// a flag mark which class use load to instead of create a new one.
    /// 一个标识符 用来 标记某个需要读取资源来创建， 而不是创建一个新的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class LoadInsteadOf : Attribute
    {
        public readonly string PATH;

        public LoadInsteadOf(string _path){
            PATH = _path;
        }
    }
}