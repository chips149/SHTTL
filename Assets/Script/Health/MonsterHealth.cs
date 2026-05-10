using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    public int maxHp = 500;
    public int currentHp;
    public GameObject vectoryPanel;

    public Slider hpSlider;
    
    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHp <= 0) return;
        
        currentHp = Mathf.Max(0, currentHp - damage);
        UpdateHpBar();
        OnHealthChanged?.Invoke(currentHp, maxHp);
        

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
        vectoryPanel.SetActive(true);
    }

    void UpdateHpBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHp / maxHp;
        }
    }
}
