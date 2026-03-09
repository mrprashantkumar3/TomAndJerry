using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private Button onMoreButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TMP_Text timerText;
    private void OnEnable()
    {
        timerText.text = timeUI.GetFinialTime();  
        onMoreButton.onClick.AddListener(OnOneMoreButtonClicked);      
    }
    private void OnOneMoreButtonClicked()
    {
        SceneManager.LoadScene(Consts.SceneNames.Game_Scene);
    }

}
