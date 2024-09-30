using System.Collections;
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

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // Follow the target position smoothly
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void TriggerZoomAndTilt()
    {
        StartCoroutine(ZoomAndTilt());
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
        //offset = zoomedOffset;
        //transform.rotation = Quaternion.Euler(tiltedRotation);
    }
}
