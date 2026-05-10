using System;

namespace Framework.BehaviorTree
{
    public class Condition : NodeBase
    {
        private readonly Func<float, bool> CONDITION;

        public Condition(Func<float, bool> condition){
            CONDITION = condition;
        }

        public override NodeStatus Tick(float dt){
            return CONDITION.Invoke(dt) ? child.Tick(dt) : NodeStatus.FAILURE;
        }
    }


    public class Until : NodeBase
    {
        private readonly Func<float, bool> CONDITION;

        public Until(Func<float, bool> condition){
            CONDITION = condition;
        }

        public override NodeStatus Tick(float dt){
            return CONDITION.Invoke(dt) ? child.Tick(dt) : NodeStatus.RUNNING;
        }
    }
}