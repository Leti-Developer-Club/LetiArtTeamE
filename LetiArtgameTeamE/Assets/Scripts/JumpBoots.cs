using UnityEngine;
using System.Collections;

public class JumpBoots : MonoBehaviour
{
    [Header("Jump Boost Settings")]
    public float jumpBoost = 2f;
    public float boostDuration = 5f;
    public GameObject pickupEffect;

    private const float baseJumpForce = 5f;

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
            movement.StartCoroutine(ApplyJumpBoost(movement));
        }

        Destroy(gameObject);
    }

    IEnumerator ApplyJumpBoost(PlayerMovement movement)
    {
        // 🔹 Show timer on UI
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null)
            ui.ShowPowerUp("Jump Boots", boostDuration);

        movement.jumpForce = baseJumpForce + jumpBoost;
        Debug.Log("Jump Boost Activated! New jump force: " + movement.jumpForce);

        yield return new WaitForSeconds(boostDuration);

        if (movement != null)
        {
            movement.jumpForce = baseJumpForce;
            Debug.Log("Jump Boost Ended. Jump force reset to: " + movement.jumpForce);
        }

        // 🔹 Hide timer when done
        if (ui != null)
            ui.HidePowerUp();
    }
}
