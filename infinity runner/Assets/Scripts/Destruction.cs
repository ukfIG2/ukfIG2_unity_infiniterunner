using UnityEngine;

public class Destruction : MonoBehaviour
{
    private GameManager gameManager; // Reference to the GameManager

    public void Start()
    {
        // Find the GameManager instance in the scene
        gameManager = FindObjectOfType<GameManager>();

        // Check if the GameManager is found to avoid null reference issues
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "sideWall" or "road"
        if (other.CompareTag("sideWall") || other.CompareTag("road"))
        {
            // Notify the GameManager and destroy the object
            gameManager?.NotifyRoadDestroyed(); // Use null-check to avoid errors
            Destroy(other.gameObject);
        }
    }
}
