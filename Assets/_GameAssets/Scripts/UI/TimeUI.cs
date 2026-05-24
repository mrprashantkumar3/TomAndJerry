using System;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TimeUI : MonoBehaviour
{
   [SerializeField] private RectTransform timeRotatbletransform;
   [SerializeField] private TMP_Text timeText;
   [SerializeField] private float rotationDuration;
   [SerializeField] private Ease rotationEase;

   private const float totalTime = 900f;        
   private const float expPerInterval = 5f;    
   private const float expInterval = 10f;      
   private const float fullWinExp = 450f;

   private float elapsedTime;
   private float playedTime; 
   private bool isTimerRunning;
   private Tween rotationTween;
   private string finalTime;
   private int finalExperience;
   private bool isGameWin = false;


    private void Start()
    {
        // PlayerRotationAnimation();
        // StartTimer();

        GameManeger.Instance.OnGameStateChange += GameManegerOnGameStateChange;
    }
    

    private void GameManegerOnGameStateChange(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Play:
                PlayerRotationAnimation();
                StartTimer();
                break;
            case GameState.Pause:
                StopTimer();
                break;
            case GameState.Resume:
                ResumeTimer();
                break;
            case GameState.GameOver:
            if (!isGameWin)
                FinishTimer(isWin: false);
                break;
            case GameState.GameWin:
                isGameWin = true;
                FinishTimer(isWin: true);
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
        playedTime = 0f;  
        isTimerRunning = true;
        elapsedTime = totalTime;
        CancelInvoke(nameof(UpdateTimerUI));
        InvokeRepeating(nameof(UpdateTimerUI), 1f, 1f);
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
            CancelInvoke(nameof(UpdateTimerUI));
            if (elapsedTime > 0f)
            {
                InvokeRepeating(nameof(UpdateTimerUI), 1f, 1f);
                rotationTween.Play();
            }
        }   
    }
    private void FinishTimer(bool isWin)
    {
       // elapsedTime = 0f;
        StopTimer();
        if (isWin)
        {
            finalExperience = (int)fullWinExp;
            Debug.Log($"Game Win! Full Experience: {finalExperience}");
        }
        else
        {
            int intervals = Mathf.FloorToInt(playedTime / expInterval);
            finalExperience = intervals * (int)expPerInterval;
        }

        finalTime = GetFormattedElapsedTime(playedTime); 
        Debug.Log($"Game End | Played: {playedTime}s | Experience: {finalExperience}");
    }
    
    private string GetFormattedElapsedTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);   

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void UpdateTimerUI()
    {
        if(!isTimerRunning){ return;}
        elapsedTime -= 1;
        playedTime += 1f;
        if(elapsedTime <= 0f)
        {
            elapsedTime = 0f;
           // UpdateTimerDisplay();
            StopTimer();
            if(GameManeger.Instance.GetCurrentGameState() != GameState.GameOver &&
            GameManeger.Instance.GetCurrentGameState() != GameState.GameWin)
            {
                GameManeger.Instance.OnTimerFinished();
            }
            return;
        }
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public string GetFinialTime()
    {
        return finalTime;
    }
    public int GetFinalExperience()
    {
        return finalExperience;
    }
    private void OnDestroy()
    {
        rotationTween?.Kill();
        CancelInvoke(nameof(UpdateTimerUI));
    }

}
