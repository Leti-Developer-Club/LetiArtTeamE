using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject player;
    [SerializeField] private float nextLevelDelay = 10f; // Delay before next level

    private bool levelWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelWon) return;

        if (other.CompareTag("Player"))
        {
            levelWon = true;
            Win();
        }
    }

    private void Win()
    {
        // ✅ Show the win UI
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        // ✅ Stop player movement and physics
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = false;
            }
        }

        Debug.Log("✅ Level Completed!");

        // ✅ Delay load of the next level
        Invoke(nameof(LoadNextLevel), nextLevelDelay);
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f; // Unpause before loading
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // ✅ Make sure the next scene exists
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // If no next level, go back to Main Menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
