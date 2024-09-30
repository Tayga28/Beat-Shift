using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourShift : MonoBehaviour
{
    public GameObject player;               // Reference to the player GameObject
    public PlayerMovement movement;
    public Color blueColor = new Color(0f, 0.5f, 0.5f); // Custom blue color
    public Color pinkColor = Color.magenta;  // Color for pink
    public bool isBlue;                      // To track if the player is blue
    public bool isPink;                      // To track if the player is pink
    public int colourIndex;                  // 0 for blue, 1 for pink

    private Material playerMaterial;         // Material of the player
    private Color startColor;                // Starting color for lerp
    private Color targetColor;               // Target color for lerp
    public float lerpDuration = 1.0f;        // Duration of the lerp
    private float lerpTime;                  // Time elapsed for lerp
    private bool isLerping = false;          // To track if currently lerping

    void Start()
    {
        // Create a new instance of the player's material to avoid shared material issues
        playerMaterial = new Material(player.GetComponent<Renderer>().material);
        player.GetComponent<Renderer>().material = playerMaterial; // Apply the instance

        // Set initial color to blue
        playerMaterial.color = blueColor;
        isBlue = true;                    // Initialize to blue
        colourIndex = 0;                 // Set initial colour index
    }

    void Update()
    {
        if (movement.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isLerping)
            {
                // Switch colors when space is pressed
                if (isBlue)
                {
                    isPink = true;
                    isBlue = false;
                    startColor = blueColor; // Save the current blue color
                    targetColor = pinkColor; // Set the target color to pink
                }
                else if (isPink)
                {
                    isBlue = true;
                    isPink = false;
                    startColor = pinkColor; // Save the current pink color
                    targetColor = blueColor; // Set the target color to blue
                }

                lerpTime = 0; // Reset lerp time when switching colors
                isLerping = true; // Indicate that lerping is in progress
            }

            // Lerp only if lerpTime is less than lerpDuration
            if (isLerping)
            {
                if (lerpTime < lerpDuration)
                {
                    lerpTime += Time.deltaTime;
                    float t = lerpTime / lerpDuration;
                    Color lerpedColor = Color.Lerp(startColor, targetColor, t);
                    playerMaterial.color = lerpedColor; // Change the color of the player's material
                }
                else
                {
                    // Finalize the current color after lerping
                    isLerping = false; // Reset lerping state
                    if (isBlue)
                    {
                        colourIndex = 0; // Update colour index
                    }
                    else if (isPink)
                    {
                        colourIndex = 1; // Update colour index
                    }
                }
            }
        }
    }
}
