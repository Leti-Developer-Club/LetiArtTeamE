using UnityEngine;
using System.Reflection; // Needed to read fields dynamically

public class RandomPowerUp : MonoBehaviour
{
    [Header("List of Possible Power-Ups")]
    public GameObject[] powerUps;     // All possible power-up prefabs
    public GameObject pickupEffect;   // Optional pickup visual effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateRandomPowerUp(other);
        }
    }

    void ActivateRandomPowerUp(Collider player)
    {
        // 🔹 Play pickup VFX
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, transform.rotation);

        // 🔹 Ensure list isn’t empty
        if (powerUps == null || powerUps.Length == 0)
        {
            Debug.LogWarning("⚠️ No power-ups assigned to RandomPowerUp!");
            Destroy(gameObject);
            return;
        }

        // 🔹 Pick a random power-up prefab
        int index = Random.Range(0, powerUps.Length);
        GameObject chosenPrefab = powerUps[index];

        if (chosenPrefab == null)
        {
            Debug.LogWarning("⚠️ Chosen power-up prefab is null!");
            Destroy(gameObject);
            return;
        }

        // 🔹 Spawn it at the player's position
        GameObject spawned = Instantiate(chosenPrefab, player.transform.position, Quaternion.identity);

        // 🔹 Try to read the duration field automatically
        float detectedDuration = GetPowerUpDuration(spawned);

        // 🔹 Show countdown timer with correct duration
        PowerUpUI ui = FindObjectOfType<PowerUpUI>();
        if (ui != null && detectedDuration > 0)
        {
            string powerUpName = chosenPrefab.name.Replace("(Clone)", "").Trim();
            ui.ShowPowerUp("Random: " + powerUpName, detectedDuration);
        }

        // 🔹 Destroy the random power-up object itself
        Destroy(gameObject);
    }

    // 🧩 Helper method — detects "duration" or "boostDuration" automatically
    float GetPowerUpDuration(GameObject obj)
    {
        Component[] components = obj.GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            System.Type type = comp.GetType();

            // Try to find a public or private field called "duration" or "boostDuration"
            FieldInfo durationField = type.GetField("duration", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (durationField != null)
                return (float)durationField.GetValue(comp);

            FieldInfo boostField = type.GetField("boostDuration", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (boostField != null)
                return (float)boostField.GetValue(comp);
        }

        // If nothing found, default to 5 seconds
        return 5f;
    }
}
