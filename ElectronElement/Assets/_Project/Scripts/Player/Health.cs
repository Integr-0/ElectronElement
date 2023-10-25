using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public float maxHealth;
    public float startingHealth;
    public float currentHealth { get; protected set; }

    public Action onDied;
    public Action<float, float> onHealthChanged;

    private void Awake()
    {
        currentHealth = startingHealth;
        onDied += () => Destroy(gameObject);
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