using UnityEngine;
using System.Collections;

public class InvinciblePowerUp : MonoBehaviour
{
    [Header("Invincibility Settings")]
    public float duration = 5f; // How long the player stays invincible

    [Header("Effects")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Start invincibility on player
                player.StartCoroutine(ActivateInvincibility(player));
            }

            // Spawn pickup VFX
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, transform.rotation);

            // Play sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

            // Destroy power-up object
            Destroy(gameObject);
        }
    }

    private IEnumerator ActivateInvincibility(PlayerMovement player)
    {
        // 🔹 Show timer on UI
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null)
            ui.ShowPowerUp("Invincibility", duration);

        // 🔹 Activate invincibility
        player.isInvincible = true;
        Debug.Log("Invincibility activated!");

        // Optional: change player color or glow to show effect
        Renderer rend = player.GetComponentInChildren<Renderer>();
        Color originalColor = Color.white;
        if (rend != null)
        {
            originalColor = rend.material.color;
            rend.material.color = Color.yellow;
        }

        // Wait until timer runs out
        yield return new WaitForSeconds(duration);

        // 🔹 Disable invincibility
        player.isInvincible = false;
        Debug.Log("Invincibility ended!");

        // Restore player color
        if (rend != null)
            rend.material.color = originalColor;

        // 🔹 Hide the UI
        if (ui != null)
            ui.HidePowerUp();
    }

    private void Update()
    {
        // Optional: make the power-up spin for visibility
        transform.Rotate(0, 100 * Time.deltaTime, 0, Space.World);
    }
}
