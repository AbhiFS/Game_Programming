using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The prefab you want to spawn
    public Transform[] spawnPoints; // Array of spawn points where objects will be spawned
    public float spawnInterval = 2f; // Time interval between spawns
    public int maxSpawnCount = 10; // Maximum number of objects to spawn

    private int currentSpawnCount = 0; // Current number of spawned objects
    private float spawnTimer = 0f; // Timer for spawn interval

    void Update()
    {
        // Check if it's time to spawn a new object
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && currentSpawnCount < maxSpawnCount)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }

    void SpawnObject()
    {
        // Check if there are spawn points available
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to SpawnManager.");
            return;
        }

        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the object at the chosen spawn point
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);

        // Increment the spawn count
        currentSpawnCount++;
    }
}
