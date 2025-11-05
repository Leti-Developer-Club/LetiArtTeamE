using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    // Shared counter across all tiles
    private static int tileCount = 0;

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

    [Header("Power-Up Settings")]
    [SerializeField] private GameObject jumpingBotsPrefab;
    [SerializeField] private GameObject randomBuffPrefab;
    [SerializeField] private GameObject magnetPrefab;
    [SerializeField] private GameObject skipPrefab;
    [SerializeField] private GameObject speedBurstPrefab;
    [SerializeField] private GameObject invinciblePrefab;
    [SerializeField] private float powerUPSpawnChance = 0.09f;

    private void Start()
    {
        // ✅ Use new Unity API if available
#if UNITY_2023_1_OR_NEWER
        groundSpawner = Object.FindFirstObjectByType<GroundSpawner>();
#else
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
#endif

        tileCount++;

        // ✅ Only start spawning from the 4th tile onward
        if (tileCount > 3)
        {
            SpawnObstacle();
            SpawnCoins();
            SpawnPowerUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    // ✅ Add this function so GroundSpawner can reset it
    public static void ResetTileCount()
    {
        tileCount = 0;
    }

    void SpawnObstacle()
    {
        GameObject objectToSpawn = obstaclePrefab;
        float random = Random.Range(0f, 1f);

        if (random < tallObstacleChance)
            objectToSpawn = tallObstaclePrefab;
        else if (random > 1 - rampChance)
            objectToSpawn = rampPrefab;

        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinsToSpawn; i++)
        {
            int lane = Random.Range(0, 3);
            float laneX = (lane - 1) * laneDistance;
            Vector3 coinPos = transform.position + new Vector3(laneX, 1f, i * 2f);
            Instantiate(coinPrefab, coinPos, Quaternion.identity, transform);
        }
    }

    void SpawnPowerUp()
    {
        float randomChance = Random.Range(0f, 1f);
        if (randomChance > powerUPSpawnChance) return;

        int randomIndex = Random.Range(0, 6);
        GameObject powerUpToSpawn = null;

        switch (randomIndex)
        {
            case 0: powerUpToSpawn = jumpingBotsPrefab; break;
            case 1: powerUpToSpawn = randomBuffPrefab; break;
            case 2: powerUpToSpawn = magnetPrefab; break;
            case 3: powerUpToSpawn = skipPrefab; break;
            case 4: powerUpToSpawn = speedBurstPrefab; break;
            case 5: powerUpToSpawn = invinciblePrefab; break;
        }

        int lane = Random.Range(0, 3);
        float laneX = (lane - 1) * laneDistance;
        float offsetZ = Random.Range(5f, 15f);

        Vector3 spawnPos = transform.position + new Vector3(laneX, 1f, offsetZ);
        Instantiate(powerUpToSpawn, spawnPos, Quaternion.identity, transform);
    }
}
