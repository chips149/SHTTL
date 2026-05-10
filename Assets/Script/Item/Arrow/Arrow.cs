using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float coolTime = 2f;
    private bool _canShoot = true;

    void OnEnable()
    {
       _canShoot = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble") || !_canShoot) return;

        var isEgg = other.GetComponent<MarbleBehavior>().isClone;
        ShootArrow(isEgg);

        _canShoot = false;
        Invoke("Reset", coolTime);
        
        MarbleBehavior marble = other.GetComponent<MarbleBehavior>();
        if (marble != null && marble.hasCake)
        {
            ShootCake();
            marble.hasCake = false;
        }
    }

    void ShootArrow(bool isEgg)
    {
        GameObject arrow = Resources.Load<GameObject>("Prefab/ArrowBullet");
        GameObject go = Instantiate(arrow, transform.position, Quaternion.identity);
        if (go.TryGetComponent<ArrowFly>(out var af))
        {
            af.isEggBoosted = isEgg;
        }
    }

    void ShootCake()
    {
        GameObject cake = Resources.Load<GameObject>("Prefab/WatchOutForTheCupcake");
        if (cake != null)
        {
            Instantiate(cake, transform.position, Quaternion.identity);
        }
    }
    void Reset()
    {
        _canShoot = true;
    }
}