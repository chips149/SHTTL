using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Cooldown _cooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<MarbleBehavior>(out _) || !_cooldown.CanActivate()) return;

        _cooldown.Begin();

        int count = Random.Range(1, 7);
        for (int i = 0; i < count; i++)
        {
            GameState.Bm.CreateNewMarbleAndSetPos();
        }
    }

}
