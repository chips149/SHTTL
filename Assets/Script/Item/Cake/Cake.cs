using System;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    private bool _canShoot = true;
    public float coolTime = 5f;

    void OnEnable()
    {
        _canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Marble")) return;
        if (!_canShoot) return;

        _canShoot = false;

        MarbleBehavior marble = collision.GetComponent<MarbleBehavior>();
        marble.AttachCake();

        Invoke(nameof(Reset), coolTime);
    }

    void Reset()
    {
        _canShoot = true;
    }
}
