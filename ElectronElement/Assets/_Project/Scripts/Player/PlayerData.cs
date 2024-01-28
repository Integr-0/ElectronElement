using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : NetworkBehaviour
{
    public const string KEY_MOUSE_SENS = "MouseSens";
    public const string KEY_NAME = "PlayerName";

    [SerializeField] private Transform characterModelParent;
    [SerializeField] private GameObject hostDisconnectPopup;
    public GameObject hud;

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

    public Camera cam;

    public FirstPersonMovement Movement;
    public MouseLook Look;
    public GameObject gunParent;
    public ShelfLooter shelfLooter;

    public PauseMenu pauseMenu;

    public bool canPause = true;

    private void Awake()
    { 
        NetworkManager.OnClientConnectedCallback += (_) =>
        {
            SetCharacter();
            if (!IsServer) // Only if you're a client (hosts are servers and clients)
            {
                GameManager.Instance.ClientStartGame();
            }
        };
        NetworkManager.OnServerStopped += async (_) =>
        {
            if (!IsServer)
            {
                hud.SetActive(false);
                hostDisconnectPopup.SetActive(true);
                await Task.Delay(500);
                hud.SetActive(true);
                hostDisconnectPopup.SetActive(false);

                MainMenu();
            }
        };
    }


    public void Activate()
    {
        Movement.enabled = true;
        Look.enabled = true;
        hud.SetActive(true);
        gunParent.SetActive(true);
    }
    public void Deactivate()
    {
        Movement.enabled = false;
        Look.enabled = false;
        hud.SetActive(false);
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

        characterModelParent.GetChild(GameManager.Instance.characterIndex).gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        if (!IsOwner) return;

        pauseMenu.Unpause();
        GameManager.Instance.ResetLobby();
        Cursor.lockState = CursorLockMode.None;

        var load = LoadingScreen.Instance;
        load.Activate("Returning to Main Menu", "Despawning Objects", "Loading Scene");


        foreach (var obj in FindObjectsOfType<NetworkObject>())
        {
            if (obj.IsSpawned) obj.Despawn();
        }

        load.MarkTaskCompleted();


        NetworkManager.SceneManager.LoadScene("MAIN", LoadSceneMode.Single);

        load.MarkTaskCompleted();
    }
    public void Quit()
    {
        if (IsOwner) Application.Quit();
    }
}