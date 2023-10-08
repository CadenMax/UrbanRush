using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float pushForce = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }

            Destroy(gameObject);  // Destroy the bullet upon hitting the player
        }
    }
}
