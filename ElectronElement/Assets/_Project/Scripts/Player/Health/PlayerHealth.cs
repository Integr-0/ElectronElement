using TMPro;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        var health = GetComponent<Health>();

        healthText.text = $"{health.startingHealth}/{health.maxHealth}";

        health.onHealthChanged += (changedFactor, newHealth) => healthText.text = $"{newHealth}/{health.maxHealth}";

        health.onDied += () =>
        {
            health.ResetHealth(false);
            transform.position = Vector3.zero;
        };
    }
}