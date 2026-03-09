using UnityEngine;
using UnityEngine.UI;

public class HolyWeatCollictibles : MonoBehaviour, ICollectibles
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WheatDesingSO wheatDesingSO;
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
        
        playerStateUI.PlayBoosterUIAnimation(playerBoosterTransform, playerBoosterImage, playerStateUI.GetHolyBoosterWheatImage, wheatDesingSO.ActiveSprite, wheatDesingSO.PassiveSprite,
        wheatDesingSO.ActiveWheatSprite, wheatDesingSO.PassiveWheatSprite, wheatDesingSO.ResetBoostDuration);

        Destroy(gameObject);
    }
}
