using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;

    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float airControlMultiplier = 1f;


    [Space, SerializeField] private float jumpStrength = 1.5f;
    [SerializeField] private float jumpBuffer = 0.2f;

    [Space, SerializeField] private float gravity = -20f;


    [Space, SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayers;


    [Space, SerializeField] private Transform ladderCheckTransform;
    [SerializeField] private float ladderCheckDistance = 0.1f;
    [SerializeField] private LayerMask climbableLayers;

    [Space, SerializeField] private float pushForce = 0.7f;


    [Space, SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.3f;


    private CharacterController controller;
    private AudioSource footstepAudioSource;


    private Vector2 input;


    private float yMovement;
    private float currentGroundSpeed;
    private float currentClimbSpeed;

    private float jumpBufferTimer;

    private float nextFootstep = 0;


    private bool isGrounded;

    private bool tryingToClimb;
    private bool isSprinting;

    private void Awake()
    {
        //Init
        controller = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();

        currentGroundSpeed = speed;
    }

    void Update()
    {     
        //Set basic variables
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayers);
        tryingToClimb = Physics.CheckSphere(ladderCheckTransform.position, ladderCheckDistance, climbableLayers);
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        currentGroundSpeed = isSprinting ? speed * sprintSpeedMultiplier : isGrounded ? speed : speed * airControlMultiplier;
        currentClimbSpeed = isSprinting ? climbSpeed * sprintSpeedMultiplier : climbSpeed;


        //resetting gravity force when grounded
        if ((isGrounded || tryingToClimb) && yMovement < 0)
        {
            yMovement = -2f;
        }


        //get input
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 move = transform.right * input.x + transform.forward * input.y;


        //climbing
        if (tryingToClimb && input.magnitude > 0) yMovement = currentClimbSpeed;


        //jumping (with jumpBuffer)
        jumpBufferTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && !tryingToClimb)
        {
            jumpBufferTimer = jumpBuffer;
        }
        if(jumpBufferTimer > 0f && isGrounded)
        {
            //real life gravity formula
            yMovement = Mathf.Sqrt(jumpStrength * -2f * gravity);
            
        }

        
        //adding gravity
        yMovement += gravity * Time.deltaTime;


        //moving
        controller.Move(currentGroundSpeed * Time.deltaTime * move);
        controller.Move(yMovement * transform.up * Time.deltaTime);


        //footsteps
        if (isGrounded && input.magnitude > 0)
        {
            nextFootstep -= Time.deltaTime * (isSprinting ? sprintSpeedMultiplier : 1f);
            if (nextFootstep <= 0)
            {
                footstepAudioSource.PlayOneShot(footstepSound, 0.7f);
                nextFootstep += footstepDelay;
            }
        }
    }

    //Push physics
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRb = hit.collider.attachedRigidbody;

        if (hitRb == null || hitRb.isKinematic || hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new(hit.moveDirection.x, 0, hit.moveDirection.z);

        hitRb.velocity = pushDir * pushForce * currentGroundSpeed;
    }

    //Draw visualizatiuons for groundCheck und ladderCheck
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ladderCheckTransform.position, ladderCheckDistance);
    }
}