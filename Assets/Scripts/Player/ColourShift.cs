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
    private Material playerMaterial;         // Material of the player
    private Color startColor;                // Starting color for lerp
    private Color targetColor;               // Target color for lerp
    public float lerpDuration = 1.0f;        // Duration of the lerp
    private float lerpTime;                  // Time elapsed for lerp
    public bool isLerping = false;          // To track if currently lerping
    public bool isSwitchingDuringDeath = false; // New flag to track if switching during death


    void Start()
    {
        playerMaterial = new Material(player.GetComponent<Renderer>().material);
        player.GetComponent<Renderer>().material = playerMaterial; // Apply the instance

        playerMaterial.color = blueColor; // Set initial color to blue
        isBlue = true;                     // Initialize to blue
        //colourIndex = 0;                  // Set initial colour index
    }

    void Update()
    {
        if (movement.hasGameplayStarted && movement.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isLerping && !movement.isMobileControls)
            {
                StartColorSwitch();
            }
            if (movement.isMobileControls && movement.moveLeft && movement.moveRight && !isLerping) StartColorSwitch();

            if (isLerping)
            {
                ContinueLerp();
            }
        }
    }

    public void StartColorSwitch(bool duringDeath = false)
    {
        if (duringDeath)
        {
            isSwitchingDuringDeath = true; // Set the flag if we're switching due to death
        }

        if (!isSwitchingDuringDeath)
        {
            if (isBlue)
            {
                isPink = true;
                isBlue = false;
                startColor = blueColor;
                targetColor = pinkColor;
            }
            else if (isPink)
            {
                isBlue = true;
                isPink = false;
                startColor = pinkColor;
                targetColor = blueColor;
            }

            lerpTime = 0; // Reset lerp time
            isLerping = true; // Begin lerping
        }
    }


    void ContinueLerp()
    {
        if (lerpTime < lerpDuration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            playerMaterial.color = lerpedColor; // Update the material color
        }
        else
        {
            playerMaterial.color = targetColor;
            isLerping = false; // Stop lerping
            isSwitchingDuringDeath = false; // Reset the switching during death flag
        }
    }
}
