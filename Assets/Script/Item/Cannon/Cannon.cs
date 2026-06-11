using UnityEngine;

public class Cannon : MonoBehaviour
{
    private const string CannonBulletPath = "Prefab/Bullet/CannonBullet";

    [SerializeField] private Cooldown _cooldown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble") || !_cooldown.CanActivate()) return;

        MarbleBehavior marble = other.GetComponent<MarbleBehavior>();
        ShootCannon(marble.isClone);

        _cooldown.Begin();

        if (marble.hasCake)
        {
            marble.DetachCake();
        }
    }


    private void ShootCannon(bool isEgg)
    {
        GameObject cannon = Resources.Load<GameObject>(CannonBulletPath);
        if (cannon == null)
        {
            return;
        }

        GameObject go = Instantiate(cannon, transform.position, Quaternion.identity);
        if (go.TryGetComponent<CannonFly>(out var cf))
        {
            cf.isEggBoosted = isEgg;
        }
    }
}


