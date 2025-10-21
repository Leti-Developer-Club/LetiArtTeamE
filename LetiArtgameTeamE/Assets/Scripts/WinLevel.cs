using UnityEngine;

public class WinLevel : MonoBehaviour
{
    [SerializeField] GameObject winUI; 
    [SerializeField] GameObject player;

    private bool levelwon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelwon) return;

        if (other.CompareTag("Player"))
        {
            levelwon = true;
            Win();
        }
    }

    void Win()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }

            var movementScript = player.GetComponent<MonoBehaviour>();
            if (movementScript != null)
            {
                movementScript.enabled = false;
            }
        }

        Debug.Log("Level Completed!");
    }
}
