using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIntarectionController : MonoBehaviour
{
    [SerializeField] private Transform playerVisualTransform;

    private PlayerController playerController;
    private Rigidbody playerRigidbody;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<ICollectibles>(out var collectibles))
        {
            collectibles.Collect();
        }
        if(other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GiveDamage(playerRigidbody, playerVisualTransform);
            CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
        }
        if(other.gameObject.TryGetComponent<IHealthable>(out var healthable))
        {
            healthable.GiveHealth();
        }
        // if (other.CompareTag(Consts.WheatTypes.GOLD_WHEAT))
        // {
        //    other.gameObject?.GetComponent<GoldWheatCollectibles>().Collect();
        // }
        // if (other.CompareTag(Consts.WheatTypes.HOLY_WHEAT))
        // {
        //     other.gameObject?.GetComponent<HolyWeatCollictibles>().Collect();
        // }
        // if (other.CompareTag(Consts.WheatTypes.ROTTEN_WHEAT))
        // {
        //     other.gameObject?.GetComponent<RottenWheatCollectibles>().Collect();
        // }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<IBoostable>(out var boostable))
        {
            boostable.Boost(playerController);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GiveDamage(playerRigidbody, playerVisualTransform);
            CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
        }
    }
    

}
