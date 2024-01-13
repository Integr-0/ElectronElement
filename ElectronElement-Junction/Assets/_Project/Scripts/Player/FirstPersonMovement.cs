using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FirstPersonMovement : NetworkBehaviour
{
    #region Fields

    [SerializeField] private Animator anim;

    [Space, SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;

    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float sneakSpeedMultiplier = 0.5f;
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

    #endregion

    #region Variables

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
    private bool isSneaking;

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();

        currentGroundSpeed = speed;
    }

    void Update()
    {
        if (!IsOwner) return;

        #region set basic variables

        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayers);
        tryingToClimb = Physics.CheckSphere(ladderCheckTransform.position, ladderCheckDistance, climbableLayers);
        isSneaking = Input.GetKey(KeyCode.LeftControl);
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        currentGroundSpeed = speed;
        if (isSneaking) //Sneaking has the most priority
        {
            currentGroundSpeed *= sneakSpeedMultiplier;
        }
        else if (isSprinting) //Sprinting has less priority than sneaking
        {
            currentGroundSpeed *= sprintSpeedMultiplier;
        }
        //if not grounded (in the air)
        else if (!isGrounded) //Being grounded doesn't change the currentGroundSpeed value
        {
            currentGroundSpeed *= airControlMultiplier;
        }
        
        currentClimbSpeed = isSprinting ? climbSpeed * sprintSpeedMultiplier : climbSpeed;

        #endregion

        #region resetting gravity force when grounded

        if ((isGrounded || tryingToClimb) && yMovement < 0)
        {
            yMovement = -2f;
        }

        #endregion

        #region get Input

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        #endregion

        #region calculate values

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        #endregion

        #region climbing

        if (tryingToClimb && input.magnitude > 0) yMovement = currentClimbSpeed;

        #endregion

        #region jumping & gravity

        //jumping
        jumpBufferTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && !tryingToClimb)
        {
            jumpBufferTimer = jumpBuffer;
        }
        if (jumpBufferTimer > 0f && isGrounded)
        {
            //real life gravity formula
            yMovement = Mathf.Sqrt(jumpStrength * -2f * gravity);  
        }

        //adding gravity
        yMovement += gravity * Time.deltaTime;

        #endregion

        #region applying movement

        controller.Move(currentGroundSpeed * Time.deltaTime * move);
        controller.Move(Time.deltaTime * yMovement * transform.up);

        #endregion

        #region play footsteps

        if (isGrounded && input.magnitude > 0 && !isSneaking)
        {
            nextFootstep -= Time.deltaTime * (isSprinting ? sprintSpeedMultiplier : 1f);
            if (nextFootstep <= 0)
            {
                footstepAudioSource.PlayOneShot(footstepSound, 0.7f);
                nextFootstep += footstepDelay;
            }
        }

        #endregion

        #region set animator values

        if (anim != null) anim.SetFloat("Speed", move.magnitude > 0.1f ? currentGroundSpeed : 0);

        #endregion
    }

    #region Push physics
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRb = hit.collider.attachedRigidbody;

        if (hitRb == null || hitRb.isKinematic || hit.moveDirection.y < -0.6f) return;

        Vector3 pushDir = new(hit.moveDirection.x, 0, hit.moveDirection.z);

        hitRb.velocity = pushDir * pushForce * currentGroundSpeed;
    }
    #endregion

    #region Draw visualizations for groundCheck and ladderCheck

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(ladderCheckTransform.position, ladderCheckDistance);
    }

    #endregion
}