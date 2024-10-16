using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksHandler : MonoBehaviour
{
    public enum BlockType
    {
        None, Sticky, Bouncy, SpeedUp, AntiGravity, AntiAntiGravity
    }
    public BlockType currentType;
    public Camera playerCam;
    public bool dynamicFOV;
    public float fovZoomAmount;
    public float fovZoomTime;
    Rigidbody rb;
    //public MobilePlayerMovement player;
    [Header("Player")]
    PlayerMovement player;
    public float bounceForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fovZoomAmount, fovZoomTime);
        } else {
            playerCam.fieldOfView = 60;
        }
        if (currentType == BlockType.Bouncy)
        {
            //Debug.Log("Bounce");
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        } 
        else if (currentType == BlockType.SpeedUp)
        {
            //Debug.Log("Speedy");
            //player.speedModifier = 2;
            //if(dynamicFOV) playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fovZoomAmount, fovZoomTime * Time.deltaTime);
            player.maxSpeed = 1000;
            //currentType = BlockType.None;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.CompareTag("SpeedUpBlock"))
        {
            currentType = BlockType.SpeedUp;
        } 
        else if (collisionInfo.collider.CompareTag("BouncyBlock"))
        {
            currentType = BlockType.Bouncy;
        } else {
            currentType = BlockType.None;
        }
    }

    void OnCollisionExit(Collision collsionInfo) 
    {
        currentType = BlockType.None;
    }

    
}
