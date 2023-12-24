using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Authentication;

public class LobbyUIValues : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public void SetAccessibility(bool isPublic)
    {
        master.Variables.isHostedLobbyPrivate = !isPublic;
    }
    public void SetScene(int index)
    {
        master.Variables.sceneIndex = index + LobbyVariables.SCENES_BEFORE_LEVELS;
    }
    public void SetMaxPlayers(string max)
    {
        try
        {
            master.Variables.maxPlayers = int.Parse(max);
        }
        catch (System.FormatException) { }
    }
    public void SetLobbyName(string name)
    {
        master.Variables.lobbyName = name;
    }

    public void SetPlayerGamertag(string name)
    {
        master.Variables.gamertag = name;
    }
    public void SetPlayerCharacterIndex(int i)
    {
        master.Variables.characterIndex = i;
    }

    public void ToggleIsReady(bool isReady)
    {
        if (master.Variables.joinedLobby == null)
            return;

        string ready = isReady ? LobbyVariables.STRING_IS_READY_TRUE : LobbyVariables.STRING_IS_READY_FALSE;

        Debug.Log($"Joined ID: {master.Variables.joinedLobby.Id}, Player ID: {AuthenticationService.Instance.PlayerId}, Ready: {isReady}");

        LobbyService.Instance.UpdatePlayerAsync(master.Variables.joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                 { LobbyVariables.KEY_PLAYER_IS_READY, new(PlayerDataObject.VisibilityOptions.Member, ready) }
            }
        });
    }
}