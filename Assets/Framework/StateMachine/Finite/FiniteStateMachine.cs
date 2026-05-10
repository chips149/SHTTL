namespace Framework.StateMachine.Finite
{
    public class FiniteStateMachine<TContext>
    {
        private State<TContext> _current;

        public T GetCurrent<T>() where T : State<TContext>
        {
            return _current as T;
        }

        public void Entry(State<TContext> state, TContext ctx)
        {
            _current = state;
            _current.OnEnter(ctx);
        }

        public void Tick(TContext ctx, float dt)
        {
            var s = _current.Tick(dt, ctx);
            if (s == null) return;

            To(s, ctx);
        }


        public void To(State<TContext> state, TContext ctx)
        {
            _current.OnExit(ctx);
            _current = state;
            _current.OnEnter(ctx);
        }
    }
}