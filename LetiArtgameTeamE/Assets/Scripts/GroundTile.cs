using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner groundSpawner;

    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject jumpBootsPrefab; // 👈 NEW

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoins();
        SpawnJumpBoots(); // 👈 NEW
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

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
        // 👇 25% chance to spawn JumpBoots on this tile
        if (Random.value < 0.25f)
        {
            Vector3 spawnPos = GetRandomPointInCollider(GetComponent<Collider>());
            spawnPos.y = 1f; // Make sure it sits above the ground
            Instantiate(jumpBootsPrefab, spawnPos, Quaternion.identity, transform);
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
