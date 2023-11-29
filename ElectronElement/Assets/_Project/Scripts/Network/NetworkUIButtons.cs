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

    //[Space, Space, SerializeField] private TMPro.TMP_InputField codeInputField;

    //[Space, SerializeField] private Button toggleHostPanelButton;
    //[SerializeField] private Button joinLobbyButton;
    //[SerializeField] private Button quickJoinLobbyButton;

    //[SerializeField] private GameObject leaveLobbyButton;
    //[SerializeField] private GameObject deleteLobbyButton;
    //[SerializeField] private GameObject codeDisplayText;
    //[SerializeField] private GameObject startGameButton;

    private string lobbyCodeInput;

    public void Host_TogglePanel()
    {
        hostParamPanel.SetActive(!hostParamPanel.activeSelf);
    }

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
        /*
        codeInputField.interactable = true;

        toggleHostPanelButton.interactable = true;
        joinLobbyButton.interactable = true;
        quickJoinLobbyButton.interactable = true;

        leaveLobbyButton.SetActive(false);
        startGameButton.SetActive(false);
        deleteLobbyButton.SetActive(false);
        codeDisplayText.SetActive(false);
        */

        inLobbyScreen.SetActive(false);

        playerPreviewParent.DestroyChildren();
    }
    public void JoinLobby(JoinData data)
    {
        /*
        hostParamPanel.SetActive(false);

        toggleHostPanelButton.interactable = false;

        codeInputField.interactable = false;

        joinLobbyButton.interactable = false;
        quickJoinLobbyButton.interactable = false;

        leaveLobbyButton.SetActive(true);
        */

        inLobbyScreen.SetActive(true);

        var prev = Instantiate(playerPreviewPrefab, playerPreviewParent);

        prev.CharacterImg.sprite = characterImages[data.CharacterIndex];
        prev.GamertagText.text = data.Name;

        prev.SetState(PlayerPreview.InLobbyState.Connecting);
    }
}