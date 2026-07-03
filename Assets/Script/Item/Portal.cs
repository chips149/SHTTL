using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private static AudioClip _portalSfx;
    private static AudioClip PortalSfx => _portalSfx ??= Resources.Load<AudioClip>("Sound/SFX/Portal");
    public static List<Portal> portals = new List<Portal>();

    [SerializeField] private Cooldown _cooldown;
    private SpriteRenderer _spriteRenderer;

    public ParticleSystem warpEnterEffect;
    public ParticleSystem warpLeaveEffect;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        portals.Add(this);
        BattleManager.OnCardSelectPhase += OnCardSelectPhase;
    }

    private void OnDisable()
    {
        portals.Remove(this);
        BattleManager.OnCardSelectPhase -= OnCardSelectPhase;
    }

    private void OnCardSelectPhase()
    {
        ResetVisual();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_cooldown.CanActivate()) return;
        if (!other.CompareTag("Marble")) return;
        if (portals.Count < 2) return;

        AudioManager.PlaySFX(PortalSfx);
        float minY = float.MaxValue;
        foreach (var p in portals)
        {
            if (p.transform.position.y < minY)
                minY = p.transform.position.y;
        }
        bool isInBottomRow = Mathf.Approximately(transform.position.y, minY);

        List<Portal> validTargets = new List<Portal>();
        foreach (var tp in portals)
        {
            if (tp == this) continue;
            if (tp.transform.position.y > transform.position.y)
                validTargets.Add(tp);
        }
        if (validTargets.Count == 0) return;

        Portal randomTarget = validTargets[Random.Range(0, validTargets.Count)];

        if (isInBottomRow)
            warpEnterEffect.Play();

        other.transform.position = randomTarget.transform.position;

        randomTarget.warpLeaveEffect.Play();

        _cooldown.Begin();
    }


    private void ResetVisual()
    {
        warpEnterEffect.Stop();
        warpLeaveEffect.Stop();
    }
    public static void ResetAllPortals() { }
}