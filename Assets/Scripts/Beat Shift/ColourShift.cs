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
        playerMaterial = new Material(player.GetComponent<Renderer>().material);
        player.GetComponent<Renderer>().material = playerMaterial; // Apply the instance

        playerMaterial.color = blueColor; // Set initial color to blue
        isBlue = true;                     // Initialize to blue
        colourIndex = 0;                  // Set initial colour index
    }

    void Update()
    {
        if (movement.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isLerping)
            {
                StartColorSwitch();
            }
            if(movement.isMobileControls && movement.moveLeft && movement.moveRight && !isLerping) StartColorSwitch();

            if (isLerping)
            {
                ContinueLerp();
            }
        }
    }

    void StartColorSwitch()
    {
        if (isBlue)
        {
            isPink = true;
            isBlue = false;
            startColor = blueColor;
            targetColor = pinkColor;
            colourIndex = 1; // Immediately set to pink for accurate collision checks
        }
        else if (isPink)
        {
            isBlue = true;
            isPink = false;
            startColor = pinkColor;
            targetColor = blueColor;
            colourIndex = 0; // Immediately set to blue for accurate collision checks
        }

        lerpTime = 0; // Reset lerp time
        isLerping = true; // Begin lerping
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
        }
    }
}
