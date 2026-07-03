using Framework;
using UnityEngine;

public class FuckMachine : MonoBehaviour
{
    private static AudioClip _fuckMachineSfx;
    private static AudioClip FuckMachineSfx => _fuckMachineSfx ??= Resources.Load<AudioClip>("Sound/SFX/FuckMachine");
    private Animator _ani;
    private UserAreaManager _areaManager;

    private void Start()
    {
        _ani= GetComponent<Animator>();
        _areaManager = ModulesManager.Get<UserAreaManager>();
    }

    private void OnDestroy()
    {
        _areaManager?.RemovePlacedPosition(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Marble")) return;
        
        AudioManager.PlaySFX(FuckMachineSfx);
        Vector2 hitNormal = collision.contacts[0].normal;
        Vector2 reflectDir=Vector2.Reflect(collision.relativeVelocity, hitNormal);
        _ani.SetTrigger("Atk" );

        collision.rigidbody.velocity = reflectDir.normalized * 6f;

    }
}
