using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PusherEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        None,
        Chase,
        Push,
        Race
    }

    public EnemyState currentState;

    public float pushForce = 10.0f; // Force to push the player
    public float pushRadius = 2.0f; // Radius to detect the player for pushing
    public float chaseRange = 10.0f; // Range to start chasing the player
    public GameObject finishLine;
    private Transform playerTransform;
    private Transform targetTransform;
    private Rigidbody playerRigidbody; // Player's rigidbody reference

    private NavMeshAgent nav;

     void Start()
    {
        currentState = EnemyState.Chase;
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetTransform = finishLine.transform;
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();

        nav = GetComponent<NavMeshAgent>();
    }

     void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                UpdateChaseState();
                Debug.Log("chasing");
                break;
            case EnemyState.Push:
                UpdatePushState();
                Debug.Log("pushing");
                break;
            case EnemyState.Race:
                UpdateRaceState();
                Debug.Log("race");
                break;
        }
    }

    protected void UpdateChaseState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // If the player is within chase range, start chasing
        if (distanceToPlayer <= chaseRange)
        {

            // Check if the player is in the push radius, transition to Push state if true
            if (distanceToPlayer <= pushRadius) {
                currentState = EnemyState.Push;
            } else {
                nav.SetDestination(playerTransform.position);
            }
        }
        else
        {
            currentState = EnemyState.Race;
        }
    }

    protected void UpdateRaceState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        {
            if (distanceToPlayer > chaseRange)
            {
                nav.SetDestination(targetTransform.position);
            }
            else
            {
                currentState = EnemyState.Chase;
            }
        }
    }

    protected void UpdatePushState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // If the player is out of the push radius, transition back to Chase state
        if (distanceToPlayer > pushRadius)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            // Calculate the direction to push the player
            Vector3 pushDirection = (playerTransform.position - transform.position).normalized;

            // Apply force to push the player
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }

 
}
