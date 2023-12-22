using Unity.Services.Lobbies;
using UnityEngine;

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
            master.Variables.numPlayersText.text = "NumPlayers: " + lobby.Players.Count + "/" + lobby.MaxPlayers;
        }

        int readyPlayers = int.Parse(lobby.Data[LobbyVariables.KEY_READY_PLAYERS].Value);
        if (readyPlayers >= master.Variables.joinedLobby.Players.Count && !gameStarted)
        {
            master.LobbyActions_StartGame.StartGame();
            gameStarted = true;
        }

        if (master.Variables.joinedLobby.Data[LobbyVariables.KEY_START_GAME].Value != "0")
        {
            if (master.Variables.hostedLobby == null)
            {
                RelayManager.Instance.JoinRelay(master.Variables.joinedLobby.Data[LobbyVariables.KEY_START_GAME].Value);
                GameManager.Instance.ClientStartGame();
            }

            master.Variables.joinedLobby = null;

            master.Variables.OnGameStarted?.Invoke();
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