using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    [Header("List of Possible Power-Ups")]
    public GameObject[] powerUps;  
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateRandomPowerUp(other);
        }
    }

    void ActivateRandomPowerUp(Collider player)
    {
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, transform.rotation);


        int index = Random.Range(0, powerUps.Length);


        if (powerUps.Length > 0 && powerUps[index] != null)
        {

            Instantiate(powerUps[index], player.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
