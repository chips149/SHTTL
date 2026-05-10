using Framework.Gameplay;
using UnityEngine;


public class TakeDamageEventData : GameplayEventData
{
    public int damage;

}

public class TakeHealEventData : GameplayEventData
{
    public int healAmount;
}