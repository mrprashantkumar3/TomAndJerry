using UnityEngine;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private CatState currentCatState = CatState.Walking;
    private void Start()
    {
        ChangeState(CatState.Walking);
    }
    public void ChangeState(CatState newState)
    {
        if(currentCatState == newState){ return; }
        currentCatState = newState;
    }
    public CatState GetCurrentState()
    {
        return currentCatState;
    }
    
}
