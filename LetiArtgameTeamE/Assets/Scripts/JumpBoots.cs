using UnityEngine;
using System.Collections;

public class JumpBoots : MonoBehaviour
{
    [Header("Jump Boost Settings")]
    public float jumpBoost = 2f;        // Extra jump force amount
    public float boostDuration = 5f;    // Duration of boost
    public GameObject pickupEffect;     // Optional pickup VFX

    private const float baseJumpForce = 5f; // Normal jump force

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        // Play pickup VFX
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, transform.rotation);

        // Apply boost
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.StartCoroutine(ApplyJumpBoost(movement));
        }

        // Destroy the power-up object
        Destroy(gameObject);
    }

    IEnumerator ApplyJumpBoost(PlayerMovement movement)
    {
        // Boost jump
        movement.jumpForce = baseJumpForce + jumpBoost;
        Debug.Log("Jump Boost Activated! New jump force: " + movement.jumpForce);

        // Wait for duration
        yield return new WaitForSeconds(boostDuration);

        // Reset to normal
        if (movement != null)
        {
            movement.jumpForce = baseJumpForce;
            Debug.Log("Jump Boost Ended. Jump force reset to: " + movement.jumpForce);
        }
    }
}
