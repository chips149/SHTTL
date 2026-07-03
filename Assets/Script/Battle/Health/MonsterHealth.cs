using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterHealth : MonoBehaviour, IBeHit
{
    public int maxHp = 500;
    public int currentHp;
    public GameObject vectoryPanel;
    public GameObject hitEffectPrefab;
    public GameObject nailHitEffectPrefab;

    public bool showVictoryPanelOnDeath = true;

    public readonly GameplayContainer Container = new();

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

    private readonly Queue<GameObject> _effectPool = new();
    private readonly Queue<GameObject> _nailEffectPool = new();

    private void Start()
    {
        ResetHealth();
    }

    public void Configure(int hp, bool showVictoryPanel)
    {
        maxHp = hp;
        showVictoryPanelOnDeath = showVictoryPanel;
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHp = maxHp;
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void BeHit(BeHitData data)
    {
        if (currentHp <= 0 || !gameObject.activeInHierarchy) return;

        Container.Execute(data);
        
        bool isHeavy = data.from != "Nail";
        if (isHeavy)
        {
            var ui = GetComponent<MonsterUI>();
            ui?.PlayBeHit();

            SpawnHitEffect();
        }
        else
        {
            SpawnNailHitEffect();
        }

        int finalDamage = Mathf.RoundToInt(data.damage);

        currentHp = Mathf.Max(0, currentHp - finalDamage);
        
        DamagePopup.Spawn(transform.position, finalDamage, null, isHeavy);

        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void RemoveHp(RemoveHpData data)
    {
        if (currentHp <= 0) return;

        currentHp = Mathf.Max(0, currentHp - Mathf.RoundToInt(data.damage));
        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        BeHit(new BeHitData { damage = damage, from = "DirectCall" });
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;

        GameObject effect;
        if (_effectPool.Count > 0)
        {
            effect = _effectPool.Dequeue();
            effect.transform.position = transform.position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }
        else
        {
            effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(ReturnToPool(effect, _effectPool));
    }

    private void SpawnNailHitEffect()
    {
        if (nailHitEffectPrefab == null) return;

        Vector3 pos = GetRandomBodyPosition();

        GameObject effect;
        if (_nailEffectPool.Count > 0)
        {
            effect = _nailEffectPool.Dequeue();
            effect.transform.position = pos;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }
        else
        {
            effect = Instantiate(nailHitEffectPrefab, pos, Quaternion.identity);
        }

        StartCoroutine(ReturnToPool(effect, _nailEffectPool));
    }

    private Vector3 GetRandomBodyPosition()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            return RandomPointInBounds(sr.bounds, 0.5f);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            return RandomPointInBounds(col.bounds, 0.5f);

        return transform.position;
    }

    private static Vector3 RandomPointInBounds(Bounds bounds, float shrinkRatio)
    {
        Vector3 c = bounds.center;
        Vector3 e = bounds.extents * shrinkRatio;
        return new Vector3(
            Random.Range(c.x - e.x, c.x + e.x),
            Random.Range(c.y - e.y, c.y + e.y),
            c.z
        );
    }

    private IEnumerator ReturnToPool(GameObject effect, Queue<GameObject> pool)
    {
        yield return new WaitForSeconds(1f);
        effect.SetActive(false);
        pool.Enqueue(effect);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);

        if (showVictoryPanelOnDeath && vectoryPanel != null)
        {
            vectoryPanel.SetActive(true);
        }
    }
}
