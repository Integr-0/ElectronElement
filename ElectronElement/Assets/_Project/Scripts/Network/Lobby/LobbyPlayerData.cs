using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LobbyPlayerData : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async Task WriteInitialPlayerDataToJoinedLobby()
    {
        PlayerDataObject gamertagData = new(PlayerDataObject.VisibilityOptions.Member, master.Variables.gamertag);
        PlayerDataObject charIndexData = new(PlayerDataObject.VisibilityOptions.Member, master.Variables.characterIndex.ToString());
        PlayerDataObject isReadyData = new(PlayerDataObject.VisibilityOptions.Member, LobbyVariables.STRING_IS_READY_FALSE);

        try
        {
            UpdatePlayerOptions options = new()
            {
                Data = new Dictionary<string, PlayerDataObject>()
                {
                    { LobbyVariables.KEY_PLAYER_NAME, gamertagData },
                    { LobbyVariables.KEY_PLAYER_CHAR_INDEX, charIndexData },
                    { LobbyVariables.KEY_PLAYER_IS_READY, isReadyData }
                }
            };
            master.Variables.joinedLobby = await LobbyService.Instance.UpdatePlayerAsync(master.Variables.joinedLobby.Id, AuthenticationService.Instance.PlayerId, options);
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleException(e);
        }
    }
}