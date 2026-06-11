using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const string ArrowBulletPath = "Prefab/Bullet/ArrowBullet";

    [SerializeField] private Cooldown _cooldown;

    private void Awake()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sortingOrder = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble") || !_cooldown.CanActivate()) return;

        MarbleBehavior marble = other.GetComponent<MarbleBehavior>();
        ShootArrow(marble.isClone);

        _cooldown.Begin();

        if (marble.hasCake)
        {
            marble.DetachCake();
        }
    }


    private void ShootArrow(bool isEgg)
    {
        GameObject arrow = Resources.Load<GameObject>(ArrowBulletPath);
        if (arrow == null)
        {
            return;
        }

        GameObject go = Instantiate(arrow, transform.position, Quaternion.identity);
        if (go.TryGetComponent<ArrowFly>(out var af))
        {
            af.isEggBoosted = isEgg;
        }
    }
}
