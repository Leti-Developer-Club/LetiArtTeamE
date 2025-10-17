using UnityEngine;
using System.Collections;

public class SpeedBurst : MonoBehaviour
{
    public float speedMultiplier = 2f;      // How much faster the player becomes
    public float boostDuration = 5f;        // How long the effect lasts
    public GameObject pickupEffect;         // Optional visual effect (particle, etc.)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        // Spawn pickup effect
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, transform.rotation);

        // Get the player movement component
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            StartCoroutine(ApplySpeedBoost(movement));
        }

        // Destroy the power-up object
        Destroy(gameObject);
    }

    IEnumerator ApplySpeedBoost(PlayerMovement movement)
    {
        float originalSpeed = movement.speed;
        float originalHorizontalMultiplier = movement.horizontalMultiplier;

        // Apply the boost
        movement.speed *= speedMultiplier;
        movement.horizontalMultiplier *= speedMultiplier;

        Debug.Log("Speed burst activated! Speed = " + movement.speed);

        // Wait for the duration
        yield return new WaitForSeconds(boostDuration);

        // Reset to normal
        movement.speed = originalSpeed;
        movement.horizontalMultiplier = originalHorizontalMultiplier;

        Debug.Log("Speed burst expired. Speed reset to " + movement.speed);
    }
}

