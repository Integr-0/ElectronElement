using TMPro;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (!IsOwner) return;

        var health = GetComponent<Health>();

        healthText.text = $"{health.startingHealth}/{health.maxHealth}";

        health.onHealthChanged += (changedFactor, newHealth) => healthText.text = $"{newHealth}/{health.maxHealth}";

        health.onDied += () =>
        {
            health.ResetHealth();

            transform.position = Vector3.zero; // only works once for some reason
        };
    }
}