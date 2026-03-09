using UnityEngine;

public class ItemDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private float force = -10f; 
    public void GiveDamage(Rigidbody playerRigidbody, Transform playerVisualTransform)
    {
        HealthManeger.Instance.Damage(1);
        playerRigidbody.AddForce(playerVisualTransform.forward * force, ForceMode.Impulse);
        Destroy(gameObject);
    }
}
