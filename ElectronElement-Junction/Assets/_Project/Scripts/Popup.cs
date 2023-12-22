using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Image killedPlayerImageComponent;
    [SerializeField] private TMP_Text killedPlayerNameComponent;

    public string killedPlayerName;
    public Sprite killedPlayerImage;

    public void UpdateUI()
    {
        killedPlayerNameComponent.text = killedPlayerName;
        killedPlayerImageComponent.sprite = killedPlayerImage;
    }
}