using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text dmgPopup;

    private void Awake()
    {
        Health health = GetComponent<Health>();

        slider.maxValue = health.maxHealth;
        slider.minValue = 0f;
        slider.value = health.startingHealth;
        healthText.text = $"{health.startingHealth}/{health.maxHealth}";

        health.onHealthChanged += (int damageTaken, int currentHealth) =>
        {
            healthText.text = $"{currentHealth}/{health.maxHealth}";
            slider.value = currentHealth;
        };
    }
}