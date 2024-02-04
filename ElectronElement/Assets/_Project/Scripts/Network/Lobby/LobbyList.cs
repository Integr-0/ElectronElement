using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void ListLobbies(bool usedWithButton)
    {
        try
        {
            // Filter only lobbies that have at least 1 free slot
            List<QueryFilter> filters = new()
            {
                new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
            };
            // Order the results by how many remaining slots there are
            List<QueryOrder> orders = new()
            {
                new(false, QueryOrder.FieldOptions.AvailableSlots)
            };
            // combining the filters and orders into the options
            QueryLobbiesOptions options = new()
            {
                Filters = filters,
                Order = orders,
            };
            var response = await Lobbies.Instance.QueryLobbiesAsync(options);

            master.Variables.lobbyPreviewParent.DestroyChildren();


            if (response.Results.Count == 0)
            {
                if (usedWithButton) 
                    master.LobbyErrorHandler.ToggleObjectForPopupTime(master.Variables.noLobbiesFoundPopup);

                return;
            }

            foreach (Lobby lobby in response.Results)
            {
                var prev = Instantiate(master.Variables.lobbyPreview, master.Variables.lobbyPreviewParent);

                string sceneName = lobby.Data[LobbyVariables.KEY_LOBBY_MAP].Value;

                prev.Init(lobby.Name, sceneName, lobby.Players.Count, lobby.MaxPlayers, lobby.Data[LobbyVariables.KEY_LOBBY_CODE].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleRateLimits(e);
        }
    }
}