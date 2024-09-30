using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [Header("Movement Variables")]
    public float groundAcceleration = 20f;
    public float airAcceleration = 10f;
    public float maxSpeed = 10f;
    public float forwardSpeed = 10f;
    public float jumpForce = 8f;
    public float groundRayLength = 0.2f;

    [Header("Acceleration Variables")]
    public float accelerationFactor = 2f;
    public float accelerationDuration = 0.5f;

    private bool isGrounded;
    private bool isAccelerating;
    private float accelerationTimer;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //isAccelerating = false;
        //accelerationTimer = 0f;
        //rb.freezeRotation = true;
    }

    void Update()
    {
        //Debug.Log(rb.velocity);
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundRayLength);

        // Handle player input
        float horizontalMove = Input.GetAxis("Horizontal");


        // Calculate acceleration based on whether the player is grounded
        //float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        // Adjust speed based on acceleration state
        //float currentSpeed = isAccelerating ? maxSpeed * accelerationFactor : maxSpeed;
        float currentSpeed = 10;


        //Vector3 currentVel = rb.velocity;

        // Calculate the target velocity
        Vector3 targetVelocity = new Vector3(horizontalMove * currentSpeed, 0, forwardSpeed * Time.deltaTime);

        // Smoothly adjust the current velocity towards the target velocity
        //Vector3.SmoothDamp(currentVel, targetVelocity, ref currentVel, acceleration * Time.deltaTime).x;
        rb.velocity += targetVelocity;


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // Handle collision events if needed
    }
}
