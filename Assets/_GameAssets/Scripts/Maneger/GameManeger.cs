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
    [SerializeField] private EggCount eggCounterUI;
    [SerializeField] private CoinCount coinCounterUI;
    [SerializeField] private WinLoseUI winLoseUI;
    [SerializeField] private int maxEggCoumt = 10;
    private GameState currentgameState;
    private int currentEggCount;
    private int currentCoinCount;
    private float delay;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        HealthManeger.Instance.OnPlayerDeath += HealthManeger_OnPlayerDeath;
        catController.OnCatCatched += CatController_OnCatCatched;
    }

    private void CatController_OnCatCatched()
    {
        playerHealthUI.AnimateDamageForAll();
        StartCoroutine(OnGameOver());
    }

    private void HealthManeger_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver());    
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Play);
    }
    public void ChangeGameState(GameState gameState)
    {
      OnGameStateChange?.Invoke(gameState);
      currentgameState = gameState;
      Debug.Log("Game State:"+ gameState);
        
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
            ChangeGameState(GameState.GameOver);
            winLoseUI.OnGameWin();
        }
        
    }
    public void OnCoinCollected()
    {
        currentCoinCount++;
        coinCounterUI.SetCoinCounterText(currentCoinCount);
    }
    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(delay);
        ChangeGameState(GameState.GameOver);
        winLoseUI.OnGmaeLose();
    }
   
     public GameState GetCurrentGameState()
    {
        return currentgameState;
    }
}
