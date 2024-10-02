using System.Collections;
using UnityEngine;

public class PlatformFragment : MonoBehaviour
{
    public int fragmentID; // Unique ID for this platform fragment
    private Color blueColor = new Color(0f, 114f / 255f, 115f / 255f); // Color for blue
    private Color pinkColor = new Color(194f / 255f, 45f / 255f, 107f / 255f); // Color for pink
    public bool isBlue; // Bool to determine if the platform starts as blue
    public bool isPink; // Bool to determine if the platform starts as pink
    private PlayerMovement player;
    private float revealDistance = 28f; // Distance within which the material is revealed
    private Renderer platformRenderer; // Renderer of the platform
    private Material currentMaterial; // Current material of the platform
    private bool isChangingColor; // To prevent multiple color changes at once
    public bool enableCameraShake = true; // Toggle for playtesting camera shake
    public float shakeDuration = 0.2f; // Duration for the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude of the camera shake
    private float nextToggleTime; // Time for the next color toggle
    private float toggleIntervals = 8f;
    private Color currentColor; // Store current color

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        platformRenderer = GetComponent<Renderer>();

        // Create a new material instance for the platform
        currentMaterial = new Material(platformRenderer.material);
        platformRenderer.material = currentMaterial;

        // Set the initial color to black (masking color)
        currentMaterial.color = Color.black; // Start as black

        // Initially set the next toggle time
        nextToggleTime = Time.time + toggleIntervals; // Start the first toggle after 16 seconds
    }

    void Update()
    {
        //Debug.Log(gameObject.tag);
        // Check if player has a valid current platform
        if (player != null && player.currentPlatform != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // If the player is within the reveal distance, reveal the color
            if (distanceToPlayer < revealDistance)
            {
                StartCoroutine(RevealColor());

                // Check if it's time to toggle color
                if (Time.time >= nextToggleTime)
                {
                    StartCoroutine(ToggleColor());
                    nextToggleTime += toggleIntervals; // Set the next toggle time
                }
            }
        }
    }

    private IEnumerator RevealColor()
    {
        if (!isChangingColor)
        {
            isChangingColor = true;

            // Lerp from black to the original color based on the bools
            Color targetColor = Color.black; // Start as black

            if (isBlue) targetColor = blueColor;
            else if (isPink) targetColor = pinkColor;

            float lerpDuration = 0.5f; // Duration for lerping
            float elapsed = 0f;

            // Lerp from black to the target color
            while (elapsed < lerpDuration)
            {
                elapsed += Time.deltaTime;
                currentMaterial.color = Color.Lerp(Color.black, targetColor, elapsed / lerpDuration);
                yield return null;
            }

            // Ensure the final color is set
            //currentMaterial.color = targetColor;

            //isChangingColor = false; // Allow subsequent reveals
        }
    }

    private IEnumerator ToggleColor()
    {
        player.isInvincible = true; // Start invincibility early
        // Brief camera shake before toggling color
        if (enableCameraShake)
        {
            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.TriggerShake(shakeDuration, shakeMagnitude);
            }
        }

        // Fade to black before changing color
        currentColor = currentMaterial.color; // Store current color
        Color blackColor = Color.black;

        // Lerp to black variables
        float fadeDuration = 0.2f;
        float elapsed = 0f;

        // Lerping to Black
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            
            currentMaterial.color = Color.Lerp(currentColor, blackColor, elapsed / fadeDuration);
            yield return null;
        }

        // Finish lerping by setting the material color to black
        //currentMaterial.color = blackColor;
        //UpdateTag();

        // Toggle to the new color
        Color newColor = isBlue ? pinkColor : blueColor;


        // Fade back to the new color
        elapsed = 0f; // Reset elapsed time for the fade back
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            currentMaterial.color = Color.Lerp(blackColor, newColor, elapsed / fadeDuration);
            yield return null;
        }

        // After color fully switches
        isBlue = !isBlue;
        isPink = !isPink;
        UpdateTag(); // Update tag after color change

        // Delay invincibility slightly
        yield return new WaitForSeconds(0.5f); // Small delay after color switch

        player.isInvincible = false; // End invincibility after all changes
    }

    private void UpdateTag()
    {
        if (currentMaterial.color == blueColor)
        {
            this.tag = "Blue";
        }
        else if (currentMaterial.color == pinkColor)
        {
            this.tag = "Pink";
        }
        Debug.Log(this.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the platform
        {
            // Update the player's current platform to this platform's parent
            player.currentPlatform = this.transform.parent.gameObject;

            // Update the score based on fragment ID
            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            scoreKeeper.UpdateScore(fragmentID);
        }
    }
}
