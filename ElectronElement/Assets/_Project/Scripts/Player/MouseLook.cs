using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MouseLook : NetworkBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private PlayerData data;

    private float xRotation = 0f;
    private float mouseSensitivity = 300f;

    void Start()
    {
        mouseSensitivity = data.MouseSensitivity;

        Cursor.lockState = CursorLockMode.Locked;

        //disable all cameras that arent't owned by this player
        if (!IsOwner) GetComponent<Camera>().enabled = false;
    }

    void Update()
    {
        if (!IsOwner) return;

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
        data.MouseSensitivity = s;
    }
}
