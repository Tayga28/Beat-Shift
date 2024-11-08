using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameManager gameManager;
    public Transform target; // The target the camera follows
    public Vector3 offset; // The offset position of the camera
    public float smoothSpeed = 0.125f; // Smoothness factor for the camera movement
    public float zoomDuration = 1f; // Duration for the zoom effect
    public float zoomedOutFOV = 120f;
    public bool hasZoomedOut;
    public Vector3 zoomedOffset; // Target offset for the zoomed camera
    public Vector3 tiltedRotation; // Target rotation for the tilted camera
    public float targetFOV = 40f;
    private Camera camera;

    private void Start()
    {
        hasZoomedOut = false;
        camera = GetComponent<Camera>();
        // Check if the camera should zoom out upon entering the scene
        if (PlayerPrefs.GetInt("IsReadyToZoomOut", 0) == 1)
        {
            PlayerPrefs.SetInt("IsReadyToZoomOut", 0); // Reset for next session
            StartCoroutine(ZoomOutInNewScene());
        }
    }

    private IEnumerator ZoomOutInNewScene()
{
    // Store the current position and field of view
    Vector3 initialPosition = transform.position;
    float initialFOV = camera.fieldOfView;

    // Calculate the target position for the camera (target's position + offset)
    Vector3 targetPosition = target.position + offset;

    // Duration of the zoom-out effect (adjust this as needed)
    float timeElapsed = 0f;
    float zoomDuration = 1f; // Duration of the zoom-out effect

    // Smoothly transition to the target position and field of view
    while (timeElapsed < zoomDuration)
    {
        // Lerp the position and field of view
        transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed / zoomDuration);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(5, 0, 0), timeElapsed/zoomDuration);
        camera.fieldOfView = Mathf.Lerp(initialFOV, zoomedOutFOV, timeElapsed / zoomDuration);

        // Increment time
        timeElapsed += Time.deltaTime;
        yield return null;
    }

    // Ensure the camera ends at the target position and field of view
    transform.position = targetPosition;
    transform.rotation = Quaternion.Euler(5, 0, 0);
    camera.fieldOfView = zoomedOutFOV;
    

    // If needed, you can set the camera's rotation here (for example, reset to follow the target)
    //transform.LookAt(target);
    hasZoomedOut = true;
}


    private void FixedUpdate()
    {
        if(hasZoomedOut)
        {
            // Follow the target position smoothly
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        
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
        camera.transform.SetParent(target.transform);
        offset = zoomedOffset;
        transform.rotation = Quaternion.Euler(tiltedRotation);
        
    }
}