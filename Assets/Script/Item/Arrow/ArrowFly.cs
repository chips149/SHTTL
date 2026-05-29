using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public int damage = 10;
    public float speed = 8f;
    public bool isEggBoosted;

    private Transform target;

    private void Start()
    {
        GameObject monster = GameObject.FindWithTag("Monster");
        if (monster == null)
        {
            Destroy(gameObject);
            return;
        }

        target = monster.transform;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            Hit();
        }
    }

    private void Hit()
    {
        IBeHit beHit = target.GetComponent<IBeHit>();
        if (beHit != null)
        {
            int finishDamage = isEggBoosted ? damage * 2 : damage;
            beHit.BeHit(new BeHitData
            {
                damage = finishDamage,
                from = "Arrow"
            });
        }

        Destroy(gameObject);
    }
}
