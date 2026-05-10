using System.Collections.Generic;

namespace Framework.StateMachine.PushDown
{
    public class PushDownStateMachine<TContext>
    {
        private readonly TContext               CONTEXT;
        private readonly Stack<State<TContext>> STACK = new();

        private State<TContext> _current;

        public PushDownStateMachine(TContext context){
            CONTEXT = context;
        }

        public void Push(State<TContext> state){
            if (_current != null){
                _current.OnExit(CONTEXT);
                STACK.Push(_current);
            }

            _current = state;
            _current.OnEnter(CONTEXT);
        }


        public void Pop(){
            _current.OnExit(CONTEXT);
            if (STACK.TryPop(out _current)){
                _current.OnEnter(CONTEXT);
            }
        }


        public void Tick(float dt){
            if (_current == null) return;
            var s = _current.Tick(dt, CONTEXT);

            if (s == _current){
                Pop();
            }
            else if (s != null){
                Push(s);
            }
        }
    }
}