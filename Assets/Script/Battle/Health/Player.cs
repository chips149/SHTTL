using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;
using UnityEngine.UI;

// Player
public class Player : MonoBehaviour, IBeHit
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject failPanel;

    public Slider healthSlider;

    public readonly GameplayContainer Container = new();

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHpBar();
    }

    public void BeHit(BeHitData data)
    {
        // 走容器事件管线，允许肉鸽效果修改伤害
        Container.Execute(data);

        int finalDamage = Mathf.RoundToInt(data.damage);
        currentHealth = Mathf.Max(0, currentHealth - finalDamage);
        UpdateHpBar();

        if (currentHealth > 0) return;
        Die();
    }

    public void RemoveHp(RemoveHpData data)
    {
        // 直接扣血，不经过容器修饰
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
        
        currentHealth = Mathf.Max(0, currentHealth + takeHealEventData.healAmount);
        UpdateHpBar();

        if (currentHealth > 100) ;
    }

    private void Die()
    {
        failPanel.SetActive(true);
        Debug.Log("失败");
    }

    void UpdateHpBar()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }
}


