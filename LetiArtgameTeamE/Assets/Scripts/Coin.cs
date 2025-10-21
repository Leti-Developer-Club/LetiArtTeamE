using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;

    [Header("Audio Settings")]
    public AudioClip collectSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.name != "Player")
            return;

        // Add to the player's score
        GameManager.inst.IncrementScore();

        // Play sound at the coin's position and destroy the coin instantly
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
