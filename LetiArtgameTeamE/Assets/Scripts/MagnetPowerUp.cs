using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 6f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            CoinMagnet magnet = other.GetComponent<CoinMagnet>();
            if (magnet != null)
            {
                magnet.ActivateMagnet(duration);
            }

           
            Destroy(gameObject);
        }
    }
}
