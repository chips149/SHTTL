using System;

namespace Framework{
    public class LazySingleton<T> where T : class, new(){
        public static T Instance = new Lazy<T>().Value;
        protected LazySingleton(){ }
    }
}