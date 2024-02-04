using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyActions_LeaveLobby : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(master.Variables.joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            master.Variables.joinedLobby = null;

            master.LobbyUI.OnLeaveLobby();
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleRateLimits(e);
        }
    }
}