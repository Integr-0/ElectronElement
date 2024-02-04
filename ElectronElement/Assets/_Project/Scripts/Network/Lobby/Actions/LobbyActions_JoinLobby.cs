using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyActions_JoinLobby : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;
    public async void JoinLobbyByCode(string code)
    {
        try
        {
            if (string.IsNullOrEmpty(code))
                throw new LobbyServiceException(LobbyExceptionReason.LobbyNotFound, "Code is empty");

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code);
            master.Variables.joinedLobby = lobby;

            string lobbyName = lobby.Data[LobbyVariables.KEY_LOBBY_NAME].Value;

            master.Variables.lobbyNameText.text = lobbyName;

            await master.LobbyPlayerData.WriteInitialPlayerDataToJoinedLobby();

            master.LobbyUI.JoinLobby();
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.LobbyNotFound || e.Reason == LobbyExceptionReason.InvalidJoinCode)
            {
                master.LobbyErrorHandler.ToggleObjectForPopupTime(master.Variables.wrongJoinCodeText);
            }
            else
            {
                master.LobbyErrorHandler.HandleRateLimits(e);
            }   
        }
    }
}