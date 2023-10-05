using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Vector2 lookSensitivity;

    private Transform cam;
    private Rigidbody rb;

    private DefaultControls controls;
    private void Awake()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        controls = new();
        controls.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Movement.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        controls.Movement.Jump.performed += _ => Jump();
        controls.Movement.Slide.performed += _ => Slide();
        controls.Movement.Sprint.performed += _ => Sprint();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Move(Vector2 dir)
    {
        Debug.Log("Move: " + dir);

        Vector2 move = dir * transform.forward * movementSpeed * Time.deltaTime;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.y);
    }
    private void Jump()
    {
        rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
    }
    private void Slide()
    {
        Debug.Log("Sliding");
    }
    private void Sprint()
    {
        Debug.Log("Sprinting");
    }
    private void Look(Vector2 dir)
    {
        Debug.Log("Look: " + dir);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.y + (dir.x * lookSensitivity.x), 0f);

        cam.rotation = Quaternion.Euler(transform.rotation.x + (dir.y * lookSensitivity.y), 0f, 0f);
    }
}