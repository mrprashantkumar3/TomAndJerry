using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class KeyCollectionNotification : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform notificationPanel;
    [SerializeField] private TMP_Text notificationText;    
              
         

    [Header("Settings")]
    [SerializeField] private int maxKeys = 4;
    [SerializeField] private float autoHideDuration = 0.5f;
    [SerializeField] private float slideDuration = 0.35f;

    private int currentKeyCount = 0;
    private Coroutine hideCoroutine;

    
    private float hiddenY;   
    private float shownY;    

    private void Awake()
    {
        // Panel ko upar chhupa do
        hiddenY = notificationPanel.rect.height + 50f;
        shownY = -notificationPanel.rect.height * 0.0f; 

        notificationPanel.anchoredPosition = new Vector2(0, hiddenY);
        notificationPanel.gameObject.SetActive(false);
        notificationText.text = $"{currentKeyCount}/{maxKeys}";

        
        
    }

    public void OnKeyCollected()
    {
        if (currentKeyCount >= maxKeys) return;

        currentKeyCount++;
        notificationText.text = $"{currentKeyCount}/{maxKeys}";
        ShowPanel();
    }

    private void ShowPanel()
    {
        
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);
        
        notificationPanel.gameObject.SetActive(true);
        transform.gameObject.SetActive(true);
        
        notificationPanel.DOKill();

      
        notificationPanel.anchoredPosition = new Vector2(0, hiddenY);
        notificationPanel.DOAnchorPosY(shownY, slideDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                Debug.Log("Slide complete!");
                hideCoroutine = StartCoroutine(AutoHide()); 
            });;

        
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSecondsRealtime(autoHideDuration);

        
        notificationPanel
            .DOAnchorPosY(hiddenY, 0.28f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => notificationPanel.gameObject.SetActive(false));
    }

    public void ResetKeys()
    {
        currentKeyCount = 0;
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        notificationPanel.gameObject.SetActive(false);
        
        notificationText.text = $"0/{maxKeys}";
    }
    public int GetFinalkeyCount()
    {
        return currentKeyCount;
    }

    private void OnDestroy()
    {
        if (hideCoroutine != null)
        StopCoroutine(hideCoroutine);
    
        notificationPanel?.DOKill();
        DOTween.Kill(notificationPanel); 
    }
}

