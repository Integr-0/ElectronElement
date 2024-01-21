using System.Threading.Tasks;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject buttonPrompt;
    [SerializeField] private GameObject camOverlay;

    [HideInInspector] public PlayerData data;
    [HideInInspector] public SecurityCamera[] allCams;

    private int? cam = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PosessCam(int i)
    {
        cam = i;
        allCams[i].Posess(data);
        selectPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        camOverlay.SetActive(true);
    }
    public void ExitCurrentCam()
    {
        allCams[(int)cam].Leave(data);
        selectPanel.SetActive(true);
        cam = null;
        Cursor.lockState = CursorLockMode.None;

        camOverlay.SetActive(false);
    }
    public void ChangeCamera(int i)
    {
        allCams[(int)cam].Leave(data);
        cam = i;
        allCams[i].Posess(data);
    }

    private async void Update()
    {
        PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(transform.position, out float distanceToClosestPlayer);

        bool anyPlayerNearEnough = distanceToClosestPlayer < 5f;
        data = nearestPlayer;

        if (Input.GetButtonDown("Cancel") && anyPlayerNearEnough)
        {
            if (cam != null) ExitCurrentCam();
            else if(selectPanel.activeSelf)
            {
                data.Activate();
                selectPanel.SetActive(false);

                await Task.Yield(); // so it doesn't immediately pause
                data.canPause = true;

                // this doesn't work
                // not sure if it's because of unity editor though
                // will still leave it here if it works in build
                Cursor.lockState = CursorLockMode.Locked; 
            }
        }

        bool canEnter = anyPlayerNearEnough && !selectPanel.activeSelf && cam == null && !nearestPlayer.pauseMenu.IsPaused;

        buttonPrompt.SetActive(canEnter);

        if (Input.GetKeyDown(KeyCode.C) && canEnter)
        {
            data.Deactivate();
            selectPanel.SetActive(true);
            data.canPause = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (cam != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeCamera(GetPreviousIndex());
            else if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeCamera(GetNextIndex());
        }
    }

    private int GetNextIndex() => ((int)cam + 1) % allCams.Length;
    private int GetPreviousIndex()
    {
        // simplified version: return (int)((cam + allCams.Length - 1) % allCams.Length);
        // will not be used as it's hard to debug

        if (cam == 0) return allCams.Length - 1;
        return (int)cam - 1;
    }
}