using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyActions_QuickJoinLobby : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;
    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            master.Variables.joinedLobby = lobby;

            string lobbyName = lobby.Data[LobbyVariables.KEY_LOBBY_NAME].Value;

            master.Variables.lobbyNameText.text = lobbyName;

            Debug.Log("QuickJoined lobby!");

            master.LobbyPlayerData.WriteCurrentPlayerDataToJoinedLobby();

            master.LobbyUI.JoinLobby();
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.NoOpenLobbies)
            {
                master.LobbyErrorHandler.ToggleObjectForPopupTime(master.Variables.noQuickJoinLobbyFoundText);
            }
            else
            {
                master.LobbyErrorHandler.HandleException(e);
            }
        }
    }
}
