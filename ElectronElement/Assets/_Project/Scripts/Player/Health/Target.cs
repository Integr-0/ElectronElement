using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Target : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        Health health = GetComponent<Health>();
        health.onHealthChanged += async (float dmgTaken, float currentHealth) =>
        {
            text.text = dmgTaken.ToString();
            await Task.Delay(1000);
            text.text = "";
        };
    }
}