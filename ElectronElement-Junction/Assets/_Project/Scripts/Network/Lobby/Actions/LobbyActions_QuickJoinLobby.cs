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

            string lobbyName = lobby.Data[LobbyVariables.KEY_LOBBY_NAME].Value;
            master.Variables.joinedLobby = lobby;

            master.Variables.lobbyNameText.text = lobbyName;

            await master.LobbyPlayerData.WriteInitialPlayerDataToJoinedLobby();

            master.startLobbyUpdates = true;

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
