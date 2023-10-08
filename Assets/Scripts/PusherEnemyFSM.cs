using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PusherEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        None,
        Ram,
        Race
    }

    public EnemyState currentState;

    public float ramRange; // Range to start chasing the player

    public GameObject finishLine;
    private Transform playerTransform;
    private Transform targetTransform;
    private Rigidbody playerRigidbody; // Player's rigidbody reference

    private bool setDest = false;

    private NavMeshAgent nav;

    void Start()
    {
        currentState = EnemyState.Race;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetTransform = finishLine.transform;
        playerRigidbody = playerTransform.GetComponent<Rigidbody>();

        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = 0.1f;
        ramRange = 1.5f;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > ramRange && currentState != EnemyState.Race)
        {
            currentState = EnemyState.Race;
        }

        switch (currentState)
        {
            case EnemyState.Ram:
                UpdateRamState();
                break;
            case EnemyState.Race:
                setDest = false;
                UpdateRaceState();
                break;
        }
    }

    protected void UpdateRamState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // If the player is within chase range, start chasing
        if (distanceToPlayer <= ramRange) {
            nav.SetDestination(playerTransform.position);
        } else {
            currentState = EnemyState.Race;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && currentState == EnemyState.Ram)
        {
            nav.SetDestination(GameObject.Find("DetectVaultObject").transform.position);
        }
    }

    protected void UpdateRaceState()
    {
        if (!setDest) {
            nav.SetDestination(targetTransform.position);
            setDest = true;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= ramRange)
        {
            currentState = EnemyState.Ram;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ramRange);
    }
}
