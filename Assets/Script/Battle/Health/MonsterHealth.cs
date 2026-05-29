using System;
using Framework.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour, IBeHit
{
    public int maxHp = 500;
    public int currentHp;
    public GameObject vectoryPanel;

    public Slider hpSlider;
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
        UpdateHpBar();
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void BeHit(BeHitData data)
    {
        if (currentHp <= 0) return;

        // 走容器事件管线，允许肉鸽效果修改伤害
        Container.Execute(data);

        int finalDamage = Mathf.RoundToInt(data.damage);
        currentHp = Mathf.Max(0, currentHp - finalDamage);
        UpdateHpBar();
        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void RemoveHp(RemoveHpData data)
    {
        if (currentHp <= 0) return;

        // 直接扣血，不经过容器修饰（用于 Dot 等持续伤害）
        currentHp = Mathf.Max(0, currentHp - Mathf.RoundToInt(data.damage));
        UpdateHpBar();
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

    private void UpdateHpBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHp / maxHp;
        }
    }
}
