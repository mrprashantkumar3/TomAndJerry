using UnityEngine;

public class MagnetPickup : MonoBehaviour, ICollectibles
{
     public void Collect()
    {
       Destroy(gameObject);
    }
}
