using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    [Header("Score Settings")]
    public int score = 0;
    public TMP_Text scoreText; // Assign your TMP UI element in Inspector

    private void Awake()
    {
        // Singleton pattern
        if (inst == null)
            inst = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreUI(); // Display initial score
    }

    public void IncrementScore(int amount = 1)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score;
    }
}
