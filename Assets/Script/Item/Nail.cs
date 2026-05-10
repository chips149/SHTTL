using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nail : MonoBehaviour
{
    public int damage = 1;

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Marble")) return;
        
        var force = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f));
        collision.rigidbody.AddForce(force, ForceMode2D.Impulse);
        Hit();
    }

    private void Hit()
    {
        GameObject monster = GameObject.FindGameObjectWithTag("Monster");
        MonsterHealth health=monster.GetComponent<MonsterHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
