using UnityEngine;

public class IdleBlendAnimControll : StateMachineBehaviour
{
    [SerializeField] private float timeUntilIdle;
    [SerializeField] private int numberOfIdleAnim;

    private bool isIdle;
    private float idletime;
    private int idleAnimation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(isIdle == false)
        {
            idletime += Time.deltaTime;
            if(idletime > timeUntilIdle && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isIdle = true;
                idleAnimation = Random.Range(0, numberOfIdleAnim + 1);
                idleAnimation = idleAnimation * 2 - 1;
                animator.SetFloat("BlendIdle", idleAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetIdle();
        }

        animator.SetFloat("BlendIdle", idleAnimation, 0.2f, Time.deltaTime);
        
    }
    private void ResetIdle()
    {
        if (isIdle)
        {
            idleAnimation--;
        }
        isIdle = true;
        idletime = 0;
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
