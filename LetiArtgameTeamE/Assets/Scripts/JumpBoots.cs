using UnityEngine;
using System.Collections;

public class JumpBoots : MonoBehaviour
{
    public float jumpBoost = 2f;            // Jump multiplier
    public float boostDuration = 5f;        // Duration of boost
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, transform.rotation);

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            StartCoroutine(ApplyJumpBoost(movement));
        }

        Destroy(gameObject);
    }

    IEnumerator ApplyJumpBoost(PlayerMovement movement)
    {
        float originalJumpForce = movement.jumpForce;
        movement.jumpForce *= jumpBoost;

        yield return new WaitForSeconds(boostDuration);

        movement.jumpForce = originalJumpForce;
    }
}

