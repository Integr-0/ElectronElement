using UnityEngine;

public class PlayerData : MonoBehaviour
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

    public string Name;
    public Sprite Image;

    public Camera cam;

    public FirstPersonMovement Movement;
    public MouseLook Look;
    public GameObject gunParent;
    public ShelfLooter shelfLooter;

    public PauseMenu pauseMenu;

    public bool canPause = true;

    private void Awake()
    {
        if (CameraManager.Instance != null) CameraManager.Instance.testData = this;
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
        if (canPause && Input.GetKeyDown(KeyCode.Escape)) pauseMenu.TogglePause();
    }
    private void OnApplicationFocus(bool focussed)
    {
        if (!focussed && canPause) pauseMenu.Pause();
    }

    public void MainMenu()
    {
        var load = LoadingScreen.Instance;
        load.Activate("Returning to Main Menu", "Leaving lobby, Loading Scene");

        load.StartTask();
        //LobbyManager.Instance.LeaveLobby();
        load.MarkTaskCompleted();

        load.StartTask();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN");
        load.MarkTaskCompleted();

        pauseMenu.Unpause();
    }
    public void Quit()
    {
        Application.Quit();
    }
}