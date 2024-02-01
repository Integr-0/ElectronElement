using TMPro;
using UnityEngine;

public class LobbyPreview : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    private string joinCode;

    public void Init(string name, string mapName, int numPlayers, int maxPlayers, string joinCode)
    {
        infoText.text = $"{name} | {numPlayers}/{maxPlayers} | {mapName}";

        this.joinCode = joinCode;
    }
    public void ShowConfirmationWindow()
    {
        transform.parent.GetComponent<LobbyPreviewConfirmation>().ShowConfirmWindow(joinCode);
    }
}