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
    [SerializeField] private float moveDelta = 10f;

    private Vector3 initialPosition;
    private void Awake()
    {
        initialPosition = transform.position;
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

    public async Task ZoomToObject(Transform lookTarget, Transform moveTarget)
    {
        float lookDuration = Vector3.Distance(transform.position, moveTarget.position) / moveDelta;
        transform.DOLookAt(lookTarget.position, lookDuration);
        while (transform.position != moveTarget.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, moveDelta * Time.deltaTime);
            await Task.Yield();
        }
    }
    public async void ResetZoom()
    {
        while(transform.position != initialPosition)
            {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveDelta * Time.deltaTime);
            await Task.Yield();
        }
    }
}