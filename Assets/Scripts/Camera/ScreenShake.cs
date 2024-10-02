using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Shaking parameters
    public float shakeDuration = 0.2f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake

    // Method to start the shake
    public IEnumerator Shake()
    {
        Vector3 originalPosition = transform.localPosition; // Store the original position of the camera
        float elapsed = 0.0f; // Time elapsed since the shake started

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude; // Random X offset
            float y = Random.Range(-1f, 1f) * shakeMagnitude; // Random Y offset

            transform.localPosition = new Vector3(x, y, originalPosition.z); // Apply the shake

            elapsed += Time.deltaTime; // Increment elapsed time

            yield return null; // Wait until the next frame
        }

        //transform.localPosition = originalPosition; // Reset to the original position
    }
}
