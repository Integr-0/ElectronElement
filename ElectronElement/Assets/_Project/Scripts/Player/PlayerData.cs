using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : NetworkBehaviour
{
    public struct Preferences
    {
        public float MouseSensitivity
        { 
            get
            {
                return PlayerPrefs.GetFloat("MouseSens", defaultValue: 300f);
            }
            set
            {
                PlayerPrefs.SetFloat("MouseSens", value);
            }
        }
    }
    public Preferences DefaultPrefs;

    [SerializeField] private Transform characterModelParent;

    public string Name;
    public Sprite Image;

    public Camera cam;

    public FirstPersonMovement Movement;
    public MouseLook Look;
    public GameObject gunParent;
    public ShelfLooter shelfLooter;

    public PauseMenu pauseMenu;

    public bool canPause = true;

    public override void OnNetworkSpawn()
    {
        if (IsClient) 
            GameManager.Instance.ClientStartGame();
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

    public void SetCharacter(int characterIndex)
    {
        characterModelParent.SetChildrenActive(false);

        characterModelParent.GetChild(characterIndex).gameObject.SetActive(true);
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