using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFragment : MonoBehaviour
{
    public int fragmentID;  // Unique ID for this platform fragment
    public Material blueMaterial; // The blue material to be used
    public Material pinkMaterial; // The pink material to be used
    private PlayerMovement player;
    private float revealDistance = 28f; // Distance within which the material is revealed
    private Renderer platformRenderer; // Renderer of the platform
    private Material originalMaterial; // Original material of the platform
    private Material blackInstanceMaterial; // Instance of the original material set to black
    private float lerpSpeed = 10f; // Speed of the color lerp

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        platformRenderer = GetComponent<Renderer>();

        // Store the original material and create a black instance
        originalMaterial = platformRenderer.material;
        blackInstanceMaterial = new Material(originalMaterial);
        blackInstanceMaterial.color = Color.black;

        // Set the platform's initial material to the black instance
        platformRenderer.material = blackInstanceMaterial;
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

                // Check if the player is outside the threshold for death
                if (Mathf.Abs(yDifference) > player.fallDistanceBeforeDeath)
                {
                    // Trigger death if the player falls below the threshold
                    player.isAlive = false;
                    player.died.TriggerDeath();
                }
            }

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // If the player is within the reveal distance, smoothly reveal the original material
            if (distanceToPlayer < revealDistance)
            {
                // Lerp the color from black to the original material color
                platformRenderer.material.color = Color.Lerp(platformRenderer.material.color, originalMaterial.color, Time.deltaTime * lerpSpeed);
            }
            else
            {
                // Reset to black if the player moves away
                platformRenderer.material.color = Color.Lerp(platformRenderer.material.color, Color.black, Time.deltaTime * lerpSpeed);
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
            // Debug.Log("Player entered fragment ID: " + fragmentID);

            // Update the score based on fragment ID
            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            scoreKeeper.UpdateScore(fragmentID);
        }
    }
}
