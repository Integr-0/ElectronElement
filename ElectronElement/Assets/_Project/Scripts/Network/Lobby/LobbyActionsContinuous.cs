using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class LobbyActionsContinuous : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    private bool gameStarted = false;
    public async void HandleLobbyHeartbeat()
    {
        if (master.Variables.hostedLobby == null)
            return;
        
            master.Variables.hearbeatTimer -= Time.deltaTime;
        if (master.Variables.hearbeatTimer > 0f)
            return;

        master.Variables.hearbeatTimer = LobbyVariables.LOBBY_HEARTBEAT_TIMER_SECONDS;

        await LobbyService.Instance.SendHeartbeatPingAsync(master.Variables.hostedLobby.Id);
    }
    public async void HandleLobbyPollForUpdates()
    {
        if (master.Variables.joinedLobby == null)
            return;

        master.Variables.updatePollTimer -= Time.deltaTime;

        if (master.Variables.updatePollTimer > 0f)
            return;

        master.Variables.updatePollTimer = LobbyVariables.LOBBY_UPDATE_POLL_FREQUENCY_SECONDS;

        var lobby = await LobbyService.Instance.GetLobbyAsync(master.Variables.joinedLobby.Id);
        master.Variables.joinedLobby = lobby;

        master.LobbyPreviewSystem.DisplayPreviews();

        if (master.Variables.numPlayersText != null)
        {
            master.Variables.numPlayersText.gameObject.SetActive(true);
            master.Variables.numPlayersText.text = $"NumPlayers: {lobby.Players.Count}/{lobby.MaxPlayers}";
        }

        if (AllPlayersReady() && !gameStarted)
        {
            master.Variables.load.Activate("Joining Game", "Waiting for players");

            master.LobbyActions_StartGame.StartGame();
            gameStarted = true;
        }

        if (master.Variables.joinedLobby.Data[LobbyVariables.KEY_START_GAME].Value != "0")
        {
            if (master.Variables.hostedLobby == null)
            {
                RelayManager.Instance.JoinRelay(master.Variables.joinedLobby.Data[LobbyVariables.KEY_START_GAME].Value);
            }

            master.Variables.joinedLobby = null;

            master.Variables.OnGameStarted?.Invoke();
        }


        bool AllPlayersReady()
        {
            foreach (Player player in master.Variables.joinedLobby.Players)
            {
                if (player.Data == null)
                    return false;

                if (!player.Data.TryGetValue(LobbyVariables.KEY_PLAYER_IS_READY, out var data))
                    return false;

                if (data.Value == LobbyVariables.STRING_IS_READY_FALSE)
                    return false;
            }
            return true;
        }
    }

    public void HandleLobbyListing()
    {
        if (master.Variables.joinedLobby != null) //Only list when not in a lobby
            return;

        master.Variables.listLobbyTimer -= Time.deltaTime;

        if (master.Variables.listLobbyTimer > 0f)
            return;

        master.Variables.listLobbyTimer = LobbyVariables.LOBBY_LIST_DURATION_SECONDS;

        master.LobbyList.ListLobbies(usedWithButton: false);
    }
}