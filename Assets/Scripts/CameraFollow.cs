using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera follows
    public Vector3 offset; // The offset position of the camera
    public float smoothSpeed = 0.125f; // Smoothness factor for the camera movement
    public float zoomDuration = 1f; // Duration for the zoom effect
    public Vector3 zoomedOffset; // Target offset for the zoomed camera
    public Vector3 tiltedRotation; // Target rotation for the tilted camera
    public float targetFOV = 40f;
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // Follow the target position smoothly
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    public void TriggerZoomAndTilt()
    {
        StartCoroutine(ZoomAndTilt());
    }

    // Method to trigger camera shake
    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position; // Save original position

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Calculate shake offsets based on the original position
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Update the position directly with the desired position and shake offsets
            Vector3 desiredPosition = target.position + offset + new Vector3(x, y, 0);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            elapsed += Time.deltaTime; // Increment elapsed time

            yield return null; // Wait for next frame
        }

        // Ensure the camera ends at the desired position
        transform.position = target.position + offset; // Reset to desired position
    }

    private IEnumerator ZoomAndTilt()
    {
        Vector3 originalOffset = offset;
        Quaternion originalRotation = transform.rotation;
        float originalFOV = camera.fieldOfView;

        float timeElapsed = 0f;

        while (timeElapsed < zoomDuration)
        {
            // Interpolate between the original and zoomed offset
            offset = Vector3.Lerp(originalOffset, zoomedOffset, timeElapsed / zoomDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(tiltedRotation), timeElapsed / zoomDuration);

            // Interpolate the field of view
            camera.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, timeElapsed / zoomDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera ends at the target offset and rotation
        offset = zoomedOffset;
        transform.rotation = Quaternion.Euler(tiltedRotation);
    }
}
