
using UnityEngine;
using UnityEngine.UI;

public class GreenPotion : MonoBehaviour, ICollectibles
{
    [SerializeField] private BoosterDesingSO wheatDesingSO;
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

        playerStateUI.PlayBoosterUIAnimation(playerBoosterTransform, playerBoosterImage, playerStateUI.GetSpeedBoosterImage, wheatDesingSO.ActiveSprite, wheatDesingSO.PassiveSprite,
        wheatDesingSO.ActiveWheatSprite, wheatDesingSO.PassiveWheatSprite, wheatDesingSO.ResetBoostDuration);
        CameraShake.Instance.ShakeCamera(0.15f, 0.15f);
        Destroy(gameObject);
    }
}
