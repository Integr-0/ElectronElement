using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyActions_StartGame : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public async void StartGame()
    {
        if (master.Variables.hostedLobby == null)
            return;

        try
        {
            Debug.Log("Starting Game...");

            master.Variables.load.Activate("Starting Game", "Loading Level", "Building Connection", "Informing Clients", "Spawning Players");
            Debug.Log($"Loading screen active for host");

            AsyncOperation op = SceneManager.LoadSceneAsync(master.Variables.sceneIndex, LoadSceneMode.Additive);
            while (!op.isDone) await Task.Yield();

            master.Variables.load.MarkTaskCompleted();

            string relayCode = await RelayManager.Instance.CreateRelay();
            master.Variables.load.MarkTaskCompleted();

            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(master.Variables.joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { LobbyVariables.KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                }
            });

            master.Variables.load.MarkTaskCompleted();

            master.Variables.joinedLobby = lobby;

            GameManager.Instance.HostStartGame();
            master.Variables.load.MarkTaskCompleted();
        }
        catch (LobbyServiceException e)
        {
            master.LobbyErrorHandler.HandleException(e);
        }
    }
}
