using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParts : MonoBehaviour 
{
    // Amount that checks if player is close enough to spawn the next platforms.
    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 150f;
    // Amount that checks if player is close enough to despawn the previous platforms.
    private const float PLAYER_DISTANCE_DELETE_LEVEL_PART = 150f;

    [Header("Level Technicalities")]
    [SerializeField] private Transform levelStart;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private PlayerMovement player;
    private List<GameObject> levelListForDespawning = new List<GameObject>();

    private enum Difficulty 
    {
        Easy, Normal, Hard, Extreme, Impossible
    }

    [Header("Positional Values / Offsets")]
    public float xPos = 0;
    public float yPos = -1;
    public float zPos = 35;
    public float zGapIncreaseRate = 0.5f;

    private Vector3 lastEndPosition;
    private Vector3 furtherestPlatformPosition;
    int chosenLevelPartIndex;

    public int levelPartsSpawned;

    private void Awake() 
    {
        levelPartsSpawned = 0;
        lastEndPosition = levelStart.Find("EndPosition").position;
        furtherestPlatformPosition = levelStart.Find("StartPlatform").position;
        levelListForDespawning.Add(levelStart.gameObject);

        int startingSpawnLevelParts = 5;
        for (int i = 0; i < startingSpawnLevelParts; i++) {
            SpawnLevelPart();
        }
    }

    private void Update() 
    {
        if (Vector3.Distance(player.transform.position, lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART) 
        {
            SpawnLevelPart();
        }

        if (Vector3.Distance(player.transform.position, furtherestPlatformPosition) > PLAYER_DISTANCE_DELETE_LEVEL_PART) 
        {
            DespawnLevelPart(levelListForDespawning[0].gameObject);
        }
    }

    private void SpawnLevelPart() 
    {
        int chosenLevelPartIndex = Random.Range(0, levelPartList.Count); // Choose a part to spawn.
        Transform chosenLevelPart = levelPartList[chosenLevelPartIndex]; // Get the transform of this part.

        Transform newestLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition);    // Spawn it.
        levelPartsSpawned++;
        levelListForDespawning.Add(newestLevelPartTransform.gameObject);                          // Add the newly spawned level part to the end of the despawning list.

        furtherestPlatformPosition = newestLevelPartTransform.Find("StartPlatform").position;
        lastEndPosition = newestLevelPartTransform.Find("EndPosition").position;                  // Set a new endPosition, which determines how close the player needs to be to spawn a new platform.
        furtherestPlatformPosition = levelListForDespawning[0].transform.position;
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) 
    {
        Vector3 offset = new Vector3(xPos, yPos, zPos);
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition + offset, Quaternion.identity);
        return levelPartTransform;
    }

    private void DespawnLevelPart(GameObject despawnPart)
    {
        Destroy(despawnPart);
        levelListForDespawning.RemoveAt(0);
        furtherestPlatformPosition = levelListForDespawning[0].transform.position;
    }
}
