using UnityEngine;

public class TideSpawnAsset : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    public GameObject enemyPrefab;

    [Header("Spawn Weights")]
    [SerializeField] private int coinWeight = 45;
    [SerializeField] private int enemyWeight = 50;
    [SerializeField] private int gemWeight = 10;

    GameObject reference;

    public Tide tide;

    [SerializeField] GameObject destroyThis;

    private void Start()
    {
        tide.currentObjectCount++;
    }

    float timer = 0.625f;
    bool doOnce = false;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        else
        {
            if (!doOnce)
            {
                doOnce = true;
                SpawnRandomObject();
                Destroy(destroyThis);
            }
        }
        

        if (reference == null) { tide.currentObjectCount--; Destroy(gameObject); }
    }

    //Spawns a random tide objects
    private void SpawnRandomObject()
    {
        //Get the random object to spawn
        GameObject prefabToSpawn = GetWeightedRandomPrefab();

        //Checks if the prefab is null
        if (prefabToSpawn == null)
        {
            Debug.LogError("Tide: prefabToSpawn is null before instantiate");
            return;
        }

        //Get a random spawn position within spawn area and spawn tide object
     //   Vector3 spawnPos = GetRandomSpawnPosition();
        reference = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        //Adds the spawned object to the spawned objects list
       // spawnedObjects.Add(spawned);

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
}
