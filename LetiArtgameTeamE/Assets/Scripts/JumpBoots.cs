using UnityEngine;
using System.Collections;

public class JumpBoots : MonoBehaviour
{
    [Header("Jump Boost Settings")]
    public float jumpBoost = 2f;        // Extra jump force amount
    public float boostDuration = 5f;    // Duration of boost
    public GameObject pickupEffect;     // Optional pickup VFX
    public AudioClip pickupSound;       // Optional pickup sound
    public float volume = 1f;           // Sound volume

    private const float baseJumpForce = 5f; // Normal jump force
    private AudioSource audioSource;
    private static bool isJumpBoostActive = false;
    private static Coroutine activeJumpCoroutine;

    void Start()
    {
        // Add AudioSource if not present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Visual effect
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, transform.rotation);

            // Sound effect
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound, volume);

            // Apply boost
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                if (isJumpBoostActive && activeJumpCoroutine != null)
                    movement.StopCoroutine(activeJumpCoroutine);

                activeJumpCoroutine = movement.StartCoroutine(ApplyJumpBoost(movement));
            }

            // Destroy power-up
            Destroy(gameObject);
        }
    }

    IEnumerator ApplyJumpBoost(PlayerMovement movement)
    {
        isJumpBoostActive = true;

        // 🟢 Show power-up timer in UI
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null)
            ui.ShowPowerUp("Jump Boots", boostDuration);

        // Apply boost
        float originalJumpForce = movement.jumpForce;
        movement.jumpForce = baseJumpForce + jumpBoost;
        Debug.Log("🟢 Jump Boost Activated! New jump force: " + movement.jumpForce);

        // Wait for the duration
        yield return new WaitForSeconds(boostDuration);

        // Reset jump force
        movement.jumpForce = baseJumpForce;
        isJumpBoostActive = false;
        Debug.Log("🔴 Jump Boost Ended. Jump force reset to: " + movement.jumpForce);

        // 🔴 Hide UI when boost ends
        if (ui != null)
            ui.HidePowerUp();
    }
}
