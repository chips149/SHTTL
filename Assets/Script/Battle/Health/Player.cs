using System;
using System.Collections.Generic;
using Framework.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Player
public class Player : MonoBehaviour, IBeHit
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject failPanel;

    public Slider healthSlider;
    public TMP_Text hpText;

    public readonly GameplayContainer Container = new();

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHpBar();
    }

    public void BeHit(BeHitData data)
    {
        Container.Execute(data);

        int finalDamage = Mathf.RoundToInt(data.damage);
        currentHealth = Mathf.Max(0, currentHealth - finalDamage);
        UpdateHpBar();

        if (currentHealth > 0) return;
        Die();
    }

    public void RemoveHp(RemoveHpData data)
    {
        currentHealth = Mathf.Max(0, currentHealth - Mathf.RoundToInt(data.damage));
        UpdateHpBar();

        if (currentHealth > 0) return;
        Die();
    }

    public void TakeDamage(int damage)
    {
        BeHit(new BeHitData { damage = damage, from = "DirectCall" });
    }

    public void TakeHeal(int healAmount)
    {
        var takeHealEventData = new TakeHealEventData()
        {
            healAmount = healAmount
        };
        Container.Execute(takeHealEventData);
        
        currentHealth = Mathf.Min(maxHealth, currentHealth + takeHealEventData.healAmount);
        UpdateHpBar();
    }

    private void Die()
    {
        failPanel.SetActive(true);
    }

    /// <summary>
    /// 复活按钮 OnClick 调用，回满血继续关卡
    /// </summary>
    public void Revive()
    {
        currentHealth = maxHealth;
        UpdateHpBar();
        failPanel.SetActive(false);
    }

    void UpdateHpBar()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        if (hpText != null)
            hpText.text = $"{currentHealth}/{maxHealth}";
    }
}


