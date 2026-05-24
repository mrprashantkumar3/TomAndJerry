using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private CoinCount coinCount;
    [SerializeField] private DiamondCount diamondCount;
    [SerializeField] private KeyCollectionNotification keyCollectionNotification;
    [SerializeField] private Button onMoreButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text diamondText;
    private void OnEnable()
    {
        int sessionCoins    = GameManeger.Instance.GetCurrentCoins();
        int sessionDiamonds = GameManeger.Instance.GetCurrentDiamonds();
        int sessionKeys     = GameManeger.Instance.GetCurrentKeys();
        int sessionExp      = timeUI.GetFinalExperience();

        GameSessionData.SaveSessionData(sessionCoins, sessionDiamonds, sessionKeys, sessionExp);

        coinText.text    = sessionCoins.ToString();
        diamondText.text = sessionDiamonds.ToString();
        keyText.text     = sessionKeys.ToString();
        expText.text     = sessionExp.ToString();

        // timerText.text = timeUI.GetFinalExperience().ToString();
        // coinText.text =  coinCount.GetFinalCoinCount().ToString();
        // keyText.text = keyCollectionNotification.GetFinalkeyCount().ToString();
        // diamondText.text = diamondCount.GetFinalDiamondCount().ToString();
        onMoreButton.onClick.AddListener(OnOneMoreButtonClicked);
        mainMenuButton.onClick.AddListener(() =>
        {
           LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.MainMenu_Scene); 
           //SceneManager.LoadScene(Consts.SceneNames.MainMenu_Scene);
        });      
    }

    private void OnDisable()
    {
        onMoreButton.onClick.RemoveListener(OnOneMoreButtonClicked);
        //mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }
    private void OnOneMoreButtonClicked()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(Consts.SceneNames.Game_Scene);
        //LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.Game_Scene);
    }

}
