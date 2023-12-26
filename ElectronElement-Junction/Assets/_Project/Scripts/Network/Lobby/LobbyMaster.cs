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
    public LobbyPreviewSystem LobbyPreviewSystem;
    public LobbyVariables Variables;

    private bool startList = false;

    public async void OneTimeInit()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
            startList = true;
        };
        Variables.OnGameStarted.AddListener(() =>
        {
            Debug.Log("Game Started");
        });

#if UNITY_EDITOR
        AuthenticationService.Instance.ClearSessionToken();
#endif

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        if (!startList) return;
        LobbyActionsContinuous.HandleLobbyListing();

        LobbyActionsContinuous.HandleLobbyHeartbeat();
        LobbyActionsContinuous.HandleLobbyPollForUpdates();
    }

    private void OnApplicationQuit()
    {
        if (Variables.joinedLobby != null)
            LobbyService.Instance.RemovePlayerAsync(Variables.joinedLobby.Id, AuthenticationService.Instance.PlayerId);
    }
}