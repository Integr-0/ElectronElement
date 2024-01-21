using TMPro;
using UnityEngine;

public class LobbyPreview : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text mapText;
    [SerializeField] private TMP_Text numPlayersText;

    public void Init(string name, string mapName, int numPlayers, int maxPlayers, string joinCode)
    {
        nameText.text = name;
        numPlayersText.text = $"Players: {numPlayers}/{maxPlayers}";
        mapText.text = "Map: " + mapName;

        transform.parent.GetComponent<LobbyPreviewConfirmation>().ShowConfirmWindow(joinCode);
    }
}