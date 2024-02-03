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
        if (IsPaused) return;

        data.Deactivate();

        panel.SetActive(true);
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Unpause()
    {
        if (!IsPaused) return;

        data.Activate();

        panel.SetActive(false);
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            Unpause();
            IsPaused = false;
        }
        else
        {
            Pause();
            IsPaused = true;
        }
    }
}