using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject jumpBootsPrefab;
    public GameObject speedBurstPrefab; // 👈 NEW Power-up

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();

        SpawnObstacle();
        SpawnCoins();
        SpawnJumpBoots();
        SpawnSpeedBurst(); // 👈 NEW
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    // ----------------- SPAWN METHODS -----------------

    void SpawnObstacle()
    {
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoins()
    {
        int coinsToSpawn = 10;
        for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
    }

    void SpawnJumpBoots()
    {
        // 👇 25% chance to spawn JumpBoots
        if (Random.value < 0.25f && jumpBootsPrefab != null)
        {
            Vector3 spawnPos = GetRandomPointInCollider(GetComponent<Collider>());
            spawnPos.y = 1f;
            Instantiate(jumpBootsPrefab, spawnPos, Quaternion.identity, transform);
        }
    }

    void SpawnSpeedBurst()
    {
        // 👇 25% chance to spawn SpeedBurst
        if (Random.value < 0.25f && speedBurstPrefab != null)
        {
            Vector3 spawnPos = GetRandomPointInCollider(GetComponent<Collider>());
            spawnPos.y = 1f;
            Instantiate(speedBurstPrefab, spawnPos, Quaternion.identity, transform);
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );

        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 1;
        return point;
    }
}


