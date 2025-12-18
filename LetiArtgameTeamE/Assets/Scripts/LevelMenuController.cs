using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    private void OnEnable()
    {
        // Get the UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Get the Tutorial button by name
        Button tutorialButton = root.Q<Button>("tutorial");

        // Safety check
        if (tutorialButton != null)
        {
            tutorialButton.clicked += LoadTutorialScene;
        }
        else
        {
            Debug.LogError("Tutorial button not found. Check the name in UXML.");
        }
    }

    private void LoadTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
