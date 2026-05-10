using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static List<Portal> portals = new List<Portal>();

    private bool isUsed;
    private SpriteRenderer spriteRenderer;

    public ParticleSystem warpEnterEffect;  
    public ParticleSystem warpLeaveEffect;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        portals.Add(this);
    }

    private void OnDisable()
    {
        portals.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;
        if (!other.CompareTag("Marble")) return;
        if (portals.Count < 2) return;

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
        
        isUsed = true;
        spriteRenderer.color = Color.gray;
    }

    public void ResetPortal()
    {
        isUsed = false;
        spriteRenderer.color = Color.white;
        
        warpEnterEffect.Stop();
        warpLeaveEffect.Stop();
    }

    public static void ResetAllPortals()
    {
        foreach (var portal in portals)
        {
            portal.ResetPortal();
        }
    }
}