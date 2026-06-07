using System.Collections.Generic;
using UnityEngine;

public class SkipRope : MonoBehaviour
{
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Marble")) return;
        
        var force = new Vector2(0,4);
        collision.rigidbody.AddForce(force, ForceMode2D.Impulse);
    } 
}
