using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private PlayerData data;

    private float xRotation = 0f;
    private float mouseSensitivity = 300f;

    void Start()
    {
        mouseSensitivity = data.DefaultPrefs.MouseSensitivity;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX);
    }

    public void SetSensitivity(float s)
    {
        mouseSensitivity = s;
        data.DefaultPrefs.MouseSensitivity = s;
    }
}
