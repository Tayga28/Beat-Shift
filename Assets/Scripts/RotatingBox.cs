using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBox : MonoBehaviour
{
    public GameObject box;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateBox(rotSpeed);
    }

    void RotateBox(float _speed) 
    {
        box.transform.Rotate(0, 5 * _speed * Time.deltaTime, 0, Space.Self);
    }
}
