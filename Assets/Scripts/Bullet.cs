using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float pushForce = 10.0f;
    public float speed = 3.0f;
    public float lifeTime = 3.0f;

    private Vector3 newPos;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // future position if bullet doesn't hit any colliders
        newPos = transform.position + transform.up * speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Linecast(transform.position, newPos, out hit))
        {
            if (hit.collider)
            {
                transform.position = hit.point;
                //Destroy(gameObject);

                GameObject obj = hit.collider.gameObject;
                if (obj.tag == "Player")
                {
                    Rigidbody playerRigidbody = obj.GetComponent<Rigidbody>();
                    if (playerRigidbody != null)
                    {
                        Vector3 pushDirection = (obj.transform.position - transform.position).normalized;
                        playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                    }
                }
            }
        }
        else
        {
            // didn't hit - move to newPos
            transform.position = newPos;
        }
    }
}
