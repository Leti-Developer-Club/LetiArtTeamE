using UnityEngine;
using System.Collections;

public class SpeedBurst : MonoBehaviour
{
    [Header("Speed Burst Settings")]
    public float speedMultiplier = 2f;
    public float boostDuration = 5f;

    [Header("Effects")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;
    public float volume = 1f;

    private AudioSource audioSource;
    private const float baseSpeed = 6.5f;

    private static bool isBoostActive = false;
    private static Coroutine activeBoostCoroutine;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, transform.rotation);

            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound, volume);

            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                if (isBoostActive && activeBoostCoroutine != null)
                    movement.StopCoroutine(activeBoostCoroutine);

                activeBoostCoroutine = movement.StartCoroutine(ApplySpeedBoost(movement));
            }

            Destroy(gameObject);
        }
    }

    IEnumerator ApplySpeedBoost(PlayerMovement movement)
    {
        isBoostActive = true;

        // 🔹 Show timer on UI
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null)
            ui.ShowPowerUp("Speed Burst", boostDuration);

        // Apply boost
        movement.speed = baseSpeed * speedMultiplier;
        Debug.Log("Speed burst activated! Speed = " + movement.speed);

        yield return new WaitForSeconds(boostDuration);

        // Reset to default
        movement.speed = baseSpeed;
        isBoostActive = false;

        Debug.Log("Speed burst ended. Speed reset to " + movement.speed);

        // 🔹 Hide timer when done
        if (ui != null)
            ui.HidePowerUp();
    }
}
