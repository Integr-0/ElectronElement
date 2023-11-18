using UnityEngine.UI;
using UnityEngine;

public class NetworkUIButtons : MonoBehaviourSingleton<NetworkUIButtons>
{
    [SerializeField] private GameObject hostParamPanel;

    [SerializeField] private TMPro.TMP_InputField codeInputField;

    [SerializeField] private Button toggleHostPanelButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button quickJoinLobbyButton;

    [SerializeField] private GameObject leaveLobbyButton;
    [SerializeField] private GameObject deleteLobbyButton;
    [SerializeField] private GameObject codeDisplayText;
    [SerializeField] private GameObject startGameButton;

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
        codeInputField.interactable = true;

        toggleHostPanelButton.interactable = true;
        joinLobbyButton.interactable = true;
        quickJoinLobbyButton.interactable = true;

        leaveLobbyButton.SetActive(false);
        startGameButton.SetActive(false);
        deleteLobbyButton.SetActive(false);
        codeDisplayText.SetActive(false);
    }
    public void OnJoinLobby()
    {
        hostParamPanel.SetActive(false);

        toggleHostPanelButton.interactable = false;

        codeInputField.interactable = false;

        joinLobbyButton.interactable = false;
        quickJoinLobbyButton.interactable = false;

        leaveLobbyButton.SetActive(true);
    }
}