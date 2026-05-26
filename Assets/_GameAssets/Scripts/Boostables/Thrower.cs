using UnityEngine;

public class Thrower : MonoBehaviour, IBoostable
{

    [SerializeField] private Animator spatulaAnimator;

    [SerializeField] private float jumpForce;
    private bool isActivate;
    public void Boost(PlayerController playerController)
    {
       if(isActivate){return;}
       PlayBoostAnimation();
       Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();

       playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
       playerRigidbody.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
       isActivate = true;
       Invoke(nameof(ResetActivation), 0.2f);
    }
    private void PlayBoostAnimation()
    {
        spatulaAnimator.SetTrigger(Consts.OtherAnimtion.IS_SPATULA_JUMPING);
    }
    private void ResetActivation()
    {
        isActivate = false;
    }
}
