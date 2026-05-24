using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private CoinCount coinCount;
    [SerializeField] private DiamondCount diamondCount;
    [SerializeField] private KeyCollectionNotification keyCollectionNotification;
    [SerializeField] private Button tryAgain;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TMP_Text ExperienceText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text diamondText;

     private void OnEnable()
    {
        ExperienceText.text = timeUI.GetFinalExperience().ToString();
        coinText.text =  coinCount.GetFinalCoinCount().ToString();
        keyText.text = keyCollectionNotification.GetFinalkeyCount().ToString();
        diamondText.text = diamondCount.GetFinalDiamondCount().ToString();
        tryAgain.onClick.AddListener(OnTryAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        // mainMenuButton.onClick.AddListener(() =>
        // {
        //    LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.MainMenu_Scene); 
        //    SceneManager.LoadScene(Consts.SceneNames.MainMenu_Scene);
        // });      
    }
    private void OnDisable()
    {
        tryAgain.onClick.RemoveListener(OnTryAgainButtonClicked);
        mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }
    private void OnTryAgainButtonClicked()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(Consts.SceneNames.Game_Scene);
        //LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.Game_Scene);
    }
    private void OnMainMenuButtonClicked()
    {
        
        DOTween.KillAll();
        //SceneManager.LoadScene(Consts.SceneNames.MainMenu_Scene);
        LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.MainMenu_Scene);
    }
}
