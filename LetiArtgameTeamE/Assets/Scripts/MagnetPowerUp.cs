using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 6f;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activate coin magnet on player
            CoinMagnet magnet = other.GetComponent<CoinMagnet>();
            if (magnet != null)
                magnet.ActivateMagnet(duration);

            // Play sound at the power-up's position
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

            // Destroy power-up instantly
            Destroy(gameObject);
        }
    }
}
