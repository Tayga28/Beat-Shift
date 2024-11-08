using UnityEngine;
using UnityEngine.UI;

public class TextPulse : MonoBehaviour
{
    public float pulseSpeed = 1.0f; // Adjust the speed of pulsing
    public float maxSize = 1.2f;    // Maximum scale size (relative to the original size)

    private Vector3 originalScale;

    void Start()
    {
        // Save the original scale of the text
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calculate the scale factor based on a sine wave
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * (maxSize - 1);
        
        // Apply the calculated scale
        transform.localScale = originalScale * scale;
    }
}
