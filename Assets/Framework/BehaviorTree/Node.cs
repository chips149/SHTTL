using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.BehaviorTree
{
    public class Sequential : NodeBase
    {
        private          int             _index   = 0;
        private readonly List<INodeBase> CHILDREN = new();

        public override INodeBase AddChild(INodeBase c){
            CHILDREN.Add(c);
            return c;
        }

        public override void OnEnter(){
            CHILDREN[_index].OnEnter();
        }

        public override void OnExit(){
            CHILDREN[_index].OnExit();
        }

        public override NodeStatus Tick(float dt){
            while (true){
                var status = CHILDREN[_index].Tick(dt);
                switch (status){
                    case NodeStatus.RUNNING:
                        return NodeStatus.RUNNING;
                    case NodeStatus.SUCCESSFUL:
                        CHILDREN[_index].OnExit();
                        _index = (_index + 1)% CHILDREN.Count;
                        CHILDREN[_index].OnEnter();                       
                        continue;
                    case NodeStatus.FAILURE:
                        CHILDREN[_index].OnExit();
                        _index = 0;
                        CHILDREN[_index].OnEnter();
                        return NodeStatus.FAILURE;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }

    public class Parallel : NodeBase
    {
        private readonly List<INodeBase> CHILDREN = new();

        public override INodeBase AddChild(INodeBase c){
            CHILDREN.Add(c);
            return c;
        }

        public override void OnEnter(){
            CHILDREN.ForEach(c => c.OnEnter());
        }

        public override void OnExit(){
            CHILDREN.ForEach(c => c.OnExit());
        }

        public override NodeStatus Tick(float dt){
            var running = CHILDREN.Select(node => node.Tick(dt))
                .Count(status => status == NodeStatus.RUNNING);

            return running == 0 ? NodeStatus.SUCCESSFUL : NodeStatus.RUNNING;
        }
    }


    public class ActionNode : NodeBase
    {
        private readonly Func<float, NodeStatus> ACTION;

        public ActionNode(Func<float, NodeStatus> action){
            ACTION = action;
        }

        public override NodeStatus Tick(float dt){
            return ACTION.Invoke(dt);
        }
    }
}