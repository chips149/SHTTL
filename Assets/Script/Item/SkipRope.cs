using System.Collections.Generic;
using Framework;
using UnityEngine;

public class SkipRope : MonoBehaviour
{
    private UserAreaManager _areaManager;

    private void Start()
    {
        _areaManager = ModulesManager.Get<UserAreaManager>();
    }

    private void OnDestroy()
    {
        _areaManager?.RemovePlacedPosition(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Marble")) return;
        
        var force = new Vector2(0,4);
        collision.rigidbody.AddForce(force, ForceMode2D.Impulse);
    } 
}
