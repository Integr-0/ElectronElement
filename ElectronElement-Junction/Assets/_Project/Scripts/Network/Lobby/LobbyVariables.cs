using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class LobbyVariables : MonoBehaviour
{
    public const int SCENES_BEFORE_LEVELS = 1; //Here it's only 'MAIN' that is before the levels in the build settings
    public const float LOBBY_UPDATE_POLL_FREQUENCY_SECONDS = 1.5f;
    public const float LOBBY_HEARTBEAT_TIMER_SECONDS = 15f;
    public const float LOBBY_LIST_DURATION_SECONDS = 30f;

    public const string KEY_START_GAME = "StartGame";
    public const string KEY_READY_PLAYERS = "Ready Players";
    public const string KEY_LOBBY_NAME = "Lobby Name";
    public const string KEY_LOBBY_MAP = "Lobby Map";

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHAR_INDEX = "CharacterIndex";

    public const int POPUP_ACTIVE_TIME_MILLISECONDS = 2000;


    public UnityEvent OnGameStarted;

    public Transform lobbyPreviewParent;
    public LobbyPreview lobbyPreview;

    [Space] public TMP_Text codeText;
    public TMP_Text numPlayersText;
    public TMP_Text lobbyNameText;
    public GameObject deleteLobbyButton;

    [Space] public TMP_Dropdown sceneDropdown;

    [Space] public GameObject noQuickJoinLobbyFoundText;
    public GameObject wrongJoinCodeText;

    [Space] public GameObject tooManyReqestsPopup;
    public GameObject noLobbiesFoundPopup;

    [Space] public LoadingScreen load;


    public PlayerPreview playerPreviewPrefab;

    [Space] public Sprite[] characterImages;

    [Space] public Transform playerPreviewParent;

    [Space] public GameObject hostParamPanel;
    public GameObject inLobbyScreen;


    [HideInInspector] public Lobby hostedLobby;
    [HideInInspector] public Lobby joinedLobby;

    [HideInInspector] public bool isHostedLobbyPrivate = false;

    [HideInInspector] public float hearbeatTimer;
    [HideInInspector] public float updatePollTimer;
    [HideInInspector] public float listLobbyTimer;

    [HideInInspector] public int sceneIndex = SCENES_BEFORE_LEVELS; //the first level in the build settings
    [HideInInspector] public int maxPlayers = 2;
    [HideInInspector] public string lobbyName = "Unnamed Lobby";


    [HideInInspector] public string gamertag = "Unnamed";
    [HideInInspector] public int characterIndex = 0;

    [HideInInspector] public string lobbyCodeInput;
}