using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        Health health = GetComponent<Health>();

        slider.maxValue = health.maxHealth;
        slider.minValue = 0f;
        slider.value = health.startingHealth;
        healthText.text = $"{health.startingHealth}/{health.maxHealth}";

        health.onHealthChanged += (float damageTaken, float currentHealth) =>
        {
            healthText.text = $"{currentHealth}/{health.maxHealth}";
            slider.value = currentHealth;
        };
    }
}