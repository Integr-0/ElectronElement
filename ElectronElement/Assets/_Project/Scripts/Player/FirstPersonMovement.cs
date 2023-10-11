using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Space, SerializeField] private float speed = 12f;
    [SerializeField] private float airSpeed = 8f;

    [Space, SerializeField] private float accelerationSpeed = 1f;
    [SerializeField] private float decelerationSpeed = 0.5f;

    [Space, SerializeField, Range(0, 1), Tooltip("(Only for Controller) Will only start moving when the stick is above a certain value from the center")]
    private float stickDeadzone;

    [Space, SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jump = 1f;

    [Space, SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [HideInInspector] public bool swimming { get => swimming; set { swimming = value; ToggleSwim(swimming); } }

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 movement;
    private bool isGrounded;
    private bool isSliding;
    private float currentGravity;

    private float accelerationValue = 0;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentGravity = gravity;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        isSliding = Input.GetKey(KeyCode.LeftControl);

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > stickDeadzone) movement = move;

        if (move.magnitude > stickDeadzone) accelerationValue += accelerationSpeed * Time.deltaTime;
        else if (!isSliding) accelerationValue -= decelerationSpeed * Time.deltaTime;

        accelerationValue = Mathf.Clamp(accelerationValue, 0f, 1f);

        movement *= accelerationValue;

        controller.Move((isGrounded ? speed : airSpeed) * accelerationValue * Time.deltaTime * movement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * currentGravity);
        }

        velocity.y += currentGravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void ToggleSwim(bool swimming)
    {
        Debug.Log("SwimCode");
        transform.Rotate(transform.right * (swimming ? 90f : 0f));
        currentGravity = swimming ? 0f : gravity;
    }
}