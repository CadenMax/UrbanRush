using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Set these values in the Inspector to wherever you want to teleport the player.
    public float teleportToX = 0f;
    public float teleportToY = 0f;
    public float teleportToZ = 0f;

    // This function is called whenever another collider enters this object's collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has a tag of "Player". (Assumes your player object is tagged as "Player").
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the specified coordinates.
            other.transform.position = new Vector3(teleportToX, teleportToY, teleportToZ);
        }
    }
}
