using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PowerUpUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI powerUpNameText;
    public TextMeshProUGUI timerText;
    public Image panel;

    private Coroutine countdownRoutine;

    void Start()
    {
        HidePowerUp();
    }

    /// <summary>
    /// Shows the power-up UI and starts a countdown timer.
    /// </summary>
    public void ShowPowerUp(string powerUpName, float duration)
    {
        // Activate all UI elements
        panel.gameObject.SetActive(true);
        powerUpNameText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        // Set texts
        powerUpNameText.text = powerUpName;
        timerText.text = duration.ToString("F1") + "s";

        // Stop any previous countdown before starting a new one
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(Countdown(duration));
    }

    /// <summary>
    /// Coroutine to update the countdown timer in real time.
    /// </summary>
    private IEnumerator Countdown(float timeLeft)
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.Max(0, timeLeft).ToString("F1") + "s";
            yield return null;
        }

        // When timer ends, hide the UI
        HidePowerUp();
    }

    /// <summary>
    /// Hides the power-up UI.
    /// </summary>
    public void HidePowerUp()
    {
        panel.gameObject.SetActive(false);
        powerUpNameText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Stop any running countdown
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
        }
    }
}

