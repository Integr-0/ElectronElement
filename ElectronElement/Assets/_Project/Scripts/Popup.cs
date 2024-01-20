using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private TMP_Text killedPlayerNameComponent;

    [HideInInspector] public string killedPlayerName;

    public void UpdateUI()
    {
        killedPlayerNameComponent.text = killedPlayerName;
    }
}