using UnityEngine;
using System.Collections;

public class SpeedBurst : MonoBehaviour
{
    [Header("Speed Burst Settings")]
    public float speedMultiplier = 2f;      // How much faster the player becomes
    public float boostDuration = 5f;        // How long the effect lasts

    [Header("Effects")]
    public GameObject pickupEffect;         // Optional visual effect
    public AudioClip pickupSound;           // Optional sound effect
    public float volume = 1f;               // Sound volume

    private static bool isBoostActive = false;
    private static Coroutine activeBoostCoroutine;

    private AudioSource audioSource;
    private const float baseSpeed = 6.5f;   // Default player speed

    void Start()
    {
        // Add an AudioSource if there isn't one
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play visual effect
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, transform.rotation);

            // Play sound
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound, volume);

            // Get the PlayerMovement script
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                // Stop any existing boost before applying a new one
                if (isBoostActive && activeBoostCoroutine != null)
                    movement.StopCoroutine(activeBoostCoroutine);

                activeBoostCoroutine = movement.StartCoroutine(ApplySpeedBoost(movement));
            }

            // Destroy the power-up instantly
            Destroy(gameObject);
        }
    }

    IEnumerator ApplySpeedBoost(PlayerMovement movement)
    {
        isBoostActive = true;

        //  Show UI when boost starts
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null)
            ui.ShowPowerUp("Speed Burst", boostDuration);

        // Apply boost
        float originalSpeed = movement.speed;
        movement.speed = baseSpeed * speedMultiplier;
        Debug.Log("⚡ Speed burst activated! Speed = " + movement.speed);

        // Wait for the boost duration
        yield return new WaitForSeconds(boostDuration);

        // Reset to default
        movement.speed = baseSpeed;
        isBoostActive = false;
        Debug.Log(" Speed burst ended. Speed reset to " + movement.speed);

        //  Hide UI when finished
        if (ui != null)
            ui.HidePowerUp();
    }
}
