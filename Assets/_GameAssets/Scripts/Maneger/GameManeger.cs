using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    
    public static GameManeger Instance { get; private set; }
    public event Action<GameState> OnGameStateChange;
    [SerializeField] private CatController catController;
    [SerializeField] private PlayerHealthUI playerHealthUI;
    [SerializeField] private IngredientCount eggCounterUI;
    [SerializeField] private CoinCount coinCounterUI;
    [SerializeField] private DiamondCount diamondCounterUI;
    [SerializeField] private WinLoseUI winLoseUI;
    [SerializeField] private int maxEggCoumt = 10;
    [SerializeField] private KeyCollectionNotification keyNotification;
    
    private int currentKeyCount;
    private GameState currentgameState;
    private int currentEggCount;
    private int currentCoinCount;
    private int currentDiamondCount;
    
    private float delay;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        catController.OnCatCatched += CatController_OnCatCatched;   
        HealthManeger.Instance.OnPlayerDeath += HealthManeger_OnPlayerDeath;
        
    }

    private void CatController_OnCatCatched()
    {
        CameraShake.Instance.ShakeCamera(0.5f, 0.5f, 0.15f);
        //HealthManeger.Instance.Damage(5);
        playerHealthUI.AnimateDamageForAll();
        StartCoroutine(OnGameOver());
             
            //isCatCatch = true;      
    }

    private void HealthManeger_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver());    
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.CutScene);
    }
    public void ChangeGameState(GameState gameState)
    {
      OnGameStateChange?.Invoke(gameState);
      currentgameState = gameState;
      Debug.Log("Game State:"+ gameState);

      switch (gameState)
        {
            case GameState.Pause:
                // SAB kuch pause — player, cat, cutscene, physics sab
                Time.timeScale = 0f;
                break;

            case GameState.Resume:
            case GameState.Play:
            case GameState.CutScene:
                // Normal speed resume
                Time.timeScale = 1f;
                break;

            case GameState.GameOver:
                
                break;
        } 
    }
    public void OnEggCollected()
    {
        currentEggCount++;
        eggCounterUI.SetEggCounterText(currentEggCount, maxEggCoumt);
        if (currentEggCount == maxEggCoumt)
        {
            //win
            Debug.Log("Game Win");
            eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameWin); 
            winLoseUI.gameObject.SetActive(true);
            winLoseUI.OnGameWin();
            ChangeGameState(GameState.GameOver);
            
            
        }
        
    }
    public void OnTimerFinished()
    {
        if (currentgameState == GameState.GameWin || 
        currentgameState == GameState.GameOver)
        {
            return;
        }
        if (currentEggCount >= maxEggCoumt)
        {
            return;
        }

        Debug.Log("Timer khatam, ingredients complete nahi — Game Lose!");
        ChangeGameState(GameState.GameOver);
        winLoseUI.gameObject.SetActive(true);
        winLoseUI.OnGameLose();
    }
    public void OnCoinCollected()
    {
        currentCoinCount++;
        coinCounterUI.SetCoinCounterText(currentCoinCount);
    }
    public void OnDiamondCollected()
    {
        currentDiamondCount++;
        diamondCounterUI.SetDiamondCounterText(currentDiamondCount);
    }   
    public void OnKeyCollected()
    {
        currentKeyCount++;
        keyNotification.OnKeyCollected();
    }
    
    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(delay);
        ChangeGameState(GameState.GameOver);
        winLoseUI.gameObject.SetActive(true);
        winLoseUI.OnGameLose();
    }
   
     public GameState GetCurrentGameState()
    {
        return currentgameState;
    }
    public int GetCurrentCoins() => currentCoinCount;
    public int GetCurrentDiamonds() => currentDiamondCount;
    public int GetCurrentKeys() => currentKeyCount;
}
