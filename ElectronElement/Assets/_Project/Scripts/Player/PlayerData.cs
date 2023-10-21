using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string Name;

    public Camera cam;

    public FirstPersonMovement Movement;
    public MouseLook Look;

    public PauseMenu pauseMenu;

    public bool canPause;

    private void Awake()
    {
        CameraManager.Instance.testData = this;
    }

    public void Activate()
    {
        Movement.enabled = true;
        Look.enabled = true;
    }
    public void Deactivate()
    {
        Movement.enabled = false;
        Look.enabled = false;
    }

    private void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape)) pauseMenu.TogglePause();
    }
}