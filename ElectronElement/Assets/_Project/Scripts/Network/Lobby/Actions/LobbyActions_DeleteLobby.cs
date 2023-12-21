using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyActions_DeleteLobby : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(master.Variables.joinedLobby.Id);

            master.Variables.hostedLobby = null;
            master.Variables.joinedLobby = null;

            NetworkUIButtons.Instance.OnLeaveLobby();
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleException(e);
        }
    }
}