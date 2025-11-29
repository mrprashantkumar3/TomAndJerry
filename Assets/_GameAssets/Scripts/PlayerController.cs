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
     [SerializeField] private bool canJump;

     [Header("Slidingt")]
     [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideMultiple;
    [SerializeField] private float slideDrag;

     [Header("Gound Check")]
     [SerializeField] private float playerHeight;
     [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;

    private Rigidbody playerRigidbody;
    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;
    private bool isSliding;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }
    private void Update()
    {
        SetInputs();
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
            Debug.Log("player Sliding");
        }else if(Input.GetKeyDown(movementKey))
        {
            isSliding = false;
             Debug.Log("player Moving");
        }
         else if(Input.GetKey(jumpKey) && canJump && IsGrounded())
        {
            canJump = false;
            SetPlayerJumpming();
            Invoke(nameof(ResetJumping), jumpCoolDown);
        }
    }
    private void SetPlayerMovement()
    {
        movementDirection = orientationTransform.forward * verticalInput + orientationTransform.right
         * horizontalInput;

        if(isSliding)
        {
           playerRigidbody.AddForce(movementDirection.normalized * movementSpeed * slideMultiple, ForceMode.Force);

        } else
        {
             playerRigidbody.AddForce(movementDirection.normalized * movementSpeed, ForceMode.Force);

        }

     
    }
   

    private void SetPlayerDamping()
    {
        if (isSliding)
        {
             playerRigidbody.linearDamping = slideDrag;
        }
        else
        {
            playerRigidbody.linearDamping = groundDrag;
        }
       
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

}
