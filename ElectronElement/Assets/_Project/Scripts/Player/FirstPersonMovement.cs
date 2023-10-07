using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float airSpeed = 8f;

    public float accelerationSpeed = 1f;
    public float decelerationSpeed = 0.5f;

    [Range(0, 1), Tooltip("(Only for Controller) Will only start moving when the stick is above a certain value from the center")] public float stickDeadzone;

    public float gravity = -9.81f;
    public float jump = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private Vector3 movement;
    private bool isGrounded;
    private bool isSliding;

    private float accelerationValue = 0;

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
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}