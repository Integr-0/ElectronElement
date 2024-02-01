using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private float turnDistance = 100f;
    [SerializeField] private float rotationSpeed;
    private void LateUpdate()
    {
        if (NearEdge())
        {
            transform.Rotate(Vector3.up, (Input.mousePosition.x > turnDistance ? 1 : -1f) * rotationSpeed * Time.deltaTime);
        }
    }

    private bool NearEdge()
    {
        bool nearRight = Screen.width - Input.mousePosition.x < turnDistance;
        bool nearLeft = Input.mousePosition.x < turnDistance;

        return nearLeft || nearRight;
    }
}