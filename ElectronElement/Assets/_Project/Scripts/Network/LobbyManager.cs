using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviourSingleton<LobbyManager>
{
    private const int SCENES_BEFORE_LEVELS = 1; //Here it's only 'MAIN' that is before the levels in the build setting
    private const float LOBBY_UPDATE_POLL_FREQUENCY_SECONDS = 1.5f;
    private const float LOBBY_HEARTBEAT_TIMER_SECONDS = 15f;
    private const string KEY_START_GAME = "StartGame";
    private const string KEY_READY_PLAYERS = "ReadyPlayers";


    [SerializeField] private UnityEvent OnGameStarted;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text numPlayersText;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject deleteLobbyButton;

    [Space, SerializeField] private GameObject noQuickJoinLobbyFoundText;
    [Space, SerializeField] private GameObject wrongJoinCodeText;

    private Lobby hostedLobby;
    private Lobby joinedLobby;

    private bool isHostedLobbyPrivate = false;

    private float hearbeatTimer;
    private float updatePollTimer;

    private int sceneIndex = SCENES_BEFORE_LEVELS; //the first level in the build settings
    private int maxPlayers = 1;
    private string lobbyName = "Unnamed Lobby";

    private LoadingScreen load;


    private string gamertag = "Unnamed";
    private int characterIndex = 0;

    private int readyPlayers = 0;
    private bool isReady = false;

    private NetworkUIButtons.JoinData GetJoinData()
    {
        return new NetworkUIButtons.JoinData
        {
            Name = gamertag,
            CharacterIndex = characterIndex,
        };
    }

    private async void Start()
    {
        load = LoadingScreen.Instance;
        if (load == null) 
            throw new System.NullReferenceException("Custom: No LoadingScreenInstance found.\n" + 
                "If you are using the Awake method in the LoadingScreen script, please override the base.Awake and call that method");

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

    private void OnApplicationQuit()
    {
        if (joinedLobby != null)
            LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostedLobby != null)
        {
            hearbeatTimer -= Time.deltaTime;
            if (hearbeatTimer < 0f)
            {
                hearbeatTimer = LOBBY_HEARTBEAT_TIMER_SECONDS;

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
                updatePollTimer = LOBBY_UPDATE_POLL_FREQUENCY_SECONDS;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (numPlayersText != null)
                {
                    numPlayersText.gameObject.SetActive(true);
                    numPlayersText.text = "NumPlayers: " + lobby.Players.Count + "/" + lobby.MaxPlayers;
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
            load.Activate("Creating lobby", "Initializing lobby");

            CreateLobbyOptions options = new()
            {
                IsPrivate = isHostedLobbyPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                    { KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
                
            };
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            load.MarkTaskCompleted();
            
            hostedLobby = lobby;
            joinedLobby = hostedLobby;

            Debug.Log("Created Lobby! " + lobby.LobbyCode);        

            NetworkUIButtons.Instance.JoinLobby(GetJoinData());

            if (codeText != null)
            {
                codeText.gameObject.SetActive(true);
                codeText.text = "Code: " + lobby.LobbyCode;
            }

            deleteLobbyButton.SetActive(true);
        }
        catch (LobbyServiceException e)
        {
            HandleException(e);
        }
    }

    public async void JoinLobbyByCode(string code)
    {
        try
        {
            if (string.IsNullOrEmpty(code)) throw new LobbyServiceException(LobbyExceptionReason.LobbyNotFound, "Code is empty");

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code);
            joinedLobby = lobby;

            Debug.Log("Joined lobby! " + code);

            NetworkUIButtons.Instance.JoinLobby(GetJoinData());
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
                HandleException(e);
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

            NetworkUIButtons.Instance.JoinLobby(GetJoinData());
        }
        catch (LobbyServiceException e)
        {
            HandleException(e);
            if (e.Reason == LobbyExceptionReason.NoOpenLobbies)
            {
                noQuickJoinLobbyFoundText.SetActive(true);
                await Task.Delay(2000);
                noQuickJoinLobbyFoundText.SetActive(false);
            }
            else
            {
                //HandleException(e);
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
            HandleException(e);
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
            HandleException(e);
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
            HandleException(e);
        }
    }

    public async void StartGame()
    {
        if (hostedLobby != null)
        {
            try
            {
                Debug.Log("Starting Game...");

                load.Activate("Starting Game", "Loading Level", "Building Connection", "Informing Clients", "Spawning Players");

                
                SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
                load.MarkTaskCompleted();

                
                string relayCode = await RelayManager.Instance.CreateRelay();
                load.MarkTaskCompleted();


                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
                load.MarkTaskCompleted();

                joinedLobby = lobby;

                
                GameManager.Instance.HostStartGame();
                load.MarkTaskCompleted();
            }
            catch (LobbyServiceException e)
            {
                HandleException(e);
            }
        }
    }


    public void SetAccessibility(bool isPublic)
    {
        isHostedLobbyPrivate = !isPublic;
    }
    public void SetScene(int index)
    {
        sceneIndex = index + SCENES_BEFORE_LEVELS;
    }
    public void SetMaxPlayers(string max)
    {
        maxPlayers = int.Parse(max);
    }
    public void SetLobbyName(string name)
    {
        lobbyName = name;
    }

    public void SetPlayerGamertag(string name)
    {
        gamertag = name;
    }
    public void SetPlayerCharacterIndex(int i)
    {
        characterIndex = i;
    }
    public void ToggleIsReady(bool isReady)
    {
        this.isReady = isReady;


        if (joinedLobby != null)
        {
            readyPlayers = int.Parse(hostedLobby.Data[KEY_READY_PLAYERS].Value);
            readyPlayers += isReady ? 1 : -1;

            Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, readyPlayers.ToString()) }
                }
            });

            if (readyPlayers == hostedLobby.Players.Count)
            {
                StartGame();
            }
        }
    }


    private void HandleException(LobbyServiceException e)
    {
        Debug.LogWarning(e);

        if (e.ErrorCode == 50)
        {
            Debug.LogError("Too many lobby requests. Needs to be handled");
        }
    }
}