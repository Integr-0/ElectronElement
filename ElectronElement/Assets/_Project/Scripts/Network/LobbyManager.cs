using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviourSingleton<LobbyManager>
{
    [SerializeField] private UnityEvent OnGameStarted;
    [SerializeField] private TMPro.TMP_Text codeDisplayText;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject deleteLobbyButton;

    [Space, SerializeField] private GameObject noQuickJoinLobbyFoundText;
    [Space, SerializeField] private GameObject wrongJoinCodeText;

    private Lobby hostedLobby;
    private Lobby joinedLobby;

    private bool isHostedLobbyPrivate = false;

    private float hearbeatTimer;
    private float updatePollTimer;

    private const int MAX_PLAYERS = 10;
    private const float LOBBY_UPDATE_POLL_FREQUENCY = 1.1f;
    private const string KEY_START_GAME = "StartGame";

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        OnGameStarted.AddListener(() =>
        {
            Debug.Log("Game Started");
        });

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostedLobby != null)
        {
            hearbeatTimer -= Time.deltaTime;
            if (hearbeatTimer < 0f)
            {
                hearbeatTimer = 15;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostedLobby.Id);
            }
        }
    }
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            updatePollTimer -= Time.deltaTime;
            if (updatePollTimer < 0f)
            {
                updatePollTimer = LOBBY_UPDATE_POLL_FREQUENCY;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (hostedLobby != null)
                {
                    startGameButton.SetActive(true);
                    codeDisplayText.text = $"Code: {lobby.LobbyCode}\nNumPlayers: {lobby.Players.Count}/{lobby.MaxPlayers}";
                }

                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if (hostedLobby == null)
                    {
                        RelayManager.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                        GameManager.Instance.ClientStartGame();
                    }

                    joinedLobby = null;

                    OnGameStarted?.Invoke();
                }
            }
        }
    }


    public async void CreateLobby()
    {
        try
        {
            CreateLobbyOptions options = new()
            {
                IsPrivate = isHostedLobbyPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                }
                
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("myLobby", MAX_PLAYERS, options);

            hostedLobby = lobby;
            joinedLobby = hostedLobby;

            Debug.Log("Created Lobby! " + lobby.LobbyCode);

            if (codeDisplayText != null)
            {
                codeDisplayText.text = "Code: " + lobby.LobbyCode + "\n NumPlayers: " + lobby.Players.Count + "/" + lobby.MaxPlayers;
                codeDisplayText.gameObject.SetActive(true);
            }

            deleteLobbyButton.SetActive(true);

            NetworkUIButtons.Instance.OnJoinLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void JoinLobbyByCode(string code)
    {
        try
        {
            if (!string.IsNullOrEmpty(code))
            {
                Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code);
                joinedLobby = lobby;

                Debug.Log("Joined lobby! " + code);

                NetworkUIButtons.Instance.OnJoinLobby();
            }
            else
            {
                wrongJoinCodeText.SetActive(true);
                await Task.Delay(2000);
                wrongJoinCodeText.SetActive(false);
            }
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.LobbyNotFound)
            {
                wrongJoinCodeText.SetActive(true);
                await Task.Delay(2000);
                wrongJoinCodeText.SetActive(false);
            }
            else
            {
                Debug.LogWarning(e);
            }
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;

            Debug.Log("QuickJoined lobby!");


            NetworkUIButtons.Instance.OnJoinLobby();
        }
        catch (LobbyServiceException e)
        {
            if(e.Reason == LobbyExceptionReason.NoOpenLobbies)
            {
                noQuickJoinLobbyFoundText.SetActive(true);
                await Task.Delay(2000);
                noQuickJoinLobbyFoundText.SetActive(false);
            }
            else
            {
                Debug.LogWarning(e);
            }
        }
    }

    public async void ListLobbies()
    {
        try
        {
            var response = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found " + response.Results.Count);
            foreach (var l in response.Results)
            {
                Debug.Log(l.Name + " " + l.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;


            NetworkUIButtons.Instance.OnLeaveLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

            hostedLobby = null;
            joinedLobby = null;


            NetworkUIButtons.Instance.OnLeaveLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
        }
    }

    public async void StartGame()
    {
        if (hostedLobby != null)
        {
            try
            {
                Debug.Log("Starting Game...");

                string relayCode = await RelayManager.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;

                GameManager.Instance.HostStartGame();
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning(e);
            }
        }
    }


    public void SetAccessibility(bool isPublic)
    {
        isHostedLobbyPrivate = !isPublic;
    }
}