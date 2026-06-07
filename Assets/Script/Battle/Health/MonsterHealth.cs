using System;
using Framework.Gameplay;
using UnityEngine;

public class MonsterHealth : MonoBehaviour, IBeHit
{
    public int maxHp = 500;
    public int currentHp;
    public GameObject vectoryPanel;

    public bool showVictoryPanelOnDeath = true;

    public readonly GameplayContainer Container = new();

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

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
        if (currentHp <= 0) return;

        Container.Execute(data);

        // 只有弓箭(Arrow)和炮台(Cannon)触发受击动画
        if (data.from == "Arrow" || data.from == "Cannon")
        {
            var ui = GetComponent<MonsterUI>();
            ui?.PlayBeHit();
        }

        int finalDamage = Mathf.RoundToInt(data.damage);

        currentHp = Mathf.Max(0, currentHp - finalDamage);

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
