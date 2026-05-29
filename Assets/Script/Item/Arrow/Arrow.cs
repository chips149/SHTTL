using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const string ArrowBulletPath = "Prefab/Bullet/ArrowBullet";

    public float coolTime = 2f;
    private bool _canShoot = true;

    private void OnEnable()
    {
        _canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble") || !_canShoot) return;

        MarbleBehavior marble = other.GetComponent<MarbleBehavior>();
        ShootArrow(marble.isClone);

        _canShoot = false;
        Invoke(nameof(Reset), coolTime);

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
            Debug.LogError($"弓箭预制体加载失败，请确认路径存在：Resources/{ArrowBulletPath}.prefab");
            return;
        }

        GameObject go = Instantiate(arrow, transform.position, Quaternion.identity);
        if (go.TryGetComponent<ArrowFly>(out var af))
        {
            af.isEggBoosted = isEgg;
        }
    }

    private void Reset()
    {
        _canShoot = true;
    }
}
