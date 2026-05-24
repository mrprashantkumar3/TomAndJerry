using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinCount : MonoBehaviour
{
   [SerializeField] private TMP_Text coinCounterTaxt;
   [SerializeField] private Color coinCounterColor;
   [SerializeField] private float colorDuration;
   [SerializeField] private float scaleDuration;
   private RectTransform coinCountRectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Awake()
    {
        coinCountRectTransform = coinCounterTaxt.gameObject.GetComponent<RectTransform>();
    }
    private void OnDestroy()
    {
        coinCounterTaxt?.DOKill();
        coinCountRectTransform?.DOKill();
    }

    public void SetCoinCounterText(int counter)
    {
        coinCounterTaxt.text = counter.ToString();
    }
    public int GetFinalCoinCount()
    {
        return int.Parse(coinCounterTaxt.text);
    }

    public void SetCoinCompleted()
    {
        coinCounterTaxt.DOKill();
        coinCountRectTransform.DOKill();
        coinCounterTaxt.DOColor(coinCounterColor, colorDuration);
        coinCountRectTransform.DOScale(1.2f, scaleDuration).SetEase(Ease.OutBack);
    }
}
