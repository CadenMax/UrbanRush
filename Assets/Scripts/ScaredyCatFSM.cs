using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ScaredyCatEnemyFSM : MonoBehaviour
{
    public enum FSMState
    {
        None,
        Race,
        Avoid,
        Dead
    }

    // Current state that the NPC is reaching
    public FSMState curState;

    protected Transform playerTransform; // Player Transform
    protected Transform targetTransform; // Target Transform

    public GameObject[] jumpPoints; // List of waypoints for jumping
    public GameObject finishLine;

    public float ForwardSpeed;   // Speed when walking forward
    public float TailSpeed;   // Speed when walking forward

    // Ranges for chase and attack
    public float avoidRange;

    private NavMeshAgent nav;

    // current waypoint in list
    private bool setDest = false;

    public Vector3[] results;

    private GameObject objPlayer;

    private NavMeshPath path;

    void Start()
    {

        curState = FSMState.Race;

        avoidRange = 5.0f;
        ForwardSpeed = 5.0f;
        TailSpeed = 3.0f;

        // Get the target enemy(Player)
        objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        targetTransform = finishLine.transform;

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

        //reference the navmeshagent so we can access it
        nav = GetComponent<NavMeshAgent>();

        nav.speed = ForwardSpeed;
    }


    // Update each frame
    void Update()
    {
        // Update the way to the goal every second.
        if (transform.position == targetTransform.position)
        {
            nav.isStopped = true;
        }
        switch (curState)
        {
            case FSMState.Race: UpdateRaceState(); break;
            case FSMState.Avoid: UpdateAvoidState(); break;
        }
    }

    /*
     * Patrol state
     */
    protected void UpdateRaceState()
    {
        if (!setDest)
        {
            // NavMeshAgent move
            nav.SetDestination(targetTransform.position);
            setDest = true;
            nav.speed = ForwardSpeed;
        }

        // Check the distance with player tank
        // When the distance is near, transition to chase state
        if (objPlayer != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= avoidRange)
            {
                curState = FSMState.Avoid;
            }
        }
    }


    /*
     * Obstruct state
	 */
    protected void UpdateAvoidState()
    {

        if (objPlayer != null)
        {
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist > avoidRange)
            {
                curState = FSMState.Race;
                setDest = false;
            } else {
                nav.speed = TailSpeed;
            }
        }
        else
        {
            curState = FSMState.Race;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidRange);
    }
}
