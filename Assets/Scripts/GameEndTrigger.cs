using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndTrigger : MonoBehaviour
{
    public string Leaderboard;  // Name of the scene to transition to

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // assuming your player has the tag "Player"
        {
            SceneManager.LoadScene(Leaderboard);
        }
    }
}
