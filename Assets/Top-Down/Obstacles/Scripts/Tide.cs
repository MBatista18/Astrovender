using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tide : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    public GameObject enemyPrefab;

    [Header("Tide Movement")]
    [SerializeField] private float lowY = 0f;
    [SerializeField] private float highY = 5.0f;
    [SerializeField] private float riseSpeed = 1.5f;
    [SerializeField] private float fallSpeed = 1.5f;
    [SerializeField] private float pauseAtTop = 3.0f;
    [SerializeField] private float pauseAtBottom = 6.0f;

    [Header("Spawn Amount Per High Tide")]
    [SerializeField] private int minSpawnCount = 1;
    [SerializeField] private int maxSpawnCount = 4;
    [SerializeField] private int maxActiveSpawnedObjects = 20;

    [Header("Spawn Weights")]
    [SerializeField] private int coinWeight = 45;
    [SerializeField] private int enemyWeight = 50;
    [SerializeField] private int gemWeight = 10;

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private bool rising = true;
    private bool paused = false;
    private float pauseTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Getting the object's position and replacing the y with lowY
        Vector3 pos = transform.position;
        pos.y = lowY;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        //Cleans the spawned objects list then starts the next tide cycle
        CleanupSpawnList();
        HandleTideMovement();
    }

    //Controls the tide movement
    private void HandleTideMovement()
    {
        Vector3 pos = transform.position;

        //If the tide is paused, start paused timer, else pause if false and flip rising boolean
        if (paused)
        {
            pauseTimer -= Time.deltaTime;

            if (pauseTimer <= 0f)
            {
                paused = false;
                rising = !rising;
            }

            return;
        }

        //If the tide is rising, move toward high position, spawn tide objects, and pause the tide
        if (rising)
        {
            pos.y = Mathf.MoveTowards(pos.y, highY, riseSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.y, highY))
            {
                SpawnTideObjects();

                paused = true;
                pauseTimer = pauseAtTop;
            }
        }
        //If the tide is falling, move toward low position and pause the tide
        else
        {
            pos.y = Mathf.MoveTowards(pos.y, lowY, fallSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.y, lowY))
            {
                paused = true;
                pauseTimer = pauseAtBottom;
            }
        }
    }

    //Spawns tide objects
    private void SpawnTideObjects()
    {
        //If active spawned tide objects are greater than or equal to the max, no objects are spawned
        if (spawnedObjects.Count >= maxActiveSpawnedObjects)
        {
            Debug.Log("Tide: Max active spawned objects reached.");
            return;
        }

        //Random number roll to determine how many objects are spawned per tide cycle
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for(int i = 0; i < spawnCount; i++)
        {
            SpawnRandomObject();
        }
    }

    //Spawns a random tide objects
    private void SpawnRandomObject()
    {
        //Get the random object to spawn
        GameObject prefabToSpawn = GetWeightedRandomPrefab();

        //Checks if the prefab is null
        if (prefabToSpawn == null )
        {
            Debug.LogError("Tide: prefabToSpawn is null before instantiate");
            return;
        }

        //Get a random spawn position within spawn area and spawn tide object
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        //Adds the spawned object to the spawned objects list
        spawnedObjects.Add(spawned);

    }

    //Get a random tide objected based off of spawn weights(chance to spawn)
    private GameObject GetWeightedRandomPrefab()
    {
        //Get total weight
        int totalWeight = coinWeight + enemyWeight + gemWeight;

        //If total weight is 0 or less, return null
        if (totalWeight <= 0)
        {
            Debug.LogError("Total spawn weight is 0 or less.");
            return null;
        }

        //Gets a random roll
        int roll = Random.Range(0, totalWeight);

        //If the roll falls within the bounds of the object, return that object
        if (roll < coinWeight)
        {
            return coinPrefab;
        }

        roll -= coinWeight;

        if (roll < enemyWeight)
        {
            return enemyPrefab;
        }

        roll -= enemyWeight;

        if (roll < gemWeight)
        {
            return gemPrefab;
        }

        Debug.LogError("Weighted selection returned null unexpectedly.");
        return null;
    }

    //Gets a random spawn position within the bounds of the collision box and returns it
    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, 0f);
    }

    //Removes destroyed spawned objects to allow more to spawn
    private void CleanupSpawnList()
    {
        spawnedObjects.RemoveAll(obj => obj == null);
    }

}
