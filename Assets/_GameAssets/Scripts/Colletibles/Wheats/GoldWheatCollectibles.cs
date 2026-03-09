
using UnityEngine;
using UnityEngine.UI;

public class GoldWheatCollectibles : MonoBehaviour, ICollectibles
{
    [SerializeField] private WheatDesingSO wheatDesingSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStateUI playerStateUI;
    private RectTransform playerBoosterTransform;
    private Image playerBoosterImage;

    private void Awake()
    {
        playerBoosterTransform = playerStateUI.GetBoosterSpeedTransform;
        playerBoosterImage = playerBoosterTransform.GetComponent<Image>();
    }

    public void Collect()
    {
        playerController.SetMovementSpeed(wheatDesingSO.IncreaseDecreaseMultiplier, wheatDesingSO.ResetBoostDuration);

        playerStateUI.PlayBoosterUIAnimation(playerBoosterTransform, playerBoosterImage, playerStateUI.GetGoldBoosterWheatImage, wheatDesingSO.ActiveSprite, wheatDesingSO.PassiveSprite,
        wheatDesingSO.ActiveWheatSprite, wheatDesingSO.PassiveWheatSprite, wheatDesingSO.ResetBoostDuration);

        Destroy(gameObject);
    }
}
