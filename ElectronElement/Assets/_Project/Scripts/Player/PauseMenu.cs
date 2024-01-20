using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject HUD;
    [Space, SerializeField] private Slider sensitivitySlider;
    [Space, SerializeField] private PlayerData data;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        sensitivitySlider.value = data.MouseSensitivity;
    }

    public void Pause()
    {
        data.cam.GetComponent<MouseLook>().enabled = false;

        panel.SetActive(true);
        HUD.SetActive(false);
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Unpause()
    {
        data.cam.GetComponent<MouseLook>().enabled = true;

        panel.SetActive(false);
        HUD.SetActive(true);
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            IsPaused = false;
            Unpause();
        }
        else
        {
            IsPaused = true;
            Pause();
        }
    }
}