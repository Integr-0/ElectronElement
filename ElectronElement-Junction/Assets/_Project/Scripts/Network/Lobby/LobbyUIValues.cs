using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

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
        if (master.Variables.joinedLobby != null)
        {
            int readyPlayers = int.Parse(master.Variables.joinedLobby.Data[LobbyVariables.KEY_READY_PLAYERS].Value);
            readyPlayers += isReady ? 1 : -1;

            Lobbies.Instance.UpdateLobbyAsync(master.Variables.joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { LobbyVariables.KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, readyPlayers.ToString()) }
                }
            });
        }
    }
}