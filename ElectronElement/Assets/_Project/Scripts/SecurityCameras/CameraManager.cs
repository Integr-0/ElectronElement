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
        Instance = this;
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
        PlayerData playerNear = anyPlayerNear();

        if (Input.GetButtonDown("Cancel") && playerNear != null)
        {
            if (cam != null) ExitCurrentCam();
            else if(selectPanel.activeSelf)
            {
                playerNear.Activate();
                selectPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;

                playerNear.canPause = true;
            }
        }

        buttonPrompt.SetActive(playerNear != null && !selectPanel.activeSelf && cam == null);

        if (playerNear != null && Input.GetKeyDown(KeyCode.C) && cam == null)
        {
            playerNear.Deactivate();
            selectPanel.SetActive(true);
            playerNear.canPause = false;
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