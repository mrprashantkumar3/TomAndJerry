using UnityEngine;
using TMPro;
using DG.Tweening;

public class DiamondCount : MonoBehaviour
{
    [SerializeField] private TMP_Text diamondCounterTaxt;
   [SerializeField] private Color diamondCounterColor;
   [SerializeField] private float colorDuration;
   [SerializeField] private float scaleDuration;
   private RectTransform diamondCountRectTransform;

   private void Awake()
    {
        diamondCountRectTransform = diamondCounterTaxt.gameObject.GetComponent<RectTransform>();
    }
    private void OnDestroy()
    {
        diamondCounterTaxt?.DOKill();
        diamondCountRectTransform?.DOKill();
    }

    public void SetDiamondCounterText(int counter)
    {
        diamondCounterTaxt.text = counter.ToString();
    }
    public int GetFinalDiamondCount()
    {
        return int.Parse(diamondCounterTaxt.text);
    }

    public void SetDiamondCompleted()
    {
        diamondCounterTaxt.DOKill();
        diamondCountRectTransform.DOKill();
        diamondCounterTaxt.DOColor(diamondCounterColor, colorDuration);
        diamondCountRectTransform.DOScale(1.2f, scaleDuration).SetEase(Ease.OutBack);
    }
}
