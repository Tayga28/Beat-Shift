using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumFollow : MonoBehaviour
{
    public PlayerMovement player;
    public Transform target; // The player
    public float smoothSpeed = 0.125f; // Smoothing factor for movement
    //public Vector3 offset;

    private void FixedUpdate()
    {
        if (player.madeFirstContactWithLevel)
        {
            // The spectrogram should follow the player’s Y and Z positions, but keep its X position fixed.
            Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y, target.position.z);

            // Smooth the transition
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Apply the new position
            transform.position = smoothedPosition;
        }

    }
}