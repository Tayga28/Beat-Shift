using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobilePlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float baseStrafeSpeed;
    private float strafeSpeed;
    private float horizontalMove;
    public float forwardSpeed;
    public float baseMaxSpeed;
    public float maxSpeed;
    public float speedModifier = 1f;
    private float timeAdditionforStrafeSpeed;
    private float timeAdditionforForwardSpeed;

    public Text deadText;
    

    Rigidbody rb;

    [Header("Other")]
    public bool isDead;
    public bool isPlaytesting;
    public bool moveLeft;
    public bool moveRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        isDead = false;
        //deadText.gameObject.SetActive(true);
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


    // Update is called once per frame
    private void Update()
    {   
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(moveLeft)
        {
            horizontalMove = -strafeSpeed;
        }
        else if(moveRight)
        {
            horizontalMove = strafeSpeed;
        } 
        else if(!moveLeft)
        {
            horizontalMove = 0;
        }
        else if(!moveRight)
        {
            horizontalMove = 0;
        }
    }

    private void FixedUpdate()
    {
        timeAdditionforForwardSpeed = Time.time / 10;
        timeAdditionforStrafeSpeed = Time.time / 12;
        maxSpeed = baseMaxSpeed + timeAdditionforForwardSpeed;
        strafeSpeed = baseStrafeSpeed + timeAdditionforStrafeSpeed;

        if (isDead == false)
        {
            //rb.velocity += new Vector3(horizontalMove * speedModifier * Time.deltaTime, 0, forwardSpeed * speedModifier * Time.deltaTime);
            rb.AddForce(Vector3.forward * 15 * speedModifier, ForceMode.Force);
            rb.AddForce(Vector3.right * horizontalMove * speedModifier, ForceMode.Force);
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(!isPlaytesting)
        {
            if(collisionInfo.collider.CompareTag("DeathObject"))
            {
                isDead = true;
                forwardSpeed = 0;
                strafeSpeed = 0;

                deadText.gameObject.SetActive(true);
            }
        }
    }

    
}
