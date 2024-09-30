using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public AudioSource audioSource; // Reference to your AudioSource
    public AudioReverbFilter reverbFilter; // Reference to the Reverb Filter
    public GameObject deathScreenPanel; // Reference to the Panel for the death screen
    public RectTransform scoreUI; // Reference to the Score UI's RectTransform
    public Text score;
    public float fadeDuration = 3f; // Time for the fade and reverb effect
    public Vector2 targetPosition; // Target position for the score to move to
    public Vector3 targetScale = new Vector3(2f, 2f, 1f); // Target scale for the score
    public float moveDuration = 1.5f; // Time it takes for the score to move to its final position
    public CameraFollow cam;

    private Vector2 originalScorePosition; // To store the original score UI position
    private Vector3 originalScoreScale; // To store the original scale of the score UI
    private float startVolume;
    private float reverbInitialLevel = 0f;
    private Coroutine deathCoroutine; // Store the active coroutine

    void Start()
    {
        // Initialize values
        startVolume = audioSource.volume;
        reverbFilter.enabled = false;
        reverbInitialLevel = reverbFilter.reverbLevel;
        originalScorePosition = scoreUI.anchoredPosition; // Store original position of the score UI
        originalScoreScale = scoreUI.localScale; // Store original scale of the score UI
        deathScreenPanel.SetActive(false); // Hide the death screen at start
    }

    public void TriggerDeath()
    {
        // Stop any ongoing death coroutine
        if (deathCoroutine != null) 
        {
            StopCoroutine(deathCoroutine);
        }
        
        // Start the death sequence
        cam.TriggerZoomAndTilt();
        deathCoroutine = StartCoroutine(FadeOutAndReverb());
        ShowDeathScreen();
        StartCoroutine(MoveScoreUI());
    }

    IEnumerator FadeOutAndReverb()
    {
        float timeElapsed = 0f;
        reverbFilter.enabled = true;

        while (timeElapsed < fadeDuration)
        {
            // Fade the volume out over time
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);

            // Increase the reverb level to make the sound more distant
            reverbFilter.reverbLevel = Mathf.Lerp(reverbInitialLevel, 2000f, timeElapsed / fadeDuration); // Adjust as needed

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume and reverb are fully set at the end
        audioSource.volume = 0f;
        reverbFilter.reverbLevel = 2000f; // Final reverb level, adjust if needed
    }

    void ShowDeathScreen()
    {
        deathScreenPanel.SetActive(true); // Show the death screen panel
        // Additional logic can be added here to stop player controls or pause the game
    }

    IEnumerator MoveScoreUI()
    {
        
        float timeElapsed = 0f;

        // Smoothly move the score UI from its original position to the target position
        while (timeElapsed < moveDuration)
        {
            // Lerp position
            //scoreUI.anchoredPosition = Vector2.Lerp(originalScorePosition, targetPosition, timeElapsed / moveDuration);
            // Lerp scale
            scoreUI.localScale = Vector3.Lerp(originalScoreScale, targetScale, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the score is exactly at the target position and scale at the end
        //scoreUI.anchoredPosition = targetPosition;
        //scoreUI.localScale = targetScale;
    }
}
