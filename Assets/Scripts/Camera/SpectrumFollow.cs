using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumFollow : MonoBehaviour
{
    public PlayerMovement player;
    public Transform target; // The player
    public float smoothSpeed = 0.125f; // Smoothing factor for movement

    private void FixedUpdate()
    {
        if (player.madeFirstContactWithLevel)
        {
            // Start with the desired position only following z position
            Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, target.position.z - 12);

            // If the player is grounded, update the y position to follow the player
            if (player.isGrounded)
            {
                desiredPosition.y = target.position.y;
            }

            // Smoothly transition to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

            // Apply the smoothed position
            transform.position = smoothedPosition;
        }
    }
}
