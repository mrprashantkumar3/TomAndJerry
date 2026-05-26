using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;



public class PlayerStateUI : MonoBehaviour
{
    
    [Header("reference")]

    [SerializeField] private RectTransform playerWalkingTransform;
    [SerializeField] private RectTransform playerSlidingTransform;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private RectTransform boosterSpeedTransform;
    [SerializeField] private RectTransform boosterJumpTransform;
    [SerializeField] private RectTransform boosterSlowTransform;
    [SerializeField] private PlayableDirector playableDirector;

    [Header("Image")]
    [SerializeField] private Image goldBoosterWheatTmage;
    [SerializeField] private Image holyBoosterWheatTmage;
    [SerializeField] private Image rottenBoosterWheatTmage;

    [Header("Sprite")]
    [SerializeField] private Sprite playerWalkingActiveSprite;
    [SerializeField] private Sprite playerWalkingPassiveSprite;
    [SerializeField] private Sprite playerSlidingActiveSprite;
    [SerializeField] private Sprite playerSlidingPassiveSprite;

    [Header("Setting")]
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;

    public RectTransform GetBoosterSpeedTransform => boosterSpeedTransform;
    public RectTransform GetBoosterJumpTransform => boosterJumpTransform;
    public RectTransform GetBoosterSlowTransform => boosterSlowTransform;
    public Image GetGoldBoosterWheatImage => goldBoosterWheatTmage;
    public Image GetHolyBoosterWheatImage => holyBoosterWheatTmage;
    public Image GetRottenBoosterWheatImage => rottenBoosterWheatTmage;
    private Image playerWalkingImage;
    private Image playerSlidingImage;

    private void Awake()
    {
        playerWalkingImage = playerWalkingTransform.GetComponent<Image>();
        playerSlidingImage = playerSlidingTransform.GetComponent<Image>();
    }

    private void Start()
    {
        playerController.OnPlayerStateChange += PlayerController_OnPlayerStateChange;
        playableDirector.stopped += OnTimelineFinished;
        //SetStateUserInterfaces(playerWalkingActiveSprite, playerSlidingPassiveSprite, playerWalkingTransform, playerSlidingTransform);
       
    }
    private void OnDestroy()
    {
        if (playerController != null)
            playerController.OnPlayerStateChange -= PlayerController_OnPlayerStateChange;

        if (playableDirector != null)
            playableDirector.stopped -= OnTimelineFinished;

        
        // playerWalkingTransform?.DOKill();
        // playerSlidingTransform?.DOKill();

        DOTween.Kill(playerWalkingTransform);
        DOTween.Kill(playerSlidingTransform);
        DOTween.Kill(boosterSpeedTransform);
        DOTween.Kill(boosterJumpTransform);
        DOTween.Kill(boosterSlowTransform);
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        SetStateUserInterfaces(playerWalkingActiveSprite, playerSlidingPassiveSprite, playerWalkingTransform, playerSlidingTransform);
    }

    private void PlayerController_OnPlayerStateChange(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Move:
            SetStateUserInterfaces(playerWalkingActiveSprite, playerSlidingPassiveSprite, playerWalkingTransform, playerSlidingTransform);

            break;
            case PlayerState.Running:
            //case PlayerState.Slide:
            SetStateUserInterfaces(playerWalkingPassiveSprite, playerSlidingActiveSprite, playerSlidingTransform, playerWalkingTransform);

            break;
        }
    }
    private void SetStateUserInterfaces(Sprite playerWalkingSprite, Sprite playerSlidingSprite,
    RectTransform activeTransform, RectTransform passiveTransform)
    {
        if (playerWalkingImage == null || playerSlidingImage == null)
        {
            Debug.LogWarning("PlayerStateUI: Image components destroy ho chuke hain, UI update skip kar raha hai.");
            return;
        }
        playerWalkingImage.sprite = playerWalkingSprite;
        playerSlidingImage.sprite = playerSlidingSprite;

        activeTransform.DOKill();
        passiveTransform.DOKill();

        activeTransform.DOAnchorPosX(50f, moveDuration).SetEase(moveEase);
        passiveTransform.DOAnchorPosX(-50f, moveDuration).SetEase(moveEase);
    }

    private IEnumerator SetBoosterUserInterfaces(RectTransform activeTransform, Image boosterImage,
    Image wheatImage, Sprite activeSprite, Sprite passiveSprite, Sprite activeWheatSprite, Sprite passiveWheatSprite, 
    float duration)
    {
        boosterImage.sprite = activeSprite;
        wheatImage.sprite = activeWheatSprite;
        activeTransform.DOAnchorPosX(-75f, moveDuration).SetEase(moveEase);

        yield return new WaitForSeconds(duration);
        if (boosterImage == null || wheatImage == null) yield break;

        boosterImage.sprite = passiveSprite;
        wheatImage.sprite = passiveWheatSprite;
        activeTransform.DOAnchorPosX(-50f, moveDuration).SetEase(moveEase);
    }

    public void PlayBoosterUIAnimation(RectTransform activeTransform, Image boosterImage,
    Image wheatImage, Sprite activeSprite, Sprite passiveSprite, Sprite activeWheatSprite, Sprite passiveWheatSprite, 
    float duration)
    {
        StartCoroutine(SetBoosterUserInterfaces(activeTransform, boosterImage, wheatImage, activeSprite,
        passiveSprite, activeWheatSprite, passiveWheatSprite, duration));
    }
}
