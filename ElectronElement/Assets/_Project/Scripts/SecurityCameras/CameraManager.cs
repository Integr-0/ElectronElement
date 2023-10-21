using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject buttonPrompt;

    [HideInInspector] public PlayerData testData;

    private int? cam = null;
    private void Awake()
    {
        Instance = this;
    }
    public SecurityCamera[] allCams;

    public void PosessCam(int i)
    {
        cam = i;
        allCams[i].Posess(anyPlayerNear() ?? testData);
        selectPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitCurrentCam()
    {
        allCams[(int)cam].Leave(testData);
        selectPanel.SetActive(true);
        cam = null;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        PlayerData playerNear = anyPlayerNear();

        if (Input.GetButtonDown("Cancel") && playerNear != null)
        {
            if (cam != null) ExitCurrentCam();
            else
            {
                playerNear.Activate();
                selectPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        buttonPrompt.SetActive(playerNear != null && !selectPanel.activeSelf && cam == null);

        if (playerNear != null && Input.GetKeyDown(KeyCode.C) && cam == null)
        {
            playerNear.Deactivate();
            selectPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
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
}