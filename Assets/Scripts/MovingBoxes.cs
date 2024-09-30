using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxes : MonoBehaviour
{

    public enum boxTypes
    {
        horizontal, vertical
    }
    public boxTypes currentBoxType;
    private GameObject box;
    private Transform pointA;
    private Transform pointB;
    public float speed;

    public bool atPointA;
    public bool atPointB;

    // Start is called before the first frame update
    void Awake()
    {
        box = GameObject.Find("Box1");
        pointA = GameObject.Find("PointA").transform;
        pointB = GameObject.Find("PointB").transform;
    }

    // Update is called once per frame
    void Update() {
        //float time = Mathf.PingPong(Time.time * speed, 1);
        //box.transform.position = Vector3.Lerp(pointA.position, pointB.position, time);
        
    }

}    
