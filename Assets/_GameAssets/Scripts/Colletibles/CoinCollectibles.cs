using UnityEngine;

public class CoinCollectibles : MonoBehaviour,ICollectibles
{
     public void Collect()
    {
        GameManeger.Instance.OnCoinCollected();
        Destroy(gameObject);
    }
}
