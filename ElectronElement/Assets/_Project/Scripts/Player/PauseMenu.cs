using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [Space, SerializeField] private Slider sensitivitySlider;
    [Space, SerializeField] private PlayerData data;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        sensitivitySlider.value = data.MouseSensitivity;
    }

    public void Pause()
    {
        data.Deactivate();

        panel.SetActive(true);
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Unpause()
    {
        data.Activate();

        panel.SetActive(false);
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