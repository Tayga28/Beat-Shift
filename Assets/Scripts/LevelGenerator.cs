using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{

    // Amount that checks is player is close enough to spawn the next platforms.
    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;
    // Amount that checks is player is close enough to despawn the previous platforms.
    private const float PLAYER_DISTANCE_DELETE_LEVEL_PART = 200f;

    [Header("Level Technicalities")]
    [SerializeField] private Transform levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private List<Transform> easyLevelPartList;
    [SerializeField] private List<Transform> normalLevelPartList;
    [SerializeField] private List<Transform> hardLevelPartList;
    [SerializeField] private List<Transform> extremeLevelPartList;
    [SerializeField] private List<Transform> impossibleLevelPartList;
    [SerializeField] private MovePlayer player;
    private List<GameObject> levelListForDespawning = new List<GameObject>();

    private enum Difficulty 
    {
        Easy, Normal, Hard, Extreme, Impossible
    }

    [Header("Positional Values / Offsets")]
    public float xPos = 0;
    public float yPos = -1;
    public float zPos = 35;

    [Header("References To Other Scripts")] // To change stuff like speed (as the game progresses)
    public ExpandingBox expandScript;
    public MovingBoxes movingScript;
    public RotatingBox rotatingScript;

    private bool isStart;
    private Vector3 lastEndPosition;
    private Vector3 firstPlatformPosition;
    private bool isSpawningSimilarPart;
    int chosenLevelPartIndex;
    int lastLevelIndex;

    public int levelPartsSpawned;

    private void Awake() 
    {
        isSpawningSimilarPart = false;

        lastEndPosition = levelPart_Start.Find("EndPosition").position;
        firstPlatformPosition = levelPart_Start.Find("StartPlatform").position;
        levelListForDespawning.Add(levelPart_Start.gameObject);

        int startingSpawnLevelParts = 5;
        isStart = true;
        for (int i = 0; i < startingSpawnLevelParts; i++) {
            SpawnLevelPart();
        }
    }


    private void Update() 
    {
        Debug.Log(firstPlatformPosition);
        //Intensify();
        if (Vector3.Distance(player.transform.position, lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART) 
        {
            SpawnLevelPart();
        }

        if (Vector3.Distance(player.transform.position, firstPlatformPosition) > PLAYER_DISTANCE_DELETE_LEVEL_PART) 
        {
            DespawnLevelPart(levelListForDespawning[0].gameObject);
        }
    }

    private void SpawnLevelPart() 
    {
        // Parental List of Level Fragments: Fragments sorted by difficulty will be added to the no matter what happens.
        List<Transform> difficultyLevelPartList;

        // Whatever difficulty it is, the level parts of that difficulty will be transferred to this universal parental list.
        switch(GetDifficulty())
        {
            default:
            case Difficulty.Easy:       difficultyLevelPartList = easyLevelPartList;        break;
            case Difficulty.Normal:     difficultyLevelPartList = normalLevelPartList;      break;
            case Difficulty.Hard:       difficultyLevelPartList = hardLevelPartList;        break;
            case Difficulty.Extreme:    difficultyLevelPartList = extremeLevelPartList;     break;
            case Difficulty.Impossible: difficultyLevelPartList = impossibleLevelPartList;  break;
        }

        // Always have the first level fragment the same.
        if(isStart == true)
        {
            chosenLevelPartIndex = 0;
            isStart = false;
        } else {
            do {
                // Random level spawns if it isn't the beginning of the game
                chosenLevelPartIndex = Random.Range(0, levelPartList.Count);
            }
            while(chosenLevelPartIndex == lastLevelIndex);
        }

        //Set chosen fragment to last fragment so we don't get the same level twice
        lastLevelIndex = chosenLevelPartIndex;

        // Get the position of the chosen level
        Transform chosenLevelPart = levelPartList[chosenLevelPartIndex];
        Vector3 offset = new Vector3(xPos, yPos, zPos);

        // Spawn the new level part and note its transform
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition + offset);

        // Add it to the despawn list
        levelListForDespawning.Add(lastLevelPartTransform.gameObject);

        // Set a new end position for the upcoming level part to follow
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
        firstPlatformPosition = levelListForDespawning[0].transform.position;
        levelPartsSpawned++;

        lastLevelIndex = chosenLevelPartIndex;   
    }


    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) 
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, new Quaternion(11.25f, 0f, 0f, 180f));
        return levelPartTransform;
    }


    private void DespawnLevelPart(GameObject despawnPart)
    {
        Destroy(despawnPart);
        levelListForDespawning.RemoveAt(0);
        firstPlatformPosition = levelListForDespawning[0].transform.position;
    }

    private Difficulty GetDifficulty()
    {
        if(levelPartsSpawned >= 75) return Difficulty.Impossible; 
        if(levelPartsSpawned >= 50) return Difficulty.Extreme; 
        if(levelPartsSpawned >= 30) return Difficulty.Hard; 
        if(levelPartsSpawned >= 10) return Difficulty.Normal; 
        return Difficulty.Easy; 
    }

    void Intensify()
    {
        if (GetDifficulty() == Difficulty.Easy)
        {
            Debug.Log("Works and tested, easy");
        }
        if (GetDifficulty() == Difficulty.Normal)
        {
            Debug.Log("Works and tested, normal");
        }
        if (GetDifficulty() == Difficulty.Hard)
        {
            Debug.Log("Works and tested, hard");
        }
        if (GetDifficulty() == Difficulty.Extreme)
        {
            Debug.Log("Works and tested, extreme");
        }
        if (GetDifficulty() == Difficulty.Impossible)
        {
            Debug.Log("Works and tested, sheesh, impossible");
        }
    }

}

  /*
        if (chosenLevelPartIndex == 0) // The Tunnel
        {
            isSpawningSimilarPart = true;  
        }
        else if (chosenLevelPartIndex == 1) // Block-Box HARD
        {
            isSpawningSimilarPart = true;  
        }
        else if (chosenLevelPartIndex == 2) // Block-Box Easy
        {
            isSpawningSimilarPart = true;            
        } 
        else if (chosenLevelPartIndex == 3) // MiniMaze Left
        {
            isSpawningSimilarPart = false;
        }  
        else if (chosenLevelPartIndex == 4) // MiniMaze Right
        {
            isSpawningSimilarPart = true;
        } 
        else if (chosenLevelPartIndex == 5) // Slim Pathways
        {
            isSpawningSimilarPart = false;
        }*/