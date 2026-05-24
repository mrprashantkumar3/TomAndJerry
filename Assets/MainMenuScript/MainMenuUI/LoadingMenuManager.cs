using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMenuManager : MonoBehaviour
{
    public static LoadingMenuManager Instance;
    [SerializeField] private GameObject loadingScene;
    [SerializeField] private Slider ProgressBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        //Instance = this;
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
            

            
    }
    public void SwitchToScene(string sceneName, float delay = 1f)
    {
       loadingScene.SetActive(true);
       ProgressBar.value = 0;
       StartCoroutine(SwitchSceneWithWait(sceneName, delay));
    }
    IEnumerator SwitchSceneWithWait(string sceneName, float delay)
        {
            yield return new WaitForSeconds(delay);
            //loadingScene.SetActive(false);

            //Tween animationTween = StartAnimationForLoad();

            // Wait for the animation to complete
            //yield return animationTween.WaitForCompletion();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
               ProgressBar.value = asyncLoad.progress;
               yield return null;
            }
            loadingScene.SetActive(false);

            //EndAnimation();
        }

}
