using UnityEngine;

public class KeyCollection : MonoBehaviour,ICollectibles
{
    public void Collect()
    {
        GameManeger.Instance.OnKeyCollected();
        Destroy(gameObject);
    }
}
