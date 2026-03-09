using System;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
   [SerializeField] private RectTransform timeRotatbletransform;
   [SerializeField] private TMP_Text timeText;
   [SerializeField] private float rotationDuration;
   [SerializeField] private Ease rotationEase;
   private float elapsedTime;
   private bool isTimerRunning;
   private Tween rotationTween;
   private string finalTime;


    private void Start()
    {
        PlayerRotationAnimation();
        StartTimer();

        GameManeger.Instance.OnGameStateChange += GameManegerOnGameStateChange;
    }

    private void GameManegerOnGameStateChange(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Pause:
            StopTimer();
            break;
            case GameState.Resume:
            ResumeTimer();
            break;
            case GameState.GameOver:
            FinishTimer();
            break;
        }
    }
   

    private void PlayerRotationAnimation()
    {
       rotationTween = timeRotatbletransform.DORotate(new Vector3(0f,0f,360), rotationDuration, RotateMode.FastBeyond360)
       .SetLoops(-1, LoopType.Restart).SetEase(rotationEase);
    }

    private void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 15f;
       InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
    }
     private void StopTimer()
    {
        
        isTimerRunning = false;
        CancelInvoke(nameof(UpdateTimerUI));
        rotationTween.Pause();
    }
    private void ResumeTimer()
    {
       if(!isTimerRunning)
        {
            isTimerRunning = true;
            InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
            rotationTween.Play();
        }
    }
    private void FinishTimer()
    {
        elapsedTime = 0f;
        StopTimer();
        finalTime = GetFormattedElapsedTime();
    }
    private string GetFormattedElapsedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);   

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void UpdateTimerUI()
    {
        if(!isTimerRunning){ return;}
        elapsedTime -= 1; 

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public string GetFinialTime()
    {
        return finalTime;
    }

}
