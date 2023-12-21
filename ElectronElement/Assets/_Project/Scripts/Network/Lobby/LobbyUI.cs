using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public void JoinLobbyWithInputtedCode()
    {
        master.LobbyActions_JoinLobby.JoinLobbyByCode(master.Variables.lobbyCodeInput);
    }
    public void SetLobbyCodeInput(string c)
    {
        master.Variables.lobbyCodeInput = c;
    }

    public void OnLeaveLobby()
    {
        master.Variables.inLobbyScreen.SetActive(false);

        master.Variables.playerPreviewParent.DestroyChildren();
    }
    public void JoinLobby()
    {
        master.Variables.hostParamPanel.SetActive(false);

        master.Variables.inLobbyScreen.SetActive(true);

        LobbyManager.Instance.DisplayPreviews();
    }
}