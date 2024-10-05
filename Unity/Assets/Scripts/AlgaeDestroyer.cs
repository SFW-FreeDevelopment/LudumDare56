using UnityEngine;


public class AlgaeDestroyer : MonoBehaviour
{
    // Detect when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object triggering the event is the player (assuming the player has the "Player" tag)
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the algae object
        }
    }
}