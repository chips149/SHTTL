using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeHit
{
    void BeHit(BeHitData data);
    void RemoveHp(RemoveHpData data);
}
