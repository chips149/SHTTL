using UnityEngine;

public class Dice : MonoBehaviour
{
    private static AudioClip _diceSfx;
    private static AudioClip DiceSfx => _diceSfx ??= Resources.Load<AudioClip>("Sound/SFX/Dice");
    [SerializeField] private Cooldown _cooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<MarbleBehavior>(out _) || !_cooldown.CanActivate()) return;

        AudioManager.PlaySFX(DiceSfx);
        _cooldown.Begin();

        int count = Random.Range(1, 7);
        for (int i = 0; i < count; i++)
        {
            GameState.Bm.CreateNewMarbleAndSetPos();
        }
    }

}
