using System;
using UnityEngine;

namespace Framework{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{
        private static T _instance;
        public static T Instance{
            get{
                _instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }
        public void Awake(){
            if (!_instance){
                _instance = (T)this;
                if(transform.root == transform)
                    DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject);
            }
        }
        
        protected virtual void Initialize(){}
        
        public void Dispose(){
            _instance = null;
            Destroy(gameObject);
        }
    }
}