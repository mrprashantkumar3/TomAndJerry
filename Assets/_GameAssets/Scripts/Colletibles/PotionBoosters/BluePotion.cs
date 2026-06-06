using UnityEngine;
using UnityEngine.UI;

public class BluePotion : MonoBehaviour, ICollectibles
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BoosterDesingSO wheatDesingSO;
    [SerializeField] private PlayerStateUI playerStateUI;
     private RectTransform playerBoosterTransform;
    private Image playerBoosterImage;

    private void Awake()
    {
        playerBoosterTransform = playerStateUI.GetBoosterJumpTransform;
        playerBoosterImage = playerBoosterTransform.GetComponent<Image>();
    }


    public void Collect()
    {
        playerController.SetJumpForce(wheatDesingSO.IncreaseDecreaseMultiplier, wheatDesingSO.ResetBoostDuration);
        
        playerStateUI.PlayBoosterUIAnimation(playerBoosterTransform, playerBoosterImage, playerStateUI.GetJumpBoosterImage, wheatDesingSO.ActiveSprite, wheatDesingSO.PassiveSprite,
        wheatDesingSO.ActiveWheatSprite, wheatDesingSO.PassiveWheatSprite, wheatDesingSO.ResetBoostDuration);

        CameraShake.Instance.ShakeCamera(0.15f, 0.15f);

        Destroy(gameObject);
    }
}
