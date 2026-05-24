using UnityEngine;

public class DiamondCollection : MonoBehaviour, ICollectibles
{
    public void Collect()
    {
        GameManeger.Instance.OnDiamondCollected();
        Destroy(gameObject);
    }
}
