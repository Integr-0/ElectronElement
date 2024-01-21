using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private PlayerData posessingPlayer = null;

    [SerializeField] private GameObject cam;

    [SerializeField] private float zoomSensitivity = 1f;

    [Space, SerializeField] private Vector2 xMinMax;
    [SerializeField] private Vector2 yMinMax;
    [SerializeField] private Vector2 zoomFOVMinMax;

    public void Posess(PlayerData player)
    {
        player.Deactivate();
        player.cam.gameObject.SetActive(false);
        cam.SetActive(true);

        posessingPlayer = player;
    }

    public void Leave(PlayerData player)
    {
        player.cam.gameObject.SetActive(true);
        cam.SetActive(false);

        posessingPlayer = null;
    }

    private float xRot = 0f;
    private float yRot = 90f;
    private float zoom = 60f;
    void Update()
    {
        if (posessingPlayer != null)
        {
            float sensitivity = posessingPlayer.MouseSensitivity;
            float MouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            float Scroll = Input.mouseScrollDelta.y * zoomSensitivity;

            xRot -= MouseY;
            yRot += MouseX;
            zoom -= Scroll;

            xRot = Mathf.Clamp(xRot, xMinMax.x, xMinMax.y);
            yRot = Mathf.Clamp(yRot, yMinMax.x, yMinMax.y);
            zoom = Mathf.Clamp(zoom, zoomFOVMinMax.x, zoomFOVMinMax.y);

            cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);

            cam.GetComponent<Camera>().fieldOfView = zoom;
        }
    }
}