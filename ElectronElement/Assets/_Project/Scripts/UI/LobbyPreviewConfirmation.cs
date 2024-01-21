using UnityEngine;

public class LobbyPreviewConfirmation : MonoBehaviour
{
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private LobbyActions_JoinLobby join;

    private string joinCode;
    public void ShowConfirmWindow(string code)
    {
        joinCode = code;
        confirmWindow.SetActive(true);
    }
    public void JoinWithCode()
    {
        join.JoinLobbyByCode(joinCode);
    }
}