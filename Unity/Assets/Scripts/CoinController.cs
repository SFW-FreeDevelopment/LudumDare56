using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int score;
    // Detect when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object triggering the event is the player (assuming the player has the "Player" tag)
        if (other.CompareTag("Player"))
        {
            score++;
            Destroy(gameObject); // Destroy the algae object
        }
    }
}
