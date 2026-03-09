using UnityEngine;

public class Health : MonoBehaviour, IHealthable
{
    public void GiveHealth()
    {
        HealthManeger.Instance.Health(1);
        
        Destroy(gameObject);
    }
}
