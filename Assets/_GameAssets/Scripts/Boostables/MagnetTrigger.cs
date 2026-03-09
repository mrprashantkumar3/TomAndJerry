using UnityEngine;

public class MagnetTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoinCollection"))
        {
            MagnetCollect magnet = other.GetComponent<MagnetCollect>();
            if(magnet != null)
            {
                magnet.isAttracted = true;
            }
        }
    }
    
}
