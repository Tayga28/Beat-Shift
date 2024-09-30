using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float strafeSpeed;
    public float forwardSpeed;
    public float maxSpeed;
    public float fasterForce = 2;

    [Header("Ground Detection")]
    public bool isGrounded;
    public LayerMask groundLayer; // The layer(s) that represent the ground
    public float groundDistance = 0.2f; // The distance to check for ground

    [Header("Gravity")]
    public float groundGravity = 1f;
    public float airGravityMultiplier = 1f;
    public float airDrag = 0.1f;   // Adjust the air drag to control air resistance
    public float gravity = 1f;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
        isGrounded = false;
    }    

    void Update() {
        
    }

    void FixedUpdate ()
	{

        /*if (Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer)) {
            isGrounded = true;

        } else if (!Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer)) {

            isGrounded = false;
        }*/
        
        float horizontalMove = Input.GetAxis("Horizontal");
		// Add a forward force
		rb.AddForce(0, 0, forwardSpeed * 100 * Time.deltaTime);
        rb.AddForce(Vector3.down * gravity * 9.8f);

		if (Input.GetKey("d"))	// If the player is pressing the "d" key
		{
			// Add a force to the right
			rb.AddForce(strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (Input.GetKey("a"))  // If the player is pressing the "a" key
		{
			// Add a force to the left
			rb.AddForce(-strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (rb.position.y < -1f)
		{
			//FindObjectOfType<GameManager>().EndGame();
		}

        if (isGrounded)
        {
            rb.drag = 0.5f;
            gravity = groundGravity;

        } else if (!isGrounded) {
            rb.drag = airDrag;
            gravity = airGravityMultiplier;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
	}
}