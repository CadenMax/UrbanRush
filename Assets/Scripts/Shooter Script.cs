using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public enum FSMState
    {
        Patrol,
        Attack,
    }
    public FSMState curState;

    public float min = 2f;
    public float max = 3f;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public float bulletPushForce = 10f;

    protected Transform playerTransform;// Player Transform

    // Turret
    public GameObject turret;
    public float turretRotSpeed = 4.0f;

    // Bullet
    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    // Bullet shooting rate
    public float shootRate = 1.0f;
    protected float elapsedTime;

    public float attackRange = 10.0f;

    void Start()
    {
        curState = FSMState.Patrol;
        elapsedTime = 0.0f;

        min = transform.position.z;
        max = transform.position.z + 20;
        
        // Ensure there's a box collider component
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
    }

    void Update()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
        }

        // Update the time
        elapsedTime += Time.deltaTime;
    }

    protected void UpdatePatrolState()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * 2, max - min) + min);
        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist < attackRange)
        {
            curState = FSMState.Attack;
        }
    }

    protected void UpdateAttackState()
    {
        // Transitions
        // Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= attackRange)
        {
            curState = FSMState.Patrol;
        }

        // Always Turn the turret towards the player
        if (turret)
        {
            Quaternion turretRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, turretRotation, Time.deltaTime * turretRotSpeed);
        }

        // Shoot the bullets
        ShootBullet();
    }

    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            if ((bulletSpawnPoint) & (bullet))
            {
                // Shoot the bullet
                Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
            elapsedTime = 0.0f;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
