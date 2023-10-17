using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private PlayerData posessingPlayer = null;

    [SerializeField] private GameObject cam;
    public void Posess(PlayerData player)
    {
        player.Deactivate();
        cam.SetActive(true);

        posessingPlayer = player;
    }

    public void Leave(PlayerData player)
    {
        player.Activate();
        cam.SetActive(false);

        posessingPlayer = null;
    }

    public void TogglePosession(PlayerData player)
    {
        if(posessingPlayer == player) Leave(player);
        else Posess(player);
    }

    [SerializeField] private float sensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Update()
    {
        if(posessingPlayer != null)
        {
            float MouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= MouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 0f);

            yRotation += MouseX;
            yRotation = Mathf.Clamp(yRotation, 25f, 155f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}