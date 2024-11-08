using System.Collections;
using UnityEngine;

public class PlatformFragment : MonoBehaviour
{
    [Header("Platform Identifiers")]
    public int fragmentID; // Unique ID for this platform fragment
    private Color blueColor = new Color(0f, 114f / 255f, 115f / 255f); // Color for blue
    private Color pinkColor = new Color(194f / 255f, 45f / 255f, 107f / 255f); // Color for pink
    public bool isBlue; // Bool to determine if the platform starts as blue
    public bool isPink; // Bool to determine if the platform starts as pink

    private PlayerMovement player;
    private SpectrumAnalyzer spectrum;

    [Header("Reveal and Shift Values")]
    public float revealDistance = 28f; // Distance within which the material is revealed
    private Renderer platformRenderer; // Renderer of the platform
    private Material currentMaterial; // Current material of the platform
    [SerializeField] private bool isChangingColor; // To prevent multiple color changes at once
    [SerializeField] private bool hasRevealedColor = false; // To track if the color has been revealed already
    [SerializeField] private float nextToggleTime; // Time for the next color toggle
    [SerializeField] private float toggleIntervals = 8f;
    [SerializeField] private float toggleDelayAfterReveal = 2f; // Delay before first color toggle after reveal

    [Header("Camera Shake Values")]
    public bool enableCameraShake = true; // Toggle for playtesting camera shake
    public float shakeDuration = 0.2f; // Duration for the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    [Header("Platform Colour")]
    private Color currentColor; // Store current color

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        platformRenderer = GetComponent<Renderer>();
        spectrum = FindObjectOfType<SpectrumAnalyzer>();

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
        // Check if player has a valid current platform
        if (player != null && player.currentPlatform != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // If the player is within the reveal distance, reveal the color once
            if (distanceToPlayer < revealDistance && !hasRevealedColor)
            {
                StartCoroutine(RevealColor());
            }

            // Only toggle color after revealing is done
            if (hasRevealedColor && !isChangingColor && spectrum.isAllowedColourToggle)
            {
                StartCoroutine(ToggleColor());
            }
        }
    }

    private IEnumerator RevealColor()
    {
        if (!isChangingColor) // Ensure no reveal overlap
        {
            isChangingColor = true; // Mark that color is being revealed

            // Lerp from black to the original color based on the bools
            Color targetColor = Color.black;

            if (isBlue) targetColor = blueColor;
            else if (isPink) targetColor = pinkColor;

            float lerpDuration = 0.5f;
            float elapsed = 0f;

            // Lerp from black to the target color
            while (elapsed < lerpDuration)
            {
                elapsed += Time.deltaTime;
                currentMaterial.color = Color.Lerp(Color.black, targetColor, elapsed / lerpDuration);
                yield return null;
            }

            hasRevealedColor = true; // Mark that the reveal has completed
            isChangingColor = false; // Allow color toggling now

            // Add a delay before allowing color toggling to start
            nextToggleTime = Time.time + toggleDelayAfterReveal; // Delay toggle by a few seconds
        }
    }

    private IEnumerator ToggleColor()
    {
        player.isInvincible = true; // Start invincibility early
        spectrum.isAllowedColourToggle = false;
        // Brief camera shake before toggling color
        if (enableCameraShake)
        {
            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            if (cameraFollow != null && player.isAlive)
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
        UpdateTag();

        // Delay invincibility slightly
        yield return new WaitForSeconds(1f); // Small delay after color switch

        player.isInvincible = false; // End invincibility after all changes
        //UpdateTag(); // Update tag after color change
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
        //Debug.Log(this.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the platform
        {
            if(player.checkingForFirstContact) // Checking for the first contact with the level to start playing the music when we hit the ground instead of just hitting the play game button;
            {
                player.madeFirstContactWithLevel = true;
                player.checkingForFirstContact = false; 
            }
            if(fragmentID == 11 || fragmentID == 12)
            {
                player.forwardSpeed = 30f;
                player.sphereCol.enabled = true;
                player.boxCol.enabled = false;
            }
            // Update the player's current platform to this platform's parent
            player.currentPlatform = this.transform.parent.gameObject;

            // Update the score based on fragment ID
            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            scoreKeeper.UpdateScore(fragmentID);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            if(fragmentID == 11 || fragmentID == 12)
            {
                player.forwardSpeed = 10f;
                player.boxCol.enabled = true;
                player.sphereCol.enabled = false;
            }
            this.GetComponent<PlatformFragment>().enabled = false;
        }
    }
}