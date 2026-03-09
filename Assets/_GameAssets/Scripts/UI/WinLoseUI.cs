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
    }
    public void OnGameWin()
    {
        blackBackgroundObject.SetActive(true);
        winPopup.SetActive(true);
        blackBackgroundImage.DOFade(0.0f, animationDuration).SetEase(Ease.Linear);
        winPopupTransform.DOScale(1.5f, animationDuration).SetEase(Ease.OutBack);
    }
    public void OnGmaeLose()
    {
        blackBackgroundObject.SetActive(true);
        LosePopup.SetActive(true);
        blackBackgroundImage.DOFade(0.0f, animationDuration).SetEase(Ease.Linear);
        losePopupTransform.DOScale(1.5f, animationDuration).SetEase(Ease.OutBack);

    }
    
    
}
