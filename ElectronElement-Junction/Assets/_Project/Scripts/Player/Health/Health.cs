using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public float maxHealth;
    public float startingHealth;
    public float currentHealth { get; private set; }

    public event Action onDied;
    /// <summary>
    /// First value: the health added/subtracted
    /// Second value: the new health
    /// </summary>
    public event Action<float, float> onHealthChanged;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    /// <summary>
    /// Positive values: Heal
    /// Negative values: Damage
    /// </summary>
    /// <param name="ammt"></param>
    public void ChangeHealth(float ammt)
    {
        currentHealth += ammt;

        //Handle Damage
        if (currentHealth <= 0 && onDied != null)
        {
            onDied();
        }

        //Handle Healing
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        //Call event after clamping
        onHealthChanged?.Invoke(ammt, currentHealth);
    }
}