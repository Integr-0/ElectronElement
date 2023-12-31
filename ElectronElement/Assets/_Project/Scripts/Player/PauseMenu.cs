using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [Space, SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerData data;

    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        sensitivitySlider.value = data.DefaultPrefs.MouseSensitivity;
    }

    public void Pause()
    {
        panel.SetActive(true);
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
    public void Unpause()
    {
        panel.SetActive(false);
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
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

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && data.canPause) Pause();
    }
}