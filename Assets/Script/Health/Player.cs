using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;
using UnityEngine.UI;

// Player
public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject failPanel;

    public Slider healthSlider;

    public readonly GameplayContainer Container = new GameplayContainer();

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHpBar();
    }

    public void TakeDamage(int damage)
    {
        // 肉鸽
        var takeDamageEventData = new TakeDamageEventData()
        {
            damage = damage
        };
        Container.Execute(takeDamageEventData);
        
        currentHealth = Mathf.Max(0, currentHealth - takeDamageEventData.damage);
        UpdateHpBar();

        if (currentHealth > 0) return;
        Die();
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

        if (currentHealth > 100) return;
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


