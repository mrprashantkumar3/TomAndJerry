using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
   
    [Header("Reference")]
    [SerializeField] private GameObject settingPopupObject;
    [SerializeField] private GameObject blackBackgroundObject;
    
     [Header("Button")]
    [SerializeField] private Button settingButton;
    [SerializeField] private Button MusicButton;
    [SerializeField] private Button SoundButton;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button mainMenuButton;
    private Image blackBackgroundImage;
    [SerializeField] private float animationDuration;
    private void Awake()
    {
        blackBackgroundImage = blackBackgroundObject.GetComponent<Image>();
        settingPopupObject.transform.localScale = Vector3.zero;
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        ResumeButton.onClick.AddListener(OnResumeButtonClicked);
    }
    private void OnSettingButtonClicked()
    {
        GameManeger.Instance.ChangeGameState(GameState.Pause);
        blackBackgroundObject.SetActive(true);
        settingPopupObject.SetActive(true);
        blackBackgroundImage.DOFade(0.8f, animationDuration).SetEase(Ease.Linear);
        settingPopupObject.transform.DOScale(1.5f, animationDuration).SetEase(Ease.OutBack);
    }
    private void OnResumeButtonClicked()
    {
         GameManeger.Instance.ChangeGameState(GameState.Resume);

        blackBackgroundImage.DOFade(0f,animationDuration).SetEase(Ease.Linear);
        settingPopupObject.transform.DOScale(0f, animationDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
             GameManeger.Instance.ChangeGameState(GameState.Resume);
              blackBackgroundObject.SetActive(false);
              settingPopupObject.SetActive(false);
        });
    }
   
    
}
