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

    public void Quit()
    {
        Application.Quit();
    }
}