using Framework;
using UnityEngine;

public class SkipRope : MonoBehaviour
{
    private static AudioClip _skipRopeSfx;
    private static AudioClip SkipRopeSfx => _skipRopeSfx ??= Resources.Load<AudioClip>("Sound/SFX/SkipRope");
    private UserAreaManager _areaManager;

    private void Start()
    {
        _areaManager = ModulesManager.Get<UserAreaManager>();
    }

    private void OnDestroy()
    {
        _areaManager?.RemovePlacedPosition(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Marble")) return;
        
        AudioManager.PlaySFX(SkipRopeSfx);
        var force = new Vector2(0,4);
        collision.rigidbody.AddForce(force, ForceMode2D.Impulse);
    } 
}
