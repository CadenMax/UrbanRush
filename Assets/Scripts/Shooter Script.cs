using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public float min = 2f;
    public float max = 3f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public float bulletPushForce = 10f;
    public GameObject triggerZone;

    private bool canShoot = false;
    private float nextFireTime;

    void Start()
    {
        min = transform.position.z;
        max = transform.position.z + 20;
        
        // Ensure there's a box collider component
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }

        if (triggerZone != null)
        {
            triggerZone.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    void Update()
    {
        // Moving back and forth
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * 2, max - min) + min);
        
        // Shooting at the player
        if (canShoot && Time.time >= nextFireTime)
        {
            Debug.Log("Firing");
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        Debug.Log("Bullet Spawned");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.pushForce = bulletPushForce;
        }
    }

    void OnTriggerEnter(Collider other)
    {
            Debug.Log("Player Entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            canShoot = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            canShoot = false;
        }
    }
}
