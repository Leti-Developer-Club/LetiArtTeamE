using UnityEngine;

public class SkipPowerUp : MonoBehaviour
{
    public float jumpForce = 12f;
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, transform.rotation);

                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            Destroy(gameObject);
        }
    }
}
