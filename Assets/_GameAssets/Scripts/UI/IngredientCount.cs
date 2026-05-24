using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;

public class IngredientCount : MonoBehaviour
{
   [SerializeField] private TMP_Text ingredientCounterTaxt;
   [SerializeField] private Color ingredientCounterColor;
   [SerializeField] private float colorDuration;
   [SerializeField] private float scaleDuration;
   private RectTransform ingredientCountRectTransform;
    private void Awake()
    {
        ingredientCountRectTransform = ingredientCounterTaxt.gameObject.GetComponent<RectTransform>();
    }
    private void OnDestroy()
    {
        // coinCounterTaxt?.DOKill();
        // coinCountRectTransform?.DOKill();
        ingredientCounterTaxt?.DOKill();
        ingredientCountRectTransform?.DOKill();
    }

    public void SetEggCounterText(int counter, int max)
    {
        ingredientCounterTaxt.text = counter.ToString() + "/" + max.ToString();
    }

    public void SetEggCompleted()
    {
        ingredientCounterTaxt.DOKill();
        ingredientCountRectTransform.DOKill();
        ingredientCounterTaxt.DOColor(ingredientCounterColor, colorDuration);
        ingredientCountRectTransform.DOScale(1.2f, scaleDuration).SetEase(Ease.OutBack);
    }
}
