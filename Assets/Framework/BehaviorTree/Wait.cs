namespace Framework.BehaviorTree
{
    public class Wait: NodeBase
    {
        private readonly float DELAY;
        private          float _time;


        public Wait(float delay){
            DELAY = delay;
        }

        public override void OnEnter(){
            _time = 0;
            base.OnEnter();
        }

        public override NodeStatus Tick(float dt){
            _time += dt;
            if (_time < DELAY){
                return NodeStatus.RUNNING;
            }

            _time = 0;
            return child.Tick(dt);
        }
    }
}