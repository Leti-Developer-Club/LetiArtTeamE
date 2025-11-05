using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    private Vector3 nextSpawnPoint;

    public void SpawnTile()
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

    private void Start()
    {
        // ✅ Reset the shared counter before spawning new tiles
        GroundTile.ResetTileCount();

        // Spawn starting tiles
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }

}
