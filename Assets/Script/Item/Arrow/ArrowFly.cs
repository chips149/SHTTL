using UnityEditorInternal;
using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public int damage = 10;
    public float speed = 8f;
    public bool isEggBoosted;

    private Transform target;

    void Start()
    {
        GameObject monster = GameObject.FindWithTag("Monster");
        target = monster.transform;

    }

    void Update()
    { 
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            Hit();
        }
    }

    void Hit()
    {
        MonsterHealth health = target.GetComponent<MonsterHealth>();
        if (health != null)
        {
            var finishDamage=isEggBoosted?damage*2:damage;
            health.TakeDamage(finishDamage);
        }

        Destroy(gameObject);
    }
}