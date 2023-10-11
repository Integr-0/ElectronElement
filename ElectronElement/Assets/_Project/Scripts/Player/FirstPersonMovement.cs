using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Space, SerializeField] private float speed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float airSpeed = 2f;

    [Space, SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jump = 1f;

    [Space, SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentGravity;
    private float currentSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentGravity = gravity;
        currentSpeed = speed;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : isGrounded ? speed : airSpeed;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(currentSpeed * Time.deltaTime * move);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * currentGravity);
        }

        velocity.y += currentGravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}