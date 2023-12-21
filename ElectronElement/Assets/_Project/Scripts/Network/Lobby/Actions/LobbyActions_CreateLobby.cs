using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyActions_CreateLobby : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void CreateLobby()
    {
        try
        {
            master.Variables.load.Activate("Creating lobby", "Initializing lobby");

            string mapName = master.Variables.sceneDropdown.options[master.Variables.sceneIndex - LobbyVariables.SCENES_BEFORE_LEVELS].text;
            CreateLobbyOptions options = new()
            {
                IsPrivate = master.Variables.isHostedLobbyPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    { LobbyVariables.KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                    { LobbyVariables.KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                    { LobbyVariables.KEY_LOBBY_NAME, new DataObject(DataObject.VisibilityOptions.Member, master.Variables.lobbyName) },
                    { LobbyVariables.KEY_LOBBY_MAP, new DataObject(DataObject.VisibilityOptions.Public, mapName) }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(master.Variables.lobbyName, master.Variables.maxPlayers, options);
            master.Variables.load.MarkTaskCompleted();

            master.Variables.hostedLobby = lobby;
            master.Variables.joinedLobby = master.Variables.hostedLobby;

            Debug.Log("Created Lobby! " + lobby.LobbyCode);

            await master.LobbyPlayerData.WriteCurrentPlayerDataToJoinedLobby();

            master.LobbyUI.JoinLobby();

            if (master.Variables.codeText != null)
            {
                master.Variables.codeText.gameObject.SetActive(true);
                master.Variables.codeText.text = "Code: " + lobby.LobbyCode;
            }
            master.Variables.lobbyNameText.text = master.Variables.lobbyName;

            master.Variables.deleteLobbyButton.SetActive(true);
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleException(e);
        }
    }
}