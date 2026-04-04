using UnityEngine;
using System.Collections;

public class MeteorManager : MonoBehaviour
{
    [Header("Meteor Prefab")]
    [SerializeField] GameObject meteor;
    BoxCollider2D coll;

    [Header("Shower Timing")]
    [SerializeField] private float minTimeBetweenShowers = 7f;
    [SerializeField] private float maxTimeBetweenShowers = 15f;

    [Header("Rocks Per Shower")]
    [SerializeField] private int minRocksPerShower = 3;
    [SerializeField] private int maxRocksPerShower = 7;

    [Header("Delay Between Rocks In Same Shower")]
    [SerializeField] private float minDelayBetweenRocks = 0.1f;
    [SerializeField] private float maxDelayBetweenRocks = 0.35f;
    
    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private Coroutine showerRoutine;

    private void Start()
    {
        showerRoutine = StartCoroutine(RockShowerLoop());
    }

    private IEnumerator RockShowerLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenShowers, maxTimeBetweenShowers);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(SpawnRockShower());
        }
    }

    private IEnumerator SpawnRockShower()
    {
        int rockCount = Random.Range(minRocksPerShower, maxRocksPerShower + 1);

        float radiusX = (coll.size.x - 1) / 2f;
        float radiusY = (coll.size.y - 1) / 2f;

        for (int i = 0; i < rockCount; i++)
        {
            Instantiate(meteor, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);

            float delay = Random.Range(minDelayBetweenRocks, maxDelayBetweenRocks);
            yield return new WaitForSeconds(delay);
        }
    }
}

