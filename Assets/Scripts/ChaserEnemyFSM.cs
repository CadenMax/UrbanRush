using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ChaserEnemyFSM : MonoBehaviour 
{
    public enum FSMState
    {
        None,
        Race,
		Obstruct,
		Dead
    }

	// Current state that the NPC is reaching
	public FSMState curState;

	protected Transform playerTransform; // Player Transform
    protected Transform targetTransform; // Target Transform

    public GameObject[] jumpPoints; // List of waypoints for jumping
	public GameObject finishLine;

    public float ForwardSpeed = 8.0f;   // Speed when walking forward

    // Ranges for chase and attack
    public float blockRange;
	
	private NavMeshAgent nav;

	// current waypoint in list
	private bool setDest = false;

    public Vector3[] results;

	private GameObject objPlayer;

    private NavMeshPath path;
    private float elapsed = 0.0f;

    void Start() {

        curState = FSMState.Race;

        blockRange = 5.0f;

        // Get the target enemy(Player)
        objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        targetTransform = finishLine.transform;

        if(!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

		//reference the navmeshagent so we can access it
		nav = GetComponent<NavMeshAgent>();

        nav.speed = ForwardSpeed;

        path = new NavMeshPath();
        elapsed = 0.0f;
    }


    // Update each frame
    void Update() {
        // Update the way to the goal every second.
        if (transform.position == targetTransform.position) {
            nav.isStopped = true;
        }
        switch (curState) {
            case FSMState.Race: UpdateRaceState(); break;
            case FSMState.Obstruct: UpdateObstructState(); break;
        }
    }

	/*
     * Patrol state
     */
    protected void UpdateRaceState() {
		if (!setDest) {
			// NavMeshAgent move
			nav.SetDestination(targetTransform.position);
			setDest = true;
		}

        // Check the distance with player tank
        // When the distance is near, transition to chase state
		if(objPlayer != null)
        {
			if (Vector3.Distance(transform.position, playerTransform.position) <= 5.0f)
			{
				curState = FSMState.Obstruct;
			}
        }        
    }


    /*
     * Obstruct state
	 */
    protected void UpdateObstructState() {

		if(objPlayer != null)
        {
            // NavMeshAgent move
            elapsed += Time.deltaTime;
            if (elapsed > 0.1f) {
                elapsed = 0f;
                NavMesh.CalculatePath(playerTransform.position, targetTransform.position, NavMesh.AllAreas, path);
                if (path.corners[2] != targetTransform.position) {
                    nav.SetDestination(path.corners[2]);
                } else {
                    curState = FSMState.Race;
                }
            }
            /*
            for (int i = 0; i < path.corners.Length - 1; i++) {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
            */

			// When the distance is near, transition to attack state
			float dist = Vector3.Distance(transform.position, playerTransform.position);
            if (dist > blockRange) {
                setDest = false;
                curState = FSMState.Race;
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
        Gizmos.DrawWireSphere(transform.position, blockRange);
    }
}
