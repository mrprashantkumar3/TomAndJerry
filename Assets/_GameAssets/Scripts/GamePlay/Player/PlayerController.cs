using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJump;
    
    public event Action<PlayerState> OnPlayerStateChange;

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

    private float startingMovementSpeed, startingJumpForce;
    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;
    private bool isRunning;
    private bool wasFalling;
    private bool isLanding;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;

        startingMovementSpeed = movementSpeed;
        startingJumpForce = jumpForce;
    }
    private void Update()
    {
        if(GameManeger.Instance.GetCurrentGameState() != GameState.Play
        && GameManeger.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        SetInputs();
        SetStates();
        SetPlayerDamping();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        if(GameManeger.Instance.GetCurrentGameState() != GameState.Play
        && GameManeger.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey))
        {
            isRunning = true;
          //  Debug.Log("Player Sliding");
        }else if(Input.GetKeyDown(movementKey))
        {
            isRunning = false;
           // Debug.Log("Player is Moving");
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
        var isRunning = IsRunning();
        var wasFalling = IsFalling();
        var isLanding = IsLanding();
        var currentState = stateController.GetCurrentState();
        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !isRunning => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !isRunning => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded &&  isRunning => PlayerState.Running,
            _ when movementDirection == Vector3.zero && isGrounded &&  isRunning => PlayerState.Idle,
            
            _ when !canJump && wasFalling && !isGrounded => PlayerState.Jump, 
            //_ when movementDirection != Vector3.zero && !yVel && isGrounded && !isRunning => PlayerState.Move,
            _ when !wasFalling && !isGrounded => PlayerState.Falling,
            _ when !isLanding && !wasFalling && isGrounded => PlayerState.Landing,
            _ when movementDirection == Vector3.zero && !wasFalling && isGrounded && !isRunning => PlayerState.Idle,
            
            _ => currentState
            
           
        };
        if (newState != currentState)
        {
            stateController.ChangeState(newState);
            OnPlayerStateChange?.Invoke(newState);
        }
        
    }
    private void SetPlayerMovement()
    {
        movementDirection = orientationTransform.forward * verticalInput
                      + orientationTransform.right * horizontalInput;

        float forceMultiplier;

        if (IsGrounded())
        {
             forceMultiplier = stateController.GetCurrentState() switch
        {
             PlayerState.Move => 1f,
             PlayerState.Running => slideMultiple,
             PlayerState.Jump => airMultiplier,
             PlayerState.Landing => 1f,
             _ => 1f
        };
       
        }
         else 
        {
             forceMultiplier = airMultiplier;
             forceMultiplier = stateController.GetCurrentState() switch
             {
                 PlayerState.Falling => airMultiplier,
                 _ => 0f
             };
            
             
        }

    playerRigidbody.AddForce(
        movementDirection.normalized * movementSpeed * forceMultiplier,
        ForceMode.Force
    );
        // if (!IsGrounded())
       
        // movementDirection = orientationTransform.forward * verticalInput + orientationTransform.right
        //  * horizontalInput;
        
        // float forceMultiplier = stateController.GetCurrentState() switch
        // {
        //     PlayerState.Move => 1f,
        //     PlayerState.Running => slideMultiple,
        //     PlayerState.Jump => airMultiplier,
        //     _ => 1f
        // };

        // playerRigidbody.AddForce(movementDirection.normalized * movementSpeed * forceMultiplier, ForceMode.Force);
        // return;
    }
   

    private void SetPlayerDamping()
    {
        playerRigidbody.linearDamping = stateController.GetCurrentState() switch
        {
          PlayerState.Move => groundDrag,
          PlayerState.Running => slideDrag,
          PlayerState.Jump => airDrag,
          //PlayerState.Falling => airMultiplier,
          //PlayerState.Landing => groundDrag,
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
       
        OnPlayerJump?.Invoke();
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);

        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJumping()
    {
        canJump = true;
    }
    #region Helper Functions

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }
    private Vector3 GetMovementDirection()
    {
        return movementDirection.normalized;
    }
    private bool IsRunning()
    {
        return isRunning;
    }
    private void ResetLanding()
    {
        isLanding = false;
        Debug.Log("Landing Complete!");
    }
    private bool IsFalling()
    {
        return wasFalling;
    }
    private bool IsLanding()
    {
        return isLanding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }
    private void ResetMovementSpeed()
    {
        movementSpeed = startingMovementSpeed; 
    }
    public void SetJumpForce(float Force , float duration)
    {
       jumpForce += Force;
       Invoke(nameof(ResetJumpForce), duration);
    }
    private void ResetJumpForce()
    {
        jumpForce = startingJumpForce;
    }
    public Rigidbody GetPlayerRigidbody()
    {
        return playerRigidbody;
    }
    public bool CanCatChase()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.2f, groundLayer))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer(Consts.Layers.FLOOR_LAYER))
            {
                return true;
            }
            else if(hit.collider.gameObject.layer == LayerMask.NameToLayer(Consts.Layers.GROUND_LAYER))
            {
                return false;
            }
        
        }
        return false;
    }

    #endregion

}
