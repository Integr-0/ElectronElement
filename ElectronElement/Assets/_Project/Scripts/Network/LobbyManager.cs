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
    private const string KEY_LOBBY_NAME = "Lobby Name";
    private const string KEY_LOBBY_MAP = "Lobby Map";

    private const string KEY_PLAYER_NAME = "PlayerName";
    private const string KEY_PLAYER_CHAR_INDEX = "CharacterIndex";

    private const int POPUP_ACTIVE_TIME_MILLISECONDS = 2000;


    [SerializeField] private UnityEvent OnGameStarted;

    [SerializeField] private Transform lobbyPreviewParent;
    [SerializeField] private LobbyPreview lobbyPreview;

    [Space, SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text numPlayersText;
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private GameObject deleteLobbyButton;

    [Space, SerializeField] private TMP_Dropdown sceneDropdown;

    [Space, SerializeField] private GameObject noQuickJoinLobbyFoundText;
    [SerializeField] private GameObject wrongJoinCodeText;

    [Space, SerializeField] private GameObject tooManyReqestsPopup;

    private Lobby hostedLobby;
    private Lobby joinedLobby;

    private bool isHostedLobbyPrivate = false;

    private float hearbeatTimer;
    private float updatePollTimer;

    private int sceneIndex = SCENES_BEFORE_LEVELS; //the first level in the build settings
    private int maxPlayers = 2;
    private string lobbyName = "Unnamed Lobby";

    private LoadingScreen load;


    private string gamertag = "Unnamed";
    private int characterIndex = 0;

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
            throw new System.NullReferenceException("No LoadingScreenInstance found");

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
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
        if (joinedLobby == null) return;

        updatePollTimer -= Time.deltaTime;

        if (updatePollTimer > 0f) return;

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


    public async void CreateLobby()
    {
        try
        {
            load.Activate("Creating lobby", "Initializing lobby");

            string mapName = sceneDropdown.options[sceneIndex - SCENES_BEFORE_LEVELS].text;
            Debug.Log(mapName == null);
            CreateLobbyOptions options = new()
            {
                IsPrivate = isHostedLobbyPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                    { KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, "0") },
                    { KEY_LOBBY_NAME, new DataObject(DataObject.VisibilityOptions.Member, lobbyName) },
                    { KEY_LOBBY_MAP, new DataObject(DataObject.VisibilityOptions.Public, mapName) }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            load.MarkTaskCompleted();

            hostedLobby = lobby;
            joinedLobby = hostedLobby;

            Debug.Log("Created Lobby! " + lobby.LobbyCode);

            WriteCurrentPlayerDataToJoinedLobby();

            NetworkUIButtons.Instance.JoinLobby();

            if (codeText != null)
            {
                codeText.gameObject.SetActive(true);
                codeText.text = "Code: " + lobby.LobbyCode;
            }
            lobbyNameText.text = lobbyName;

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

            string lobbyName = lobby.Data[KEY_LOBBY_NAME].Value;

            lobbyNameText.text = lobbyName;

            Debug.Log("Joined lobby! " + code);

            WriteCurrentPlayerDataToJoinedLobby();

            NetworkUIButtons.Instance.JoinLobby();
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.LobbyNotFound || e.Reason == LobbyExceptionReason.InvalidJoinCode)
            {
                wrongJoinCodeText.SetActive(true);
                await Task.Delay(POPUP_ACTIVE_TIME_MILLISECONDS);
                wrongJoinCodeText.SetActive(false);
            }
            else HandleException(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;

            string lobbyName = lobby.Data[KEY_LOBBY_NAME].Value;

            lobbyNameText.text = lobbyName;

            Debug.Log("QuickJoined lobby!");

            WriteCurrentPlayerDataToJoinedLobby();

            NetworkUIButtons.Instance.JoinLobby();
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.NoOpenLobbies)
            {
                noQuickJoinLobbyFoundText.SetActive(true);
                await Task.Delay(POPUP_ACTIVE_TIME_MILLISECONDS);
                noQuickJoinLobbyFoundText.SetActive(false);
            }
            else HandleException(e);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            List<QueryFilter> filters = new()
            {
                new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
            };
            List<QueryOrder> orders = new()
            {
                new(false, QueryOrder.FieldOptions.AvailableSlots)
            };
            QueryLobbiesOptions options = new()
            {
                Filters = filters,
                Order = orders,
            };
            var response = await Lobbies.Instance.QueryLobbiesAsync(options);

            if (response.Results.Count == 0)
            {
                Debug.LogWarning("No lobbies found. Please handle later");
                return;
            }

            lobbyPreviewParent.DestroyChildren();

            foreach (Lobby lobby in response.Results)
            {
                var prev = Instantiate(lobbyPreview, lobbyPreviewParent);

                string sceneName = lobby.Data[KEY_LOBBY_MAP].Value;

                prev.Init(lobby.Name, sceneName, lobby.Players.Count, lobby.MaxPlayers);
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
        if (hostedLobby == null) return;

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
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) },
                    
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
        try
        {
            maxPlayers = int.Parse(max);
        }
        catch (System.FormatException) { }
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

    private void WriteCurrentPlayerDataToJoinedLobby()
    {
        PlayerDataObject gamertagData = new(PlayerDataObject.VisibilityOptions.Member, gamertag);
        PlayerDataObject charIndexData = new(PlayerDataObject.VisibilityOptions.Member, characterIndex.ToString());

        //if is host just take the first dictionary
        if (hostedLobby != null)
        {
            SetValues(0);

            return;
        }

        int index = 0;
        while (joinedLobby.Players[index].Data != null) index++;

        SetValues(index);

        void SetValues(int index)
        {
            joinedLobby.Players[index].Data = new() { [KEY_PLAYER_NAME] = gamertagData }; //Initialize Dictionary with the gamertagData (so it isn't null)
            joinedLobby.Players[index].Data.Add(KEY_PLAYER_CHAR_INDEX, charIndexData); //Now we can add stuff normally
        }
    }

    public void ToggleIsReady(bool isReady)
    {
        if (joinedLobby != null)
        {
            int readyPlayers = int.Parse(joinedLobby.Data[KEY_READY_PLAYERS].Value);
            readyPlayers += isReady ? 1 : -1;

            Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_READY_PLAYERS, new DataObject(DataObject.VisibilityOptions.Member, readyPlayers.ToString()) }
                }
            });

            if (readyPlayers == joinedLobby.Players.Count)
            {
                Debug.Log("Reached start game");
                StartGame();
            }
        }
    }

    private void HandleException(LobbyServiceException e)
    {
        if (e.Message == "Rate limit has been exceeded") //Too many requests (couldn't find a better condition)
        {
            tooManyReqestsPopup.SetActive(true);
        }
        else Debug.LogWarning(e);
    }

    public void DisplayPreviews()
    {
        foreach (var player in joinedLobby.Players)
        {
            var data = player.Data;

            NetworkUIButtons.JoinData joinData = new()
            {
                CharacterIndex = int.Parse(data[KEY_PLAYER_CHAR_INDEX].Value),
                Name = data[KEY_PLAYER_NAME].Value,
            };

            NetworkUIButtons.Instance.InstantiatePlayerPreview(joinData);
        }
    }
}