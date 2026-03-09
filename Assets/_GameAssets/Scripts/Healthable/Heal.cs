using UnityEngine;

public class Heal : MonoBehaviour, IHealthable
{
    public void GiveHealth()
    {
        HealthManeger.Instance.Heal(1);
        Destroy(gameObject);
    }
}

