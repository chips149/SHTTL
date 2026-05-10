namespace Framework.BehaviorTree
{
    public enum NodeStatus
    {
        SUCCESSFUL,
        FAILURE,
        RUNNING
    }

    public interface INodeBase
    {
        INodeBase AddChild(INodeBase child);
        void OnEnter();
        NodeStatus Tick(float dt);
        void OnExit();
    }


    public abstract class NodeBase : INodeBase
    {
        protected INodeBase child;

        public virtual INodeBase AddChild(INodeBase c){
           return child = c;
        }

        public virtual void OnEnter(){
            child?.OnEnter();
        }

        public abstract NodeStatus Tick(float dt);

        public virtual void OnExit(){
            child?.OnExit();
        }
    }
}