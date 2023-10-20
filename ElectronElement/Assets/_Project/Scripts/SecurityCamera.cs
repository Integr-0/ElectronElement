using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private PlayerData posessingPlayer = null;

    [SerializeField] private GameObject cam;

    [Space, SerializeField] private float sensitivity = 100f;
    [Space, SerializeField] private Vector2 xMinMax;
    [SerializeField] private Vector2 yMinMax;

    public void Posess(PlayerData player)
    {
        player.Deactivate();
        player.cam.gameObject.SetActive(true);
        cam.SetActive(true);

        posessingPlayer = player;
    }

    public void Leave(PlayerData player)
    {
        player.Activate();
        player.cam.gameObject.SetActive(true);
        cam.SetActive(false);

        posessingPlayer = null;
    }

    public void TogglePosession(PlayerData player)
    {
        if(posessingPlayer == player) Leave(player);
        else Posess(player);
    }


    private float xRot;
    private float yRot;
    void Update()
    {
        if(posessingPlayer != null)
        {
            float MouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRot -= MouseY;
            yRot += MouseX;

            xRot = Mathf.Clamp(xRot, xMinMax.x, xMinMax.y);
            yRot = Mathf.Clamp(yRot, yMinMax.x, yMinMax.y);

            cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);

            /*
            cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(cam.transform.localRotation.x, xMinMax.x, xMinMax.y),
                                                           Mathf.Clamp(cam.transform.localRotation.y, yMinMax.x, yMinMax.y),
                                                           0f);*/
        }
    }
}