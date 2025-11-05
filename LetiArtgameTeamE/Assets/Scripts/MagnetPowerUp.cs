using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float duration = 6f;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    public float volume = 1f;

    [Header("Visuals")]
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔹 Activate magnet behavior on player
            CoinMagnet magnet = other.GetComponent<CoinMagnet>();
            if (magnet != null)
            {
                magnet.ActivateMagnet(duration);
            }

            // 🔹 Show countdown timer on UI
            PowerUpUI ui = FindObjectOfType<PowerUpUI>();
            if (ui != null)
            {
                ui.ShowPowerUp("Coin Magnet", duration);
            }

            // 🔹 Play pickup VFX
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, transform.rotation);

            // 🔹 Play pickup sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

            // 🔹 Destroy the power-up object
            Destroy(gameObject);
        }
    }
}
