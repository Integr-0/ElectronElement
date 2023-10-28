using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject buttonPrompt;
    [SerializeField] private GameObject camOverlay;

    public PlayerData testData;

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
    public SecurityCamera[] allCams;

    public void PosessCam(int i)
    {
        cam = i;
        allCams[i].Posess(testData);
        selectPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        camOverlay.SetActive(true);
    }
    public void ExitCurrentCam()
    {
        allCams[(int)cam].Leave(testData);
        selectPanel.SetActive(true);
        cam = null;
        Cursor.lockState = CursorLockMode.None;

        camOverlay.SetActive(false);
    }
    public void ChangeCamera(int i)
    {
        allCams[(int)cam].Leave(testData);
        cam = i;
        allCams[i].Posess(testData);
    }

    private void Update()
    {
        PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(transform.position, out float distanceToClosestPlayer);
        bool anyPlayerNearEnough = distanceToClosestPlayer < 5f;

        if (Input.GetButtonDown("Cancel") && anyPlayerNearEnough)
        {
            if (cam != null) ExitCurrentCam();
            else if(selectPanel.activeSelf)
            {
                nearestPlayer.Activate();
                selectPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;

                nearestPlayer.canPause = true;
            }
        }

        buttonPrompt.SetActive(anyPlayerNearEnough && !selectPanel.activeSelf && cam == null);

        if (anyPlayerNearEnough && Input.GetKeyDown(KeyCode.C) && cam == null)
        {
            nearestPlayer.Deactivate();
            selectPanel.SetActive(true);
            nearestPlayer.canPause = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (cam != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeCamera(GetPreviousIndex());
            else if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeCamera(GetNextIndex());
        }
    }

    private PlayerData anyPlayerNear()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 5f)
            {
                return player.GetComponent<PlayerData>();
            }
        }
        return null;
    }

    private int GetNextIndex()
    {
        if (cam == allCams.Length - 1) return 0;
        return (int)cam + 1;
    }
    private int GetPreviousIndex()
    {
        if (cam == 0) return allCams.Length - 1;
        return (int)cam - 1;
    }
}