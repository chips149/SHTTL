using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cannon : MonoBehaviour
{
    public float coolTime = 2f;
    private bool _canShoot = true;

    void OnEnable()
    {
        _canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble")||!_canShoot) return;

        var isEgg = other.GetComponent<MarbleBehavior>().isClone;
        ShootCannon(isEgg);
        
        _canShoot = false;
        Invoke("ResetShoot", coolTime);

        MarbleBehavior marble = other.GetComponent<MarbleBehavior>();
        if (marble != null && marble.hasCake)
        {
            ShootCake();
            marble.hasCake = false;
        }
    }

    void ShootCannon(bool isEgg)
    {
        GameObject cannon = Resources.Load<GameObject>("Prefab/CannonBullet");
        GameObject go = Instantiate(cannon, transform.position, Quaternion.identity);
        if (go.TryGetComponent<CannonFly>(out var cf))
        {
            cf.isEggBoosted = isEgg;
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

    void ResetShoot()
    {
        _canShoot = true;
    }
}


