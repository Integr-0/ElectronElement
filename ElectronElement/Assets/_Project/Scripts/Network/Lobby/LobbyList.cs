using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void ListLobbies()
    {
        try
        {
            List<QueryFilter> filters = new()
            {
                new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
            };
            List<QueryOrder> orders = new()
            {
                new(false, QueryOrder.FieldOptions.AvailableSlots)
            };
            QueryLobbiesOptions options = new()
            {
                Filters = filters,
                Order = orders,
            };
            var response = await Lobbies.Instance.QueryLobbiesAsync(options);

            if (response.Results.Count == 0)
            {
                Debug.LogWarning("No lobbies found. Please handle later");
                return;
            }

            master.Variables.lobbyPreviewParent.DestroyChildren();

            foreach (Lobby lobby in response.Results)
            {
                var prev = Instantiate(master.Variables.lobbyPreview, master.Variables.lobbyPreviewParent);

                string sceneName = lobby.Data[LobbyVariables.KEY_LOBBY_MAP].Value;

                prev.Init(lobby.Name, sceneName, lobby.Players.Count, lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleException(e);
        }
    }
}