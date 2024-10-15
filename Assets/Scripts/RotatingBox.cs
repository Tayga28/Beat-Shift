using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBox : MonoBehaviour
{
    GameObject box;
    public float rotSpeed;
    public bool clockwise;
    public bool chooseRandomDirection;

    // Start is called before the first frame update
    void Start()
    {
        box = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RotateBox(rotSpeed);
    }

    void RotateBox(float _speed) 
    {
        if(!chooseRandomDirection)
        {
            if(clockwise) box.transform.Rotate(0, 5 * _speed * Time.deltaTime, 0, Space.Self);
            if(!clockwise) box.transform.Rotate(0, -5 * _speed * Time.deltaTime, 0, Space.Self);
        } else if (chooseRandomDirection)
        {
            int rando = Random.Range(1, 2);
            if (rando == 1)
            {
                box.transform.Rotate(0, 5 * _speed * Time.deltaTime, 0, Space.Self);
            } else if (rando == 2)
            {
                box.transform.Rotate(0, -5 * _speed * Time.deltaTime, 0, Space.Self);
            }
        }
        
    }
}
