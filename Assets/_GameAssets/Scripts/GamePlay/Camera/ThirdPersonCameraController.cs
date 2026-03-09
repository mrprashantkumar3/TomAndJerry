using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    // Start is called once before the first execution orf Update after the MonoBehaviour is created
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orientationTransform;
    [SerializeField] private Transform playerVisualTransform;
    [SerializeField] private float rotationSpeed;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if(GameManeger.Instance.GetCurrentGameState() != GameState.Play
         && GameManeger.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        Vector3 viewDirection = 
        playerTransform.position - new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);

        orientationTransform.forward = viewDirection.normalized;

         float horizontalInput = Input.GetAxisRaw("Horizontal");
         float verticalInput = Input.GetAxisRaw("Vertical");

         Vector3 inputDirection = 
         orientationTransform.forward * verticalInput + orientationTransform.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
          playerVisualTransform.forward 
         = Vector3.SmoothDamp(playerVisualTransform.forward, inputDirection.normalized, ref velocity, Time.deltaTime * rotationSpeed);
         //Vector3.Slerp(playerVisualTransform.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
        
    }
}
