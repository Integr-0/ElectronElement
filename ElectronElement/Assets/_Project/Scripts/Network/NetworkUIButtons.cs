using UnityEngine.UI;
using UnityEngine;

public class NetworkUIButtons : MonoBehaviourSingleton<NetworkUIButtons>
{
    [System.Serializable]
    public struct JoinData
    {
        public string Name;
        public int CharacterIndex;
    }

    [Header("PlayerPreview Customization")]
    [SerializeField] private PlayerPreview playerPreviewPrefab;

    [Space, SerializeField] private Sprite[] characterImages;

    [Space, SerializeField] private Transform playerPreviewParent;

    [Header("Other")]
    [SerializeField] private GameObject hostParamPanel;
    [SerializeField] private GameObject inLobbyScreen;

    private string lobbyCodeInput;

    public void JoinLobbyWithInputtedCode()
    {
        LobbyManager.Instance.JoinLobbyByCode(lobbyCodeInput);
    }
    public void SetLobbyCodeInput(string c)
    {
        lobbyCodeInput = c;
    }

    public void OnLeaveLobby()
    {
        inLobbyScreen.SetActive(false);

        playerPreviewParent.DestroyChildren();
    }
    public void JoinLobby(JoinData data)
    {
        hostParamPanel.SetActive(false);

        inLobbyScreen.SetActive(true);

        var prev = Instantiate(playerPreviewPrefab, playerPreviewParent);

        prev.CharacterImg.sprite = characterImages[data.CharacterIndex];
        prev.GamertagText.text = data.Name;

        prev.SetState(PlayerPreview.InLobbyState.Connecting);
    }
}