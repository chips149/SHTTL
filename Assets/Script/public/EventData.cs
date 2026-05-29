using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;

public class FrameData : GameplayEventData
{
    public float dt;
    public IBeHit beHit;
}

public class BeHitData : GameplayEventData
{
    public float damage;
    public string from;
    public GameplayContainer attacker;
    public Action<Transform, bool> afterHit;
}

public class RemoveHpData : GameplayEventData
{
    public float damage;
}