using UnityEngine;

public class EggCollectible : MonoBehaviour, ICollectibles
{
    public void Collect()
    {
       GameManeger.Instance.OnEggCollected();
       Destroy(gameObject);
    }
}
