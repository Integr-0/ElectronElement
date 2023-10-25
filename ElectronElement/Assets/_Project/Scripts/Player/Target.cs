using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Target : Health
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        maxHealth = Mathf.Infinity;
        currentHealth = maxHealth;
        onDied -= () => Destroy(gameObject);
        onHealthChanged += async (float dmgTook, float currentHealth) =>
        {
            text.text = dmgTook.ToString();
            await Task.Delay(1000);
            text.text = "";
        };
    }
}