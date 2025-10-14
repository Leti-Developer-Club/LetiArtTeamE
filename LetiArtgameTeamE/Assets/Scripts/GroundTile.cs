using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject tallObstaclePrefab;
    [SerializeField] private float tallObstacleChance = 0.25f;
    [SerializeField] private GameObject rampPrefab;
    [SerializeField] private float rampChance = 0.25f;

    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinsToSpawn = 10;
    [SerializeField] private float laneDistance = 2.5f; 

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoins();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    
    void SpawnObstacle()
    {
        // Choose which obstacle to spawn
        GameObject objectToSpawn = obstaclePrefab;
        float random = Random.Range(0f, 1f);

        if (random < tallObstacleChance)
            objectToSpawn = tallObstaclePrefab;
        else if (random > 1 - rampChance)
            objectToSpawn = rampPrefab;

        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        // Spawn the obstacle at that position
        Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity, transform);
    }

    
    void SpawnCoins()
    {
        for (int i = 0; i < coinsToSpawn; i++)
        {
            // Pick a random lane (0 = left, 1 = middle, 2 = right)
            int lane = Random.Range(0, 3);
            float laneX = (lane - 1) * laneDistance;

            // Position coin slightly above ground and spaced forward
            Vector3 coinPos = transform.position + new Vector3(laneX, 1f, i * 2f);

            Instantiate(coinPrefab, coinPos, Quaternion.identity, transform);
        }
    }

    
}
