using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOrbit : MonoBehaviour
{
    public Transform target; // The target the camera will orbit around
    public Vector3 offset; // The initial offset to maintain from the target
    public float orbitSpeed = 20f; // Speed of the camera orbit
    public float zoomDuration = 1f; // Duration for zoom-in and zoom-out
    public float zoomedInFOV = 30f; // FOV when zoomed in
    public float zoomedOutFOV = 60f; // FOV after scene transition
    public bool isReadyToZoomOut = false;

    private Camera camera;
    private bool isOrbiting = true;
    private float orbitAngle = 0f; // Current angle around the target

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (isOrbiting)
        {
            OrbitAroundTarget();
        }
    }

    private void OrbitAroundTarget()
    {
        // Increment orbit angle based on orbit speed
        orbitAngle += orbitSpeed * Time.deltaTime;
        orbitAngle %= 360f; // Wrap angle to keep it within 0-360 degrees

        // Calculate the new position on the orbit path around the target
        float orbitRadius = offset.magnitude; // Distance from target
        float x = target.position.x + Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitRadius;
        float z = target.position.z + Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitRadius;

        // Set the new position and make the camera look at the target
        transform.position = new Vector3(x, target.position.y + offset.y, z);
        transform.LookAt(target);
    }

    public void OnPlayButtonPressed()
    {
        isOrbiting = false; // Stop orbiting
        orbitSpeed = 0f; // Ensure speed is zero
        StartCoroutine(ZoomInToFaceAndTransition());
    }

    private IEnumerator ZoomInToFaceAndTransition()
    {
        float initialFOV = camera.fieldOfView;
        Vector3 initialPosition = transform.position;
        Vector3 facePosition = target.position + target.forward * offset.magnitude;

        float timeElapsed = 0f;

        // Smoothly transition to the face position while zooming in
        while (timeElapsed < zoomDuration)
        {
            camera.fieldOfView = Mathf.Lerp(initialFOV, zoomedInFOV, timeElapsed / zoomDuration);
            transform.position = Vector3.Lerp(initialPosition, facePosition, timeElapsed / zoomDuration);
            transform.LookAt(target); // Keep camera looking at target's center
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera ends up in front of the face with correct FOV
        camera.fieldOfView = zoomedInFOV;
        transform.position = facePosition;
        transform.LookAt(target);

        PlayerPrefs.SetInt("IsReadyToZoomOut", 1); // Save as 1 (true)
        PlayerPrefs.Save(); // Ensure it's saved immediately
        // Load the next scene after the zoom-in effect
        SceneManager.LoadScene("GameTestMenu"); // Replace with your scene's name
        isReadyToZoomOut = true;

        // Wait for the scene to load, then zoom out in the new scene
        // StartCoroutine(ZoomOutInNewScene());
    }

    
}
