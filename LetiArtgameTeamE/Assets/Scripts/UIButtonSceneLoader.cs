using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneChangeButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button targetButton;

#if UNITY_EDITOR
    [Header("Scene (Drag from Project)")]
    [SerializeField] private SceneAsset targetScene;
#endif

    private void Awake()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogError("SceneChangeButton: No Button assigned.");
        }
    }

    private void LoadScene()
    {
#if UNITY_EDITOR
        if (targetScene == null)
        {
            Debug.LogError("SceneChangeButton: No Scene assigned.");
            return;
        }

        SceneManager.LoadScene(targetScene.name);
#else
        Debug.LogError("SceneChangeButton: SceneAsset reference only works in Editor. Use scene name for builds.");
#endif
    }
}
