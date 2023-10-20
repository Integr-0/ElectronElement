using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 300f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    public const string KEY_SENSITIVITY = "mouse_sens";

    void Start()
    {
        if (PlayerPrefs.GetFloat(KEY_SENSITIVITY) == default) 
            PlayerPrefs.SetFloat(KEY_SENSITIVITY, 300f);

        mouseSensitivity = PlayerPrefs.GetFloat(KEY_SENSITIVITY);

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
        PlayerPrefs.SetFloat(KEY_SENSITIVITY, s);
    }
}
