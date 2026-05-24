using UnityEngine;

public class IngredientsCollectible : MonoBehaviour, ICollectibles
{
    public void Collect()
    {
       GameManeger.Instance.OnEggCollected();
       CameraShake.Instance.ShakeCamera(0.15f, 0.15f);
       Destroy(gameObject);
    }
}
