using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                if (playerMovement.isInvincible)
                {
                    // Destroy the obstacle instead of killing the player
                    Destroy(gameObject);
                }
                else
                {
                    // Kill the player
                    playerMovement.Die();
                }
            }
        }
    }
}

