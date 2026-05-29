using UnityEngine;

public class Cannon : MonoBehaviour
{
    private const string CannonBulletPath = "Prefab/Bullet/CannonBullet";

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
        ShootCannon(marble.isClone);

        _canShoot = false;
        Invoke(nameof(ResetShoot), coolTime);

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
            Debug.LogError($"炮弹预制体加载失败，请确认路径存在：Resources/{CannonBulletPath}.prefab");
            return;
        }

        GameObject go = Instantiate(cannon, transform.position, Quaternion.identity);
        if (go.TryGetComponent<CannonFly>(out var cf))
        {
            cf.isEggBoosted = isEgg;
        }
    }

    private void ResetShoot()
    {
        _canShoot = true;
    }
}


