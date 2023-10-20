using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Space, SerializeField] private float speed = 3f;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float airSpeed = 2f;
    [SerializeField] private float climbSpeed;

    [Space, SerializeField] private float acceleration = 0.4f;

    [Space, SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jump = 1f;

    [Space, SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Space, SerializeField] private Transform ladderCheck;
    [SerializeField] private float ladderDistance = 0.1f;
    [SerializeField] private LayerMask ladderMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool tryingToClimb;
    private float currentGravity;

    private float currentSpeed
    {
        get => _currentSpeed;
        set
        {
            /*while (_currentSpeed != value)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, value, acceleration * Time.deltaTime);
            }*/
            _currentSpeed = value;
        }
    }
    private float _currentSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentGravity = gravity;
        _currentSpeed = speed;
    }

    void Update()
    {     
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        tryingToClimb = Physics.CheckSphere(ladderCheck.position, ladderDistance, ladderMask);

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed*sprintSpeedMultiplier : isGrounded ? speed : airSpeed;

        if ((isGrounded || tryingToClimb) && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(currentSpeed * Time.deltaTime * move);

        if (tryingToClimb && (x != 0 || z != 0)) 
                velocity = 
                (Input.GetKey(KeyCode.LeftShift) ? sprintSpeedMultiplier : 1f)
                * climbSpeed * transform.up;

        if (Input.GetButtonDown("Jump") && isGrounded && !tryingToClimb)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * currentGravity);
        }

        velocity.y += currentGravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);    
    }
}