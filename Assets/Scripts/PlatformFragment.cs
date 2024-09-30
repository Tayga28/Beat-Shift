using UnityEngine;

public class PlatformFragment : MonoBehaviour
{
    public int fragmentID;  // Unique ID for this platform fragment
    PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        // Check if player has a valid current platform
        if (player != null && player.currentPlatform != null)
        {
            // Get the parent transform of the current platform
            Transform parentPlatformTransform = player.currentPlatform.transform;

            if (parentPlatformTransform != null)
            {
                // Calculate the difference in Y positions
                float yDifference = player.transform.position.y - parentPlatformTransform.position.y;

                // Check if the player is outside the threshold
                if (Mathf.Abs(yDifference) > player.fallDistanceBeforeDeath)
                {
                    // Trigger death if the player falls below the threshold
                    player.isAlive = false;
                    player.died.TriggerDeath();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the player entered the platform
        {
            // Update the player's current platform to this platform's parent
            player.currentPlatform = this.transform.parent.gameObject;

            // Log for debugging
            //Debug.Log("Player entered fragment ID: " + fragmentID);

            // Update the score based on fragment ID
            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            scoreKeeper.UpdateScore(fragmentID);
        }
    }
}
