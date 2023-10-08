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

	protected Transform playerTransform;// Player Transform
	
	public GameObject[] jumpPoints; // List of waypoints for jumping
	public GameObject finishLine;

    // Whether the NPC is destroyed or not
    protected bool bDead;

    public float ForwardSpeed = 8.0f;   // Speed when walking forward
    public float BackwardSpeed = 4.0f;  // Speed when walking backwards
    public float StrafeSpeed = 4.0f;    // Speed when walking sideways
    public float SpeedInAir = 8.0f;   // Speed when onair
    public float JumpForce = 30f;

	// Ranges for chase and attack
    public float chaseRange = 35.0f;
	
	private NavMeshAgent nav;

	// current waypoint in list
	private int curWaypoint = -1;
	private bool setDest = false;

	public float pathCheckTime = 1.0f;
	private float elapsedPathCheckTime;

	private GameObject objPlayer;

	void Start() {

        curState = FSMState.Race;

        bDead = false;

        // Get the target enemy(Player)
        objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if(!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

		//reference the navmeshagent so we can access it
		nav = GetComponent<NavMeshAgent>();

		// if there are waypoints in the list set our destination to be the current waypoint
		if (jumpPoints.Length > 0)
			curWaypoint = 0;

		// set to pathCheckTime so it will trigger first time
		elapsedPathCheckTime = pathCheckTime;
	}


    // Update each frame
    void Update() {
        switch (curState) {
            case FSMState.Race: UpdateRaceState(); break;
            case FSMState.Obstruct: UpdateObstructState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }
    }

	/*
     * Patrol state
     */
    protected void UpdateRaceState() {
        
		// only move if there are waypoints in list for object
		if (curWaypoint > -1) {
			// check if close to current waypoint
			if (Vector3.Distance(transform.position, jumpPoints[curWaypoint].gameObject.transform.position) <= 2.0f) {
				// get next waypoint
				curWaypoint++;
				// if we have travelled to last waypoint, go back to the first
				if (curWaypoint > (jumpPoints.Length - 1))
					curWaypoint = 0;

				setDest = false;
			}

			if (!setDest) {
				// NavMeshAgent move
				nav.SetDestination(jumpPoints[curWaypoint].gameObject.transform.position);
				setDest = true;
			}
		}

        // Check the distance with player tank
        // When the distance is near, transition to chase state
		if(objPlayer != null)
        {
			if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRange)
			{

				// see if playerTank is Line of Sight
				RaycastHit hit;
				if (Physics.Linecast(transform.position + new Vector3(0f, 1f, 0f), playerTransform.position + new Vector3(0f, 1f, 0f), out hit))
				{
					if (hit.collider.gameObject.tag == "Player")
					{
						curState = FSMState.Obstruct;
					}
				}
			}
        }
        
    }


    /*
     * Chase state
	 */
    protected void UpdateObstructState() {

		if(objPlayer != null)
        {
			// NavMeshAgent move
			if (elapsedPathCheckTime >= pathCheckTime)
			{
				nav.SetDestination(playerTransform.position);
				elapsedPathCheckTime = 0f;
			}

			// Check the distance with player tank
			// When the distance is near, transition to attack state
			float dist = Vector3.Distance(transform.position, playerTransform.position);

			if (dist >= chaseRange) {
				curState = FSMState.Race;
				setDest = false;
			}
		}
        else
        {
			curState = FSMState.Race;
        }
		
	}

    /*
     * Dead state
     */
    protected void UpdateDeadState() {
        // Show the dead animation with some physics effects
        if (!bDead) {
			nav.isStopped = true;
			nav.enabled = false;
            bDead = true;
        }
    }
	/*
	void OnDrawGizmos () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, chaseRange);
	}
	*/

}
