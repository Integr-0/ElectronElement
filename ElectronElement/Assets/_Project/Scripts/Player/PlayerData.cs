using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : NetworkBehaviour
{
    public const string KEY_MOUSE_SENS = "MouseSens";
    public const string KEY_NAME = "PlayerName";

    [SerializeField] private Transform characterModelParent;

    public string Name
    {
        get => PlayerPrefs.GetString(KEY_NAME, defaultValue: "Unnamed");
        set => PlayerPrefs.SetString(KEY_NAME, value);
    }
    public float MouseSensitivity
    {
        get => PlayerPrefs.GetFloat(KEY_MOUSE_SENS, defaultValue: 300f);
        set => PlayerPrefs.SetFloat(KEY_MOUSE_SENS, value);
    }
    public static int CharacterIndex;

    public Camera cam;

    public FirstPersonMovement Movement;
    public MouseLook Look;
    public GameObject gunParent;
    public ShelfLooter shelfLooter;

    public PauseMenu pauseMenu;

    public bool canPause = true;

    private void Awake()
    {
        if (IsServer) SetCharacter();

        NetworkManager.OnClientConnectedCallback += (_) =>
        {
            if (!IsServer) // Only if you're a client (hosts are servers and clients)
            {
                GameManager.Instance.ClientStartGame();
                SetCharacter();
            }
        };
        NetworkManager.OnServerStopped += (_) =>
        {
            MainMenu();
        };
    }


    public void Activate()
    {
        Movement.enabled = true;
        Look.enabled = true;
        gunParent.SetActive(true);
    }
    public void Deactivate()
    {
        Movement.enabled = false;
        Look.enabled = false;
        gunParent.SetActive(false);
    }

    private void Update()
    {
        if (canPause && IsOwner && Input.GetKeyDown(KeyCode.Escape)) pauseMenu.TogglePause();
    }
    private void OnApplicationFocus(bool focussed)
    {
        if (!focussed && canPause && IsOwner) pauseMenu.Pause();
    }

    public void SetCharacter()
    {
        characterModelParent.SetChildrenActive(false);

        characterModelParent.GetChild(CharacterIndex).gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        if (!IsOwner) return;

        var load = LoadingScreen.Instance;
        load.Activate("Returning to Main Menu", "Leaving lobby", "Despawning Objects", "Loading Scene");

        load.MarkTaskCompleted();
        pauseMenu.Unpause();

        GameManager.Instance.ResetLobby();

        Cursor.lockState = CursorLockMode.None;

        NetworkManager.SceneManager.LoadScene("MAIN", LoadSceneMode.Single);

        load.MarkTaskCompleted();

        var allObjects = FindObjectsOfType<NetworkObject>();
        foreach (var obj in allObjects)
        {
            if (obj.IsSpawned) obj.Despawn();
        }

        load.MarkTaskCompleted();
    }
    public void Quit()
    {
        if (IsOwner) Application.Quit();
    }
}