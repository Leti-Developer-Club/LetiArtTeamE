using UnityEngine;

public class InvinciblePowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>()?.ActivateInvincibility();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Optional: make the power-up spin so it's more visible
        transform.Rotate(0, 100 * Time.deltaTime, 0, Space.World);
    }
}
