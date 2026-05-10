using System.Collections.Generic;
using System.Linq;

namespace Framework{

    public interface IUpdate{
        
        public bool IsDone{get;}
        virtual void OnUpdate(){}
        virtual void OnFixedUpdate(){}
        virtual void OnLateUpdate(){}
    }
    
    
    public class GlobalUpdate : MonoSingleton<GlobalUpdate>{
        private readonly List<IUpdate>  _updates = new();

        public void Register(IUpdate update){
            _updates.Add(update);
        }

        private void Update(){
            var temp = _updates.ToList();
            foreach (var update in temp){
                update.OnUpdate();
            }
        }

        private void FixedUpdate(){
            var temp = _updates.ToList();
            foreach (var update in temp){
                update.OnFixedUpdate();
            }
        }

        private void LateUpdate(){
            var temp = _updates.ToList();
            temp.ForEach(i=>i.OnLateUpdate());
            
            _updates.RemoveAll(i=>i.IsDone);
        }
    }
}