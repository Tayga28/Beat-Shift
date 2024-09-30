using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{   
    // The array that takes in all the platform types which gets randomised later.
    [Header("Section Types")]
    public GameObject[] levelSection;

    [Header("Positional Values / Offsets")]
    public float xPos;
    public float yPos;
    public float zPos = 50;
    private Vector3 desiredPosition;

    private int counter;
    public bool spawningPlatform = false;
    public int secNum;

    void Start()
    {
        zPos = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningPlatform == false)
        {
            spawningPlatform = true;
            StartCoroutine(SpawnPlatform());
        }
    }

    IEnumerator SpawnPlatform()
    {
        //secNum = Random.Range(0, 3);
        secNum = 0; 
        //Vector3 offset;
        //Instantiate(levelSection[secNum], new Vector3(0, 0, zOffset), Quaternion.identity);

        if (secNum == 0)
        {
            xPos = Random.Range(-5, 5);
            desiredPosition = new Vector3(xPos, yPos, zPos);
            yPos -= Random.Range(0.2f, 2f);
            zPos += 60;
        }

        Instantiate(levelSection[secNum], desiredPosition, Quaternion.identity);
        counter++;
        Debug.Log(counter);
        yield return new WaitForSeconds(1f);
        spawningPlatform = false;
    }
}
