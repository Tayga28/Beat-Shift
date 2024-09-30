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
    public ColourShift cs;
    public PlayerDeath died;
    private string currentGroundTag = ""; // Store the tag of the ground we're on
    
    //public MusicBehaviour music;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        maxSpeed = Mathf.Min(forwardSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) & isGrounded)
        {
            //Debug.Log("jumped");
            rb.AddForce(0, 1 * jumpForce, 0, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {

        float horizontalMove = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;
        
        rb.AddForce(0, 0, forwardSpeed);
        rb.AddForce(horizontalMove * sideSpeed, 0, 0);
        
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

     // Check if player and ground colors mismatch and trigger death
    private void CheckColorMismatch()
    {
        if (currentGroundTag == "Blue" && cs.colourIndex != 0)
        {
            maxSpeed = 0;
            isAlive = false;
            died.TriggerDeath();
        }
        else if (currentGroundTag == "Pink" && cs.colourIndex != 1)
        {
            maxSpeed = 0;
            isAlive = false;
            died.TriggerDeath();
        }
    }

    // OnCollisionEnter to detect ground color
    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;

        if (!isPlaytesting)
        {
            // Store the tag of the object we're colliding with
            if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Pink"))
            {
                currentGroundTag = other.gameObject.tag;

                // Initial check on collision
                CheckColorMismatch();
            } 

            if (other.gameObject.CompareTag("DeathObject"))
            {
                isAlive = false;
                died.TriggerDeath();
            }
        }
    }

    // OnCollisionExit to clear the ground tag when player leaves the surface
    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
        if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Pink"))
        {
            currentGroundTag = "";
        }
    }

}
