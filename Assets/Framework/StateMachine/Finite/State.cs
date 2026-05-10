using System;

namespace Framework.StateMachine.Finite
{
    public abstract class State<TContext>
    {
        public abstract void OnEnter(TContext context);
        public abstract State<TContext> Tick(float dt, TContext context);
        public abstract void OnExit(TContext context);
    }


    public class WaitSecond<TContext> : State<TContext>
    {
        private readonly float                           DURATION;
        private readonly Func<TContext, State<TContext>> NEXT;

        private Action<TContext>        _onEnter;
        private Action<float, TContext> _onTick;
        private Action<TContext>        _onExit;

        private float _time;

        public WaitSecond(float duration, Func<TContext, State<TContext>> next){
            DURATION = duration;
            NEXT     = next;
        }

        public override void OnEnter(TContext context){
            _time = 0;
            _onEnter?.Invoke(context);
        }

        public override State<TContext> Tick(float dt, TContext context){
            _time += dt;
            _onTick?.Invoke(dt, context);
            return _time < DURATION ? null : NEXT.Invoke(context);
        }

        public override void OnExit(TContext context){
            _onExit?.Invoke(context);
        }


        public WaitSecond<TContext> SetEnter(Action<TContext> onEnter){
            _onEnter = onEnter;
            return this;
        }

        public WaitSecond<TContext> SetTick(Action<float, TContext> onTick){
            _onTick = onTick;
            return this;
        }


        public WaitSecond<TContext> SetExit(Action<TContext> onExit){
            _onExit = onExit;
            return this;
        }
    }
}