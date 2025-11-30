using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Referance")]
    [SerializeField] private Transform orientationTransform;

     [Header("Movement Settitng")]
     [SerializeField] private KeyCode movementKey;
    [SerializeField] private float movementSpeed = 20f;

     [Header("Jump Settitng")]
     [SerializeField] private KeyCode jumpKey;
     [SerializeField] private float jumpForce;
     [SerializeField] private float jumpCoolDown;
     [SerializeField] private float airMultiplier;
     [SerializeField] private float airDrag;
     [SerializeField] private bool canJump;

     [Header("Slidingt")]
     [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideMultiple;
    [SerializeField] private float slideDrag;

     [Header("Gound Check")]
     [SerializeField] private float playerHeight;
     [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;
    private StateController stateController;
    private Rigidbody playerRigidbody;
    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;
    private bool isSliding;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }
    private void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDamping();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey))
        {
            isSliding = true;
            
        }else if(Input.GetKeyDown(movementKey))
        {
            isSliding = false;
            
        }
         else if(Input.GetKey(jumpKey) && canJump && IsGrounded())
        {
            canJump = false;
            SetPlayerJumpming();
            Invoke(nameof(ResetJumping), jumpCoolDown);
        }
    }

    private void SetStates()
    {
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var isSliding = IsSliding();
        var currentState = stateController.GetCurrentState();
        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.SlideIdle,
            _ when !canJump && !isGrounded => PlayerState.Jump,           
            _ => currentState
        };
        if (newState != currentState)
        {
            stateController.ChangeState(newState);
        }
        
    }
    private void SetPlayerMovement()
    {
        movementDirection = orientationTransform.forward * verticalInput + orientationTransform.right
         * horizontalInput;
    
        float forceMultiplier = stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => slideMultiple,
            PlayerState.Jump => airMultiplier,
            _ => 1f
        };
         playerRigidbody.AddForce(movementDirection.normalized * movementSpeed * forceMultiplier, ForceMode.Force);


       
     
    }
   

    private void SetPlayerDamping()
    {
        playerRigidbody.linearDamping = stateController.GetCurrentState() switch
        {
          PlayerState.Move => groundDrag,
          PlayerState.Slide => slideDrag,
          PlayerState.Jump => airDrag,
          _ => playerRigidbody.linearDamping


        };
        
    }
     private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f , playerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * movementSpeed;
            playerRigidbody.linearVelocity = new Vector3(limitVelocity.x, playerRigidbody.linearVelocity.y, limitVelocity.z);

        }
    }
    private void SetPlayerJumpming()
    {
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);

        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJumping()
    {
        canJump = true;
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }
    private Vector3 GetMovementDirection()
    {
        return movementDirection.normalized;
    }
    private bool IsSliding()
    {
        return isSliding;
    }

}
