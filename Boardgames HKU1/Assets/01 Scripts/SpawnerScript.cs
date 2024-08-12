using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array to hold the prefabs to spawn
    public Terrain terrain; // Reference to the terrain
    [SerializeField] private int numberOfObjects = 10; // Number of objects to spawn
    public float yOffset = 1.0f; // Offset to place the object above the terrain

    private TerrainData terrainData;
    private float terrainWidth;
    private float terrainLength;

    void Start()
    {
        terrainData = terrain.terrainData;
        terrainWidth = terrainData.size.x;
        terrainLength = terrainData.size.z;
        SpawnObjects();
        InvokeRepeating("SpawnObjects", 30, 30);
    }

    
    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Get a random position on the terrain
            Vector3 randomPosition = GetRandomPositionOnTerrain();
            // Get a random object from the array
            GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            // Instantiate the object at the random position
            Instantiate(randomObject, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        // Get random x and z positions within the terrain bounds
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get the terrain height at the random x and z positions
        float y = GetHeightAtPosition(randomX, randomZ) + yOffset;

        // Return the random position
        return new Vector3(randomX, y, randomZ);
    }

    float GetHeightAtPosition(float x, float z)
    {
        // Get the normalized position on the terrain (0-1 range)
        float normalizedX = x / terrainWidth;
        float normalizedZ = z / terrainLength;

        // Get the height at the normalized position
        float height = terrainData.GetHeight(
            Mathf.RoundToInt(normalizedX * terrainData.heightmapResolution),
            Mathf.RoundToInt(normalizedZ * terrainData.heightmapResolution)
        );

        return height;
    }
}
