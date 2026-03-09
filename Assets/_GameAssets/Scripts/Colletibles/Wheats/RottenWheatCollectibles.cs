using UnityEngine;
using UnityEngine.UI;

public class RottenWheatCollectibles : MonoBehaviour, ICollectibles
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WheatDesingSO wheatDesingSO;
    
    [SerializeField] private PlayerStateUI playerStateUI;
    private RectTransform playerBoosterTransform;
    private Image playerBoosterImage;

    private void Awake()
    {
        playerBoosterTransform = playerStateUI.GetBoosterSlowTransform;
        playerBoosterImage = playerBoosterTransform.GetComponent<Image>();
    }
    public void Collect()
    {
        playerController.SetMovementSpeed(wheatDesingSO.IncreaseDecreaseMultiplier, wheatDesingSO.ResetBoostDuration);

         playerStateUI.PlayBoosterUIAnimation(playerBoosterTransform, playerBoosterImage, playerStateUI.GetRottenBoosterWheatImage, wheatDesingSO.ActiveSprite, wheatDesingSO.PassiveSprite,
        wheatDesingSO.ActiveWheatSprite, wheatDesingSO.PassiveWheatSprite, wheatDesingSO.ResetBoostDuration);

        Destroy(gameObject);
    }
}
