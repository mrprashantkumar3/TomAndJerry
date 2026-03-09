using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private Button tryAgain;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TMP_Text timerText;
     private void OnEnable()
    {
        timerText.text = timeUI.GetFinialTime();  
        tryAgain.onClick.AddListener(OnTryAgainButtonClicked);      
    }
    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene(Consts.SceneNames.Game_Scene);
    }
}
