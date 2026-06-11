using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public ParticleSystem eggPrefab;
    public static Sprite[] Sprites;

    [SerializeField] private Cooldown _cooldown;

    public int spawnCount = 3;
    public float spreadAngle = 40f;

    private void Awake()
    {
        Sprites ??= new Sprite[]
        {
            Resources.Load<Sprite>("2D/Item_Other_CaiDanQiu_IMG/Item_Other_CaiDanQiu1_IMG"),
            Resources.Load<Sprite>("2D/Item_Other_CaiDanQiu_IMG/Item_Other_CaiDanQiu2_IMG"),
            Resources.Load<Sprite>("2D/Item_Other_CaiDanQiu_IMG/Item_Other_CaiDanQiu3_IMG"),
        };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Marble") || !_cooldown.CanActivate()) return;

        SpawnEgg();
        _cooldown.Begin();

        ParticleSystem egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        egg.Play();
        if (!egg.main.loop)
        {
            Destroy(egg.gameObject, egg.main.duration);
        }
    }


    private void SpawnEgg()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float angle = (i - 1) * spreadAngle;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            
            var ball = GameState.Bm.CreateNewMarble();
            ball.transform.position = transform.position;
            ball.isClone = true;
            ball.GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, 3)];
            ball.eggPrefab.gameObject.SetActive(true);
            if (ball.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = dir * 5f;
            }
        }
    }
}