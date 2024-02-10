using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private WindowGroup controlGroup;
    [Space, SerializeField] private float turnDistance = 100f;
    [SerializeField] private float rotationSpeed;

    [Header("Tweening")]
    [SerializeField] private float zoomDuration = 10f;

    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        bool anyWindowActive = controlGroup.Windows.Any(go => go.activeSelf);
        if (NearEdge() && !anyWindowActive)
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

    public async Task ZoomToObject(Vector3 lookTarget, Vector3 moveTarget)
    {
        bool moveDone = false;
        bool lookDone = false;

        transform.DODynamicLookAt(lookTarget, zoomDuration).OnComplete(() => lookDone = true);
        transform.DOMove(moveTarget, zoomDuration).OnComplete(() => moveDone = true);

        while (!(moveDone && lookDone)) await Task.Yield();
    }
    public async void ResetZoom()
    {
        bool moveDone = false;
        bool lookDone = false;

        transform.DORotate(initialRotation, zoomDuration).OnComplete(() => lookDone = true);
        transform.DOMove(initialPosition, zoomDuration).OnComplete(() => moveDone = true);

        while (!(moveDone && lookDone)) await Task.Yield();
    }
}