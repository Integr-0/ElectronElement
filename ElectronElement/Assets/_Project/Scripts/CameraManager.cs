using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public SecurityCamera[] allCams;
}