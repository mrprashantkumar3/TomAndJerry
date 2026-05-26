using System.Transactions;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManuController : MonoBehaviour
{
    
    [SerializeField] private Button playButton;
    //[SerializeField] private Button QuitButton;
    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
           // TransitionManager.Instance.LoadLevel(Consts.SceneNames.Game_Scene);
            //SceneManager.LoadScene(Consts.SceneNames.Game_Scene);
            LoadingMenuManager.Instance.SwitchToScene(Consts.SceneNames.Game_Scene);
        });
    }

}
