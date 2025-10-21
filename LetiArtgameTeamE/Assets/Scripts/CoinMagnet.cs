using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 6f;   
    [SerializeField] private float magnetForce = 10f;   

    private bool isMagnetActive = false;

    void Update()
    {
        if (isMagnetActive)
        {
            AttractCoins();
        }
    }

    public void ActivateMagnet(float duration)
    {
        StopAllCoroutines(); 
        StartCoroutine(MagnetRoutine(duration));
    }

    private IEnumerator MagnetRoutine(float duration)
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
    }

    private void AttractCoins()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Coin"))
            {
                // Smoothly move coin toward the player
                Vector3 direction = (transform.position - hit.transform.position).normalized;
                hit.transform.position += direction * magnetForce * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (isMagnetActive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, magnetRadius);
        }
    }
}
