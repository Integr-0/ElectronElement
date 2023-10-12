using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public bool IsPaused {  get; private set; }

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

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Pause();
    }
}