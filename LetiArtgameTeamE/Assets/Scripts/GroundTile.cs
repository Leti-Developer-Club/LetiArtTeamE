using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] private float powerUPSpawnChance = 0.09f;

    [System.Serializable]
    public class LevelPowerUpRule
    {
        public GameObject powerUpPrefab;
        public int minLevel = 1;
        public int maxLevel = 99;
    }

    [Header("Level Based PowerUps")]
    public LevelPowerUpRule[] levelPowerUps;

    private int currentLevel;

    private void Start()
    {
        // Detect level number automatically from scene name "Level1", "Level2", etc.
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level"))
        {
            string levelNumber = sceneName.Replace("Level", "");
            int.TryParse(levelNumber, out currentLevel);
        }
        else
        {
            currentLevel = 1;
        }

#if UNITY_2023_1_OR_NEWER
        groundSpawner = Object.FindFirstObjectByType<GroundSpawner>();
#else
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
#endif

        tileCount++;

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
        float chance = Random.Range(0f, 1f);
        if (chance > powerUPSpawnChance)
            return;

        List<GameObject> validPowerUps = new List<GameObject>();

        foreach (var rule in levelPowerUps)
        {
            if (currentLevel >= rule.minLevel && currentLevel <= rule.maxLevel)
            {
                validPowerUps.Add(rule.powerUpPrefab);
            }
        }

        if (validPowerUps.Count == 0)
            return;

        GameObject selectedPowerUp = validPowerUps[Random.Range(0, validPowerUps.Count)];

        int lane = Random.Range(0, 3);
        float laneX = (lane - 1) * laneDistance;
        float offsetZ = Random.Range(5f, 15f);

        Vector3 spawnPos = transform.position + new Vector3(laneX, 1f, offsetZ);

        Instantiate(selectedPowerUp, spawnPos, Quaternion.identity, transform);
    }
}
