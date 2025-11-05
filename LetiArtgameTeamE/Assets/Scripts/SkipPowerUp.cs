using UnityEngine;

public class SkipPowerUp : MonoBehaviour
{
    [Header("Skip Power-Up Settings")]
    public float jumpForce = 12f;
    public float displayDuration = 3f;  // How long the countdown shows
    public GameObject pickupEffect;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 🔹 Visual effect
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, transform.rotation);

                // 🔹 Play sound at power-up position
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

                // 🔹 Apply skip jump
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

                // 🔹 Show timer UI for short duration
                PowerUpUI ui = FindObjectOfType<PowerUpUI>();
                if (ui != null)
                {
                    ui.ShowPowerUp("Skip Jump", displayDuration);
                }
            }

            // 🔹 Destroy the power-up instantly
            Destroy(gameObject);
        }
    }
}
