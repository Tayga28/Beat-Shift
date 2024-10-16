using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Playtesting")]
    public bool isPlaytesting;

    [Header("Player Values")]
    [SerializeField] private Rigidbody rb;
    public float forwardSpeed;
    public float sideSpeed;
    public float maxSpeed;
    public float jumpForce;
    float horizontalMove;
    [SerializeField] private float rotationStraightenSpeed = 1.0f; // Speed of straightening
    private float currentYRotation;

    [Header("Death Handling")]
    public PlayerDeath died;
    public bool isAlive;
    public bool isDying;
    public float fallDistanceBeforeDeath = 10f;

    [Header("Checks")]
    public GameObject currentPlatform;
    float platformYPosition; // Store the Y position of the current platform
    [SerializeField] private string currentGroundTag = ""; // Store the tag of the ground we're on
    public bool isGrounded;
    public bool isInvincible;
    public bool checkingForFirstContact;
    public bool madeFirstContactWithLevel;

    [Header("Colour Shifting")]
    public ColourShift cs;

    [Header("User Status")]
    public bool userOnMenu;
    public bool hasGameplayStarted;

    [Header("Mobile Stuff")]
    public bool moveLeft;
    public bool moveRight;
    public bool isMobileControls;

    void Start()
    {
        isAlive = true;
        checkingForFirstContact = true;
        madeFirstContactWithLevel = false;
        maxSpeed = Mathf.Min(forwardSpeed, maxSpeed);
    }

    public void OnPointerDownLeft()
    {
        moveLeft = true;
    }

    public void OnPointerUpLeft()
    {
        moveLeft = false;
    }

    public void OnPointerDownRight()
    {
        moveRight = true;
    }

    public void OnPointerUpRight()
    {
        moveRight = false;
    }
    private void MovePlayer() // Mobile Controls
    {
        if (moveLeft)
        {
            horizontalMove = -sideSpeed;
        }
        else if (moveRight)
        {
            horizontalMove = sideSpeed;
        }
        else if (!moveLeft)
        {
            horizontalMove = 0;
        }
        else if (!moveRight)
        {
            horizontalMove = 0;
        }
    }

    void Update()
    {
        if (!hasGameplayStarted) return;
        if (isMobileControls) MovePlayer();
        if (Input.GetKeyDown(KeyCode.M) && isGrounded)
        {
            rb.AddForce(0, 1 * jumpForce, 0, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (!hasGameplayStarted) return;
        if (!isMobileControls) horizontalMove = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;

        rb.AddForce(0, 0, forwardSpeed);
        if (!isMobileControls) rb.AddForce(horizontalMove * sideSpeed, 0, 0);
        if (isMobileControls) rb.AddForce(horizontalMove, 0, 0);

        if (velocity.z > maxSpeed)
        {
            velocity.z = maxSpeed;
            velocity.x = rb.velocity.x;
        }
        rb.velocity = velocity;

        // Capture player's current Y rotation
        currentYRotation = transform.rotation.eulerAngles.y;

        // Check if the player has spun and isn't at zero Y rotation
        if (currentYRotation != 0)
        {
            // Gradually lerp back to a neutral Y rotation (Quaternion.identity)
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationStraightenSpeed);
        }

        // Continuously check if player and ground colors match after collision
        if (!isPlaytesting && !string.IsNullOrEmpty(currentGroundTag))
        {
            CheckColorMismatch();
        }
        CheckFallDeath();
    }

    void CheckColorMismatch()
    {
        if (!isInvincible && !cs.isLerping && !isDying) // Only check when not invincible, not lerping, and not dying
        {
            // Check for color mismatches
            if (currentGroundTag == "Blue" && cs.isPink)
            {
                // Player switched to pink on blue ground, but let the color shift complete
                isDying = true; // Set dying flag
                cs.StartColorSwitch(true); // Start the color switch (if it hasn’t already)
                Invoke("TriggerDelayedDeath", cs.lerpDuration); // Trigger death after lerp completes
            }
            else if (currentGroundTag == "Pink" && cs.isBlue)
            {
                // Player switched to blue on pink ground, but let the color shift complete
                isDying = true; // Set dying flag
                cs.StartColorSwitch(true); // Start the color switch (if it hasn’t already)
                Invoke("TriggerDelayedDeath", cs.lerpDuration); // Trigger death after lerp completes
            }
        }
    }

    void TriggerDelayedDeath()
    {
        if (!isAlive || !isDying) return; // Prevent triggering death multiple times
        maxSpeed = 0;
        died.TriggerDeath();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isAlive = false;
        isDying = false; // Reset dying flag after death is triggered
    }

    void CheckFallDeath()
    {
        if (!isAlive || isDying) return; // Don't check if already dead or dying

        float playerYPosition = transform.position.y;
        
        // If player falls below the platform by more than the threshold distance
        if (playerYPosition < platformYPosition - fallDistanceBeforeDeath)
        {
            isDying = true; // Mark as dying
            Invoke("TriggerDelayedDeath", 0.5f); // Add delay for visual effect
        }
    }


    public void UpdateGroundTag(string newTag)
    {
        currentGroundTag = newTag;
    }

    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
        platformYPosition = currentPlatform.transform.position.y; // Store platform Y position
        CheckColorMismatch();

        if (!isPlaytesting)
        {
            if (other.gameObject.CompareTag("Blue"))
            {
                UpdateGroundTag("Blue");
                CheckColorMismatch();
            }
            else if (other.gameObject.CompareTag("Pink"))
            {
                UpdateGroundTag("Pink");
                CheckColorMismatch();
            }

            if (other.gameObject.CompareTag("DeathObject"))
            {
                isAlive = false;
                died.TriggerDeath();
            }
        }
    }

    // OnCollisionStay to continuously check collision and tag updates
    void OnCollisionStay(Collision other)
    {
        if (!isPlaytesting)
        {
            // Continuously check for color changes while in contact
            if (other.gameObject.CompareTag("Blue"))
            {
                UpdateGroundTag("Blue");
                CheckColorMismatch(); // Always check for mismatches
            }
            else if (other.gameObject.CompareTag("Pink"))
            {
                UpdateGroundTag("Pink");
                CheckColorMismatch(); // Always check for mismatches
            }

            if (other.gameObject.CompareTag("DeathObject"))
            {
                isAlive = false;
                died.TriggerDeath();
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
        if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Pink"))
        {
            currentGroundTag = "";
        }
    }
}