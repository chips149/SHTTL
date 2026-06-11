using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;

public class DotBuff : GameplayEffect,IGameplayEvent<FrameData>
{
    private const float Period = 1f;
    private const float Duration = 8f;
    private float t;
    private float timer;

    public override void OnRefresh()
    {
        t = 0;
        timer = 0;
    }
    public void Execute(FrameData data)
    {
        t += data.dt;
        if (t > Duration)
        {
            finish = true;
            return;
        }

        timer += data.dt;
        if (timer > Period)
        {
            timer -= Period;
            data.beHit.RemoveHp(new RemoveHpData { damage = 5 });
        }
    }
}
