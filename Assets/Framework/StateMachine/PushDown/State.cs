namespace Framework.StateMachine.PushDown
{
    public abstract class State<TContext>
    {
        public abstract void OnEnter(TContext context);
        public abstract State<TContext> Tick(float dt, TContext context);
        public abstract void OnExit(TContext context);
    }
}