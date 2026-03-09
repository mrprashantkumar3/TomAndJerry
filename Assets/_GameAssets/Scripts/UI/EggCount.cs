using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EggCount : MonoBehaviour
{
   [SerializeField] private TMP_Text eggCounterTaxt;
   [SerializeField] private Color eggCounterColor;
   [SerializeField] private float colorDuration;
   [SerializeField] private float scaleDuration;
   private RectTransform eggCountRectTransform;
    private void Awake()
    {
        eggCountRectTransform = eggCounterTaxt.gameObject.GetComponent<RectTransform>();
    }

    public void SetEggCounterText(int counter, int max)
    {
        eggCounterTaxt.text = counter.ToString() + "/" + max.ToString();
    }

    public void SetEggCompleted()
    {
        eggCounterTaxt.DOColor(eggCounterColor, colorDuration);
        eggCountRectTransform.DOScale(1.2f, scaleDuration).SetEase(Ease.OutBack);
    }
}
