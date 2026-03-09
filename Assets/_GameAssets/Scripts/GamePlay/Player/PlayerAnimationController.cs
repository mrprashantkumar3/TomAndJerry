using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
  [SerializeField] private Animator playerAnimator;

  private PlayerController playerController;
  private StateController stateController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        stateController = GetComponent<StateController>();
    }
    private void Start()
    {
        playerController.OnPlayerJump += PlayerController_OnPlayerJump;
    }

    

    private void Update()
    {
       if(GameManeger.Instance.GetCurrentGameState() != GameState.Play
         && GameManeger.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        SetPlayerAnimtion();
    }
    private void PlayerController_OnPlayerJump()
    {
       playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, true);
       Invoke(nameof(ResetJumping), 0.5f);
    }
    private void ResetJumping()
    {
        playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);
    }
    private void SetPlayerAnimtion()
    {
        var currentState = stateController.GetCurrentState();
        switch (currentState)
        {
            case PlayerState.Idle:
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_RUNNING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_FALLING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_LANDING, false);
              break;
            case PlayerState.Move:
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_RUNNING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_FALLING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_LANDING, false);
              break;
            case PlayerState.Running:
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_RUNNING, true);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_FALLING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_LANDING, false);
              break;
            case PlayerState.Falling:
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_FALLING, true);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_RUNNING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_LANDING, false);
              break;
            case PlayerState.Landing:
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_LANDING, true);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_FALLING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
              playerAnimator.SetBool(Consts.PlayerAnimations.IS_RUNNING, false);
              break;
            // case PlayerState.Running:
            //   playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
            //   playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
            //   break;
        }
        
        
    }
}
