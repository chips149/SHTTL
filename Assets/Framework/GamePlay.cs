using System.Collections.Generic;
using System.Linq;

namespace Framework.Gameplay
{
    public static class GameplayCombineHandle
    {
        public static void CombineExecute<T>(T data, params GameplayContainer[] containers) where T : GameplayEventData
        {
            var combine = new GameplayContainer();
            foreach (var container in containers)
            {
                combine.effects.AddRange(container.effects);
            }
            combine.Build();

            combine.Execute(data);
        }

    }

    // 容器
    public class GameplayContainer
    {
        public readonly List<GameplayEffect> effects = new();
        public void Build()
        {
            // 排序
            effects.Sort((a,b) => a.Priority = b.Priority);
        }

        public void Execute<T>(T ctx) where T : GameplayEventData
        {
            var temp = effects.OfType<IGameplayEvent<T>>().ToArray();
            foreach (var effect in temp)
            {
                effect.Execute(ctx);

                if (ctx.isInterrupt)
                    return;
            }

            foreach (var effect in  temp)
            {
                if(effect is not GameplayEffect{finish:true}e)continue;
                e.OnRemove();
                effects.Remove(e);
            }
        }
    }

    public abstract class GameplayEffect
    {
        public int Priority;
        public bool finish = false;

        public virtual void OnRefresh()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }
    }

    public interface IGameplayEvent<in T> where T : GameplayEventData
    {
        void Execute(T data);
    }

    public abstract class GameplayEventData
    {
        // 流程控制
        public bool isInterrupt = false;
    }

    //----------------------------------------------------------------------
    // 定义事件的data

    //
    // public class Attacking : GameplayEventData
    // {
    //     public IAttackAbility a;
    //     public IBeHitable b;
    //     public float baseValue;
    //     public float moreValue;
    // }
    //
    // // 定义 Buff
    // public class DoubleDamage : GameplayEffect, IGameplayEvent<Attacking>
    // {
    //     public void Execute(Attacking data)
    //     {
    //         data.moreValue += data.baseValue;
    //     }
    // }
    //
    // // 调用
    // public class Player
    // {
    //     private GameplayContainer container;
    //
    //     void Attacking()
    //     {
    //         GameplayCombineHandle.CombineExecute(new Attacking(), this.container, this.container, this.container);
    //     }
    // }
}