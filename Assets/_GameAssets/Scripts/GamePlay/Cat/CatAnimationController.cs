using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
   [SerializeField] private Animator catAnimatior;
   private CatStateController catStateController;
    private void Awake()
    {
        catStateController = GetComponent<CatStateController>();
    }
    private void Update()
    {
        SetCatAnimation();
    }
    private void SetCatAnimation()
    {
        var currentCatState = catStateController.GetCurrentState();
        switch (currentCatState)
        {
            case CatState.Idle:
                catAnimatior.SetBool(Consts.CatAnimations.IS_IDLING, true);
                catAnimatior.SetBool(Consts.CatAnimations.IS_WALkING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_RUNNING, false);
                break;
            case CatState.Walking:
                catAnimatior.SetBool(Consts.CatAnimations.IS_IDLING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_WALkING, true);
                catAnimatior.SetBool(Consts.CatAnimations.IS_RUNNING, false);
                break;
            case CatState.Running:
                catAnimatior.SetBool(Consts.CatAnimations.IS_IDLING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_WALkING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_RUNNING, true);
                break;
            case CatState.Attacking:
                catAnimatior.SetBool(Consts.CatAnimations.IS_IDLING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_WALkING, false);
                catAnimatior.SetBool(Consts.CatAnimations.IS_ATTACKING, true);
                break;
        }
    }
}
