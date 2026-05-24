using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
   [Header("Reference")]
   [SerializeField] private GameObject blackBackgroundObject;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject LosePopup;

    [Header("Setting")]
    [SerializeField] private float animationDuration = 0.3f;

    private Image blackBackgroundImage;
    private RectTransform winPopupTransform;
    private RectTransform losePopupTransform;

    private void Awake()
    {
        blackBackgroundImage = blackBackgroundObject.GetComponent<Image>();
        winPopupTransform = winPopup.GetComponent<RectTransform>();
        losePopupTransform = LosePopup.GetComponent<RectTransform>();
        SetAlpha(blackBackgroundImage, 0f);
        blackBackgroundObject.SetActive(false);
        winPopup.SetActive(false);
        LosePopup.SetActive(false);
    }

    public void OnGameWin()
    {
        blackBackgroundImage.DOKill();
        winPopupTransform.DOKill();


        blackBackgroundObject.SetActive(true);
        winPopup.SetActive(true);
        SetAlpha(blackBackgroundImage, 0f);
       // winPopupTransform.localScale = Vector3.zero;
        blackBackgroundImage.DOFade(0.5f, animationDuration).SetEase(Ease.Linear).SetUpdate(true);
        winPopupTransform.DOScale(1f, animationDuration).SetEase(Ease.OutBack).SetUpdate(true);
        
    }
    public void OnGameLose()
    {
        blackBackgroundImage.DOKill();
        losePopupTransform.DOKill();

        blackBackgroundObject.SetActive(true);
        LosePopup.SetActive(true);


        SetAlpha(blackBackgroundImage, 0f);
        //losePopupTransform.localScale = Vector3.zero;

        blackBackgroundImage.DOFade(0.85f, animationDuration).SetEase(Ease.Linear).SetUpdate(true);
        losePopupTransform.DOScale(1f, animationDuration).SetEase(Ease.InBack).SetUpdate(true);

    }
    private void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
    private void OnDestroy()
    {
        blackBackgroundImage?.DOKill();
        winPopupTransform?.DOKill();
        losePopupTransform?.DOKill();
    }
    
    
}
