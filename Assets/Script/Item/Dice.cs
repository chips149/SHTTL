using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private float _lastTriggerTime;
    private const float Cooldown = 8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<MarbleBehavior>(out _)) return;
        if (Time.time - _lastTriggerTime < Cooldown) return;

        _lastTriggerTime = Time.time;
        
        int count = Random.Range(1, 7);

        for (int i = 0; i < count; i++)
        {
            GameState.Bm.CreateNewMarbleAndSetPos();
        }
    } 
}
