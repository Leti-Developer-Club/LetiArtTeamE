using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLevel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject player;

    [Header("Buttons")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    private bool levelWon = false;

    private void Start()
    {
        // Hook up button listeners (if assigned)
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

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
            winUI.SetActive(true);

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
                movementScript.enabled = false;
        }

        Time.timeScale = 0f; // Pause game for win UI
        Debug.Log("✅ Level Completed!");
    }

    // ✅ Next Level button
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // ✅ Main Menu button
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
