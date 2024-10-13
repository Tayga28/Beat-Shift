using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlaytesting;
    [SerializeField] private Rigidbody rb;
    public float forwardSpeed;
    public float sideSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float fallDistanceBeforeDeath = 10f;
    public GameObject currentPlatform;
    public bool isGrounded;
    public bool isAlive;
    public bool isInvincible;
    public ColourShift cs;
    public PlayerDeath died;
    float horizontalMove;
    [SerializeField] private string currentGroundTag = ""; // Store the tag of the ground we're on

    [Header("Mobile Stuff")]
    public bool moveLeft;
    public bool moveRight;
    public bool isMobileControls;

    void Start()
    {
        isAlive = true;
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
        if (isMobileControls) MovePlayer();
        if (Input.GetKeyDown(KeyCode.M) && isGrounded)
        {
            rb.AddForce(0, 1 * jumpForce, 0, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (!isMobileControls) horizontalMove = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;

        rb.AddForce(0, 0, forwardSpeed);
        if (!isMobileControls) rb.AddForce(horizontalMove * sideSpeed, 0, 0);
        if (isMobileControls)  rb.AddForce(horizontalMove, 0, 0);

        if (velocity.z > maxSpeed)
        {
            velocity.z = maxSpeed;
            velocity.x = rb.velocity.x;
        }
        rb.velocity = velocity;

        // Continuously check if player and ground colors match after collision
        if (!isPlaytesting && !string.IsNullOrEmpty(currentGroundTag))
        {
            CheckColorMismatch();
        }
    }

    private void CheckColorMismatch()
    {
        Debug.Log($"Current Ground Tag: {currentGroundTag}, Player Colour Index: {cs.colourIndex}");

        if (!isInvincible) // Only check when not invincible
        {
            // Check for color mismatches
            if (currentGroundTag == "Blue" && cs.colourIndex != 0)
            {
                Debug.Log("Player is pink on blue ground. Triggering death.");
                maxSpeed = 0;
                isAlive = false;
                died.TriggerDeath();
            }
            else if (currentGroundTag == "Pink" && cs.colourIndex != 1)
            {
                Debug.Log("Player is blue on pink ground. Triggering death.");
                maxSpeed = 0;
                isAlive = false;
                died.TriggerDeath();
            }
        }
    }

    public void UpdateGroundTag(string newTag)
    {
        currentGroundTag = newTag;
    }

    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
        CheckColorMismatch();

        if (!isPlaytesting)
        {
            if (other.gameObject.CompareTag("Blue"))
            {
                UpdateGroundTag("Blue");
                if (!isInvincible) cs.colourIndex = 0; // Set player color index only if not invincible
                CheckColorMismatch();
            }
            else if (other.gameObject.CompareTag("Pink"))
            {
                UpdateGroundTag("Pink");
                if (!isInvincible) cs.colourIndex = 1; // Set player color index only if not invincible
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
