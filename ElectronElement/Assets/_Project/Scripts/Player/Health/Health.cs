using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public float maxHealth;
    public float startingHealth;
    public float currentHealth { get; private set; }

    public event Action onDied;
    public event Action<float, float> onHealthChanged;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    /// <summary>
    /// Use negative values for healing
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //for damage
        if (currentHealth <= 0)
        {
            onDied();
        }

        //For healing
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        //apply after clamping
        onHealthChanged(-damage, currentHealth);
    }
}