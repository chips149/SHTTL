using System;

namespace Framework.Timer
{
    public abstract class TimerBase : IUpdate
    {
        protected Action onComplete;
        protected float  time;

        public bool IsDone{ get; protected set; } = false;

        public virtual void OnUpdate(){ }

        public void Kill(){
            IsDone = true;
        }
    }
}