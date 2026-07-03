using UnityEngine;

public class CakeFly : MonoBehaviour
{
    public int healAmount = 10;
    private static AudioClip _eatCakeSfx;
    private static AudioClip EatCakeSfx => _eatCakeSfx ??= Resources.Load<AudioClip>("Sound/SFX/EatCake");
    public int speed = 10;
    private Transform target;
    
    void Start()
    {
        GameObject player=GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            target.position, 
            speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            Heal();
        }
    }

    private void Heal()
    {
        Player health = target.GetComponent<Player>();
        
        health.TakeHeal(healAmount);
        AudioManager.PlaySFX(EatCakeSfx);
        
        Destroy(gameObject);
    }
}
