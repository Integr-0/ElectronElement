using System;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public int maxHealth;
    public int startingHealth;
    public int currentHealth { get; private set; }

    public event Action onDied;
    /// <summary>
    /// First value: the health added/subtracted, 
    /// Second value: the new health
    /// </summary>
    public event Action<int, int> onHealthChanged;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    /// <summary>
    /// Positive values: Heal
    /// Negative values: Damage
    /// </summary>
    /// <param name="ammt"></param>
    public void ChangeHealth(int ammt)
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

    /// <summary>
    /// toMax true: set health to max health
    /// toMax false: set health to starting health
    /// </summary>
    /// <param name="toMax"></param>
    public void ResetHealth(bool toMax = false) => currentHealth = toMax ? maxHealth : startingHealth;
}