using System;
using Object = UnityEngine.Object;

namespace Framework
{
    [Serializable]
    public class InterfaceReference<TInterface, TObject> where TInterface : class where TObject :class 
    {
        public TObject obj;
        public TInterface Value => obj as  TInterface;
    }


    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object> where TInterface : class
    {
        
    }
}