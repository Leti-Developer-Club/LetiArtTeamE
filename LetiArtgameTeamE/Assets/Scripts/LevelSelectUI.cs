using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectUI : MonoBehaviour
{
    private Button level1Button;
    private Button level2Button;
    private Button level3Button;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find buttons by their names from UI Builder
        level1Button = root.Q<Button>("Level1Button");
        level2Button = root.Q<Button>("Level2Button");
        level3Button = root.Q<Button>("Level3Button");

        // Add event listeners
        if (level1Button != null) level1Button.clicked += () => LoadLevel("Level1");
        if (level2Button != null) level2Button.clicked += () => LoadLevel("Level2");
        if (level3Button != null) level3Button.clicked += () => LoadLevel("Level3");
    }

    private void LoadLevel(string sceneName)
    {
        Debug.Log($"🎮 Loading {sceneName}...");
        SceneManager.LoadScene(sceneName);
    }
}
