using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float dashForce;
    public float dashTime;
    public float dashCooldown;
    public bool readyToDash;
    private bool dashing;
    public bool isMoving;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public float crouchForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction crouch;
    private InputAction sprint;
    private InputAction dash;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround, climbZone;
    public bool grounded;
    public Transform ground;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Enemy Pathfinding")]
    public LayerMask enemyLandArea;
    public float antiLandRange;
    public bool landRangeDeactivate;
    public Transform goal;
    public GameObject NearestGroundToPlayer;
    public bool landingZone;
    public float landingRange;

    public Animator GribbyRun;
    public bool isJumping;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        stationary,
        air
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        jump = playerControls.Player.Jump;
        jump.Enable();
        crouch = playerControls.Player.Crouch;
        crouch.Enable();
        sprint = playerControls.Player.Sprint;
        sprint.Enable();
        dash = playerControls.Player.Dash;
        dash.Enable();
        dash.performed += Dash;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
        sprint.Disable();
        dash.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        dashing = false;
        readyToDash = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        CheckGrounded();

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded && !isMoving)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        NearestGroundToPlayer = gameObject.GetComponent<SenseGround>().NearestGround;
        ground = GameObject.Find("Goal").transform;
        landingZone = ground.GetComponent<Goal>();

        //Get speed
        //Debug.Log(rb.velocity.magnitude);
    }

    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround) || OnSlope())
        {
            grounded = true;
        }

        if (!Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround) && !OnSlope())
        {
            grounded = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * crouchForce, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void JumpingAnimation()
    {
        if (isJumping)
        {
            GribbyRun.SetBool("isOnGround", false);
        }
        else if (!isJumping)
        {
            GribbyRun.SetBool("isOnGround", true);
        }
    }
    
    private void MovePlayer()
    {
        // Calculate movement direction
        // moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection = orientation.forward * move.ReadValue<Vector3>().z + orientation.right * move.ReadValue<Vector3>().x;

        Debug.Log(moveDirection);
        
        if (moveDirection.x == 0 && moveDirection.z == 0)
            isMoving = false;
        
        if (moveDirection.x != 0 || moveDirection.z != 0)
            isMoving = true;


        

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.up * Physics.gravity.y, ForceMode.Force);
        }

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f, ForceMode.Force);
        }

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if (!isMoving && OnSlope() && grounded)
        {
            rb.useGravity = false;

            rb.velocity = new Vector3(EaseToZero(rb.velocity.x, 0.1f), rb.velocity.y, EaseToZero(rb.velocity.z, 0.1f));
            
        }
        else
            rb.useGravity = true;
    }

    public float EaseToZero(float value, float amount)
    {
        while (value != 0)
        {
            if (value < 0)
                value += amount;
            if (value > 0)
                value -= amount;

            return value;
        }

        return value;
    }

    // dash
    private void Dash(InputAction.CallbackContext context)
    {
        if (readyToDash)
        {
            readyToDash = false;
            dashing = true;
            PlayerStats.godMode = true;

            Vector3 direction = moveDirection;
            bool moving = isMoving;

            StartCoroutine(executeDash(direction, moving));

            Invoke(nameof(DashCooldown), dashCooldown);
        }
    }

    IEnumerator executeDash(Vector3 direction, bool moving)
    {
        Invoke(nameof(Dashing), dashTime);

        while (dashing)
        {
            if (moving)
            {
                rb.AddForce(direction * dashForce, ForceMode.Force);
            }
            else
            {
                rb.AddForce(orientation.forward * dashForce, ForceMode.Force);
            }

            yield return null;
        }
    }

    void Dashing()
    {
        dashing = false;
        PlayerStats.godMode = false;
    }

    void DashCooldown()
    {
        readyToDash = true;
    }

    private void StateHandler()
    {
        
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            isJumping = false;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey) && isMoving)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            // Debug.Log(rb.velocity.magnitude);
            GribbyRun.SetFloat("Speed",  rb.velocity.magnitude);
            isJumping = false;
            JumpingAnimation();
        }

        // Mode - Walking
        else if (grounded && isMoving)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            GribbyRun.SetFloat("Speed", 0);
            isJumping = false;
            JumpingAnimation();
        }

        //Mode - Stationary
        else if (grounded)
        {
            state = MovementState.stationary;
            GribbyRun.SetFloat("Speed", 0);
            isJumping = false;
            JumpingAnimation();
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
            isJumping = true;
            JumpingAnimation();
        }
    }

    
    private void SpeedControl()
    {
        // limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limit speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, landingRange);
        Gizmos.color = Color.white;
    }
}
