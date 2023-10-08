using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PunterEnemy : MonoBehaviour
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
        nav.stoppingDistance = 0.1f;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > chaseRange && currentState != EnemyState.Race)
        {
            currentState = EnemyState.Race;
        }

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
            nav.SetDestination(playerTransform.position);

            // Check if the player is in the push radius, transition to Push state if true
            if (distanceToPlayer <= pushRadius)
            {
                currentState = EnemyState.Push;
            }
        }
    }

    protected void UpdateRaceState()
    {
        float distanceToFinish = Vector3.Distance(transform.position, targetTransform.position);

        if (distanceToFinish <= nav.stoppingDistance)
        {
            // AI reached the finish line
            // currentState = EnemyState.Finish; // assuming you add a Finish state
        }
        else
        {
            nav.SetDestination(targetTransform.position);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chase;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        //draw gizmo for current direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

    }

}
