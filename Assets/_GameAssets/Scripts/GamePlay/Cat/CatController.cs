using System;
using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{
    public event Action OnCatCatched;
    private NavMeshAgent catAgent;
    private CatStateController catStateController;
    [Header("Reference")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float chaseSpeed = 8f;
    [Header("Navigation Setting")]
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float patrolRedus = 10f;
    [SerializeField] private int maxDestinationAttemts = 10;
    [SerializeField] private float chaseDistanceThreshold = 1.5f;
    [SerializeField] private float chaseDistance = 2f;
    private bool isWaiting;
    private bool isChasing;
    private float timer;
    
    private Vector3 initialPostion;
    private void Awake()
    {
        catAgent = GetComponent<NavMeshAgent>();
        catStateController = GetComponent<CatStateController>();
    }
    private void Start()
    {
        initialPostion = transform.position;
        SetRandomDestination();
    }
    private void Update()
    {
        if (playerController.CanCatChase())
        {
            SetChaseMovement();
        }
        else
        {
           SetPatrolMovement(); 
        }
        

    }
    private void SetChaseMovement()
    {
        isChasing = true;
        Vector3 direntionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 offSetPosition = playerTransform.position - direntionToPlayer * chaseDistanceThreshold;
        catAgent.SetDestination(offSetPosition);
        catAgent.speed = chaseSpeed;
        catStateController.ChangeState(CatState.Running);
        if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance && isChasing)
        {
            OnCatCatched?.Invoke();
            catStateController.ChangeState(CatState.Attacking);
            isChasing = false;
        }
    }
    private void SetPatrolMovement()
    {
        catAgent.speed = defaultSpeed;
        if (!catAgent.pathPending && catAgent.remainingDistance <= catAgent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                timer = waitTime;
                catStateController.ChangeState(CatState.Idle);

            }
        }
        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                isWaiting = false;
                SetRandomDestination();
                catStateController.ChangeState(CatState.Walking);
            }
        }
    }
    private void SetRandomDestination()
    {
        int attemt = 0;
        bool destinationSet = false;
        while(attemt < maxDestinationAttemts && !destinationSet)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRedus;
            randomDirection += initialPostion;
            if(NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRedus, NavMesh.AllAreas))
            {
               Vector3 finalPosition = hit.position;
                if (!IsPositionBlocked(finalPosition))
                {
                    catAgent.SetDestination(finalPosition);
                    destinationSet = true;
                }
                else
                {
                    attemt++;
                }
            }
            else
            {
                attemt++;
            }
        }

        if (!destinationSet)
        {
            Debug.LogWarning(" failed to find ");
            isWaiting = true;
            timer = waitTime *2;
        }

    }
    private bool IsPositionBlocked(Vector3 position)
    {
        if (NavMesh.Raycast(transform.position, position, out NavMeshHit hit, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = (initialPostion != Vector3.zero) ? initialPostion : transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, patrolRedus);
    }
}
