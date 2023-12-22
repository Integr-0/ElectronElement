using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreview : MonoBehaviour
{
    public enum InLobbyState
    {
        Connecting, NotReady, Ready
    }

    public TMP_Text GamertagText;
    public Image CharacterImg;
    public Image PlaceholderImg;
    public TMP_Text StateText;

    [Space, SerializeField] private Color connectingColor = Color.yellow;
    [SerializeField] private Color readyColor = Color.green;
    [SerializeField] private Color notReadyColor = Color.red;

    public void SetState(InLobbyState state)
    {
        switch (state)
        {
            case InLobbyState.Connecting:
                StateText.text = "Connecting...";
                StateText.color = connectingColor;
                break;
            case InLobbyState.NotReady:
                StateText.text = "Not Ready";
                StateText.color = notReadyColor;
                break;
            case InLobbyState.Ready:
                StateText.text = "Ready";
                StateText.color = readyColor;
                break;
            
        }
    }
}