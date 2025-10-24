using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class STARTB : MonoBehaviour
{
    private Button startButton;

    private void Awake()
    {
        // Get the UI document and root
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Get your button from UI Builder (make sure the name matches exactly)
        startButton = root.Q<Button>("StartButton"); // this matches your #StartButton in UXML

        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }
        else
        {
            Debug.LogError("StartButton not found! Check the button name in your UXML file.");
        }
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Play button clicked — loading LevelUI...");
        SceneManager.LoadScene("LevelUI");
    }
}
