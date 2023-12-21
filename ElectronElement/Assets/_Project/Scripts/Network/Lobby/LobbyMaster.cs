using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyMaster : MonoBehaviour
{
    public LobbyActions_StartGame LobbyActions_StartGame;
    public LobbyActions_JoinLobby LobbyActions_JoinLobby;
    public LobbyUI LobbyUI;
    public LobbyUIValues LobbyUIValues;
    public LobbyList LobbyList;
    public LobbyActionsContinuous LobbyActionsContinuous;
    public LobbyPlayerData LobbyPlayerData;
    public LobbyErrorHandler LobbyErrorHandler;
    public LobbyVariables Variables;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        Variables.OnGameStarted.AddListener(() =>
        {
            Debug.Log("Game Started");
        });

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        LobbyActionsContinuous.HandleLobbyHeartbeat();
        LobbyActionsContinuous.HandleLobbyPollForUpdates();
    }

    private void OnApplicationQuit()
    {
        if (Variables.joinedLobby != null)
            LobbyService.Instance.RemovePlayerAsync(Variables.joinedLobby.Id, AuthenticationService.Instance.PlayerId);
    }
}