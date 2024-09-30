using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollow : MonoBehaviour
{
    public Transform target;
    public float offset;
    public float smoothSpeed;
    
    private void FixedUpdate() 
    {
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
