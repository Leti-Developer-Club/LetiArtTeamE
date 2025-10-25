using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private Button restartButton;
    private Button mainMenuButton;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        restartButton = root.Q<Button>("RestartButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");

        if (restartButton != null)
            restartButton.clicked += RestartLevel;

        if (mainMenuButton != null)
            mainMenuButton.clicked += ReturnToMainMenu;
    }

    private void RestartLevel()
    {
        Debug.Log("🔁 Restarting current level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("🏠 Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }
}

