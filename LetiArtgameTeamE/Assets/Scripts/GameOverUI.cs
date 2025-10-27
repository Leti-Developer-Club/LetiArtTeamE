using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private Button restartButton;
    private Button mainMenuButton;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        restartButton = root.Q<Button>("RestartButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");

        restartButton.clicked += RestartGame;
        mainMenuButton.clicked += GoToMainMenu;
    }

    private void RestartGame()
    {
        // ✅ Reload the last played level
        SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel", "Level1"));
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

