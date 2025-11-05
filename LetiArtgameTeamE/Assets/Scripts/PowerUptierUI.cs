using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public TextMeshProUGUI powerUpNameText;
    public TextMeshProUGUI timerText;
    public Image panel;

    void Start()
    {
        HidePowerUp(); // Might already be here
    }

    public void ShowPowerUp(string powerUpName, float duration)
    {
        powerUpNameText.text = powerUpName;
        timerText.text = duration.ToString("F1") + "s";
        panel.gameObject.SetActive(true);
        powerUpNameText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = timeLeft.ToString("F1") + "s";
    }

    // 👇 THIS METHOD IS MISSING — ADD IT NOW
    public void HidePowerUp()
    {
        panel.gameObject.SetActive(false);
        powerUpNameText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }
}
