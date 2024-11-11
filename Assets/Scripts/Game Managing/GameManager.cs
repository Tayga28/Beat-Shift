using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CameraFollow cam;
    public bool menuMode;
    public bool gameMode;

    void Start()
    {
        menuMode = true;
        gameMode = false;
    }
    
    public void RestartGame()
    {   
        //cam = FindAnyObjectByType<CameraFollow>();
        SceneManager.LoadScene(0);
    }
    // Ensure to subscribe to the scene loaded event when the game starts
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe to avoid memory leaks
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Callback method that is triggered when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug: Log which scene was loaded
        Debug.Log("Scene Loaded: " + scene.name);

        // Check if the scene is the game scene
        if (scene.name == "GameTestMenu")
        {
            // Find the player and initialize relevant components
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = true; // Make sure the movement script is enabled
                    playerMovement.isPlaytesting = true;
                    playerMovement.hasGameplayStarted = true;
                    playerMovement.isMobileControls = true;

                    // Optionally, you can set the player’s position here if needed
                    //player.transform.position = new Vector3(0, 0, 0); // Reset to starting position
                }

            
            }
        }

        // Optionally, you can handle other scene-specific initialization here
        // For example, if you want to disable UI components in the game scene, do so here
    }
}
