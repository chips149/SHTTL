using UnityEngine;

public class Cake : MonoBehaviour
{
    private static AudioClip _cakeSfx;
    private static AudioClip CakeSfx => _cakeSfx ??= Resources.Load<AudioClip>("Sound/SFX/Cake");
    [SerializeField] private Cooldown _cooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Marble") || !_cooldown.CanActivate()) return;

        AudioManager.PlaySFX(CakeSfx);
        _cooldown.Begin();

        MarbleBehavior marble = collision.GetComponent<MarbleBehavior>();
        marble.AttachCake();
    }

}
