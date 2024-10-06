using UnityEngine;

public class CoinDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object triggering the event is the player (assuming the player has the "Player" tag)
        if (other.CompareTag("Player"))
        {
            EventManager.CoinCollected();
            Destroy(gameObject); // Destroy the algae object
        }
    }
}
