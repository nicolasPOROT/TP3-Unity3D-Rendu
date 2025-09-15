using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int attackPower = 10;

    [Header("UI")]
    public Slider healthBar; // UI Slider au-dessus du perso

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
            Debug.Log(healthBar.value);
    }

    public void Attack(CharacterStats target)
    {
        if (target != null)
        {
            target.TakeDamage(attackPower);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} est mort !"); // verif mort 
    }
}
