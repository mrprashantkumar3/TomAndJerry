using DG.Tweening;
using TMPro;
using UnityEngine;

public class GetCoin : MonoBehaviour
{
   [SerializeField] private Camera uiCamera;
   [SerializeField] private RectTransform coinStartPosition;
    [SerializeField] private RectTransform coinEndPosition;
    [SerializeField] private RectTransform coinCount;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] private GameObject coinParticleEffect;

    
    private bool isAnimating = false; // ✅ Double trigger rokne ke liye
    private int coinsCompleted = 0;   // ✅ Kitne coins end pe pahunche
    private int totalCoinsThisSession = 0;
  

    private ParticleSystem coinParticle;

    private void Start()
    {
       
        coinText.text = GameSessionData.DisplayedCoins.ToString();
    }
    public void RewardCoin()
    {
        if (isAnimating) return;
        totalCoinsThisSession = GameSessionData.UncollectedCoins;
        if (totalCoinsThisSession <= 0) return;

        isAnimating = true;
        coinsCompleted = 0;
        
        float delay = 0f;
        //float incrementZ = -1f;
        Vector3 startPos = ConvertUIWorldPosition(coinStartPosition);
        Vector3 endPos = ConvertUIWorldPosition(coinEndPosition);

        coinParticleEffect.transform.position = endPos;
        coinParticle = coinParticleEffect.GetComponent<ParticleSystem>();
        for(int i = 0; i < totalCoinsThisSession; i++)
        {
            SpawmCoin(startPos, endPos, ref delay);
            delay += 0.08f;
            // GameObject coin = Instantiate(coinPrefab, startPos, Quaternion.identity, transform);
            // coin.transform.DOMove(endPos, 0.5f).SetDelay(delay + (i * incrementZ));
        }
    }
    private void SpawmCoin(Vector3 startPos, Vector3 endPos, ref float delay)
    {
        
        Vector3 randomOffset = new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.7f, 0.7f), 0f);
        Vector3 spawnPostion = startPos + randomOffset;

        GameObject coin = Instantiate(coinPrefab, startPos, Quaternion.identity);
        

        
        coin.transform.DORotate(new Vector3(0, 360, 0), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetDelay(delay);

        Sequence coinSequence = DOTween.Sequence();

        coinSequence.SetDelay(delay)
        .Append(coin.transform.DOMove(spawnPostion, 0.5f).SetEase(Ease.OutQuad))
        .Append(coin.transform.DOMove(endPos, 0.5f).SetEase(Ease.InQuad))
        .AppendCallback(() => onCoinReached(coin));
        //delay += 0.04f;
       // incrementZ -= 0.5f;
        
    }
    private void onCoinReached(GameObject coin)
    {
        if(coin == null) return;
        coinParticle.Play();
        Transform coinCountTransform = coinCount.transform;
        
        coinCountTransform.DOKill();
        coinCountTransform.localScale = Vector3.one;
        coinCountTransform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1f)
        .OnComplete(() => coinCountTransform.localScale = Vector3.one);

        GameSessionData.DisplayedCoins += 1;
        coinText.text = GameSessionData.DisplayedCoins.ToString();

        coin.transform.DOKill();
        Destroy(coin);

        coinsCompleted++;
        if (coinsCompleted >= totalCoinsThisSession)
        {
            isAnimating = false;
            
            GameSessionData.ClearPendingReward();
        }

    }
    private Vector3 ConvertUIWorldPosition(RectTransform uiElement)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, uiElement.position);
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, uiCamera.nearClipPlane + 0.1f));
        return worldPos;
        // Vector3 worldPos = uiElement.position;
        // worldPos = uiCamera.ScreenToWorldPoint(uiCamera.WorldToScreenPoint(worldPos));
        // worldPos.z = 0f; 
        // return worldPos;
        // Vector3 screenPos = uiCamera.WorldToScreenPoint(uiElement.position);
        // Vector3 worldPos = uiCamera.ScreenToWorldPoint(screenPos);
        // worldPos.z = -1;
        // return worldPos;
    }
}
