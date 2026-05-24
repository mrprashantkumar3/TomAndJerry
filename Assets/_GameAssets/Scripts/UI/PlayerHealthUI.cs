
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Reference")]
   [SerializeField] private Image[] playerHealthImage;

    [Header("Reference")]
    [SerializeField] private Sprite playerHealthSprite;
    [SerializeField] private Sprite playerUnHealthSprite;

    [SerializeField] private float scaleDuration;
    private RectTransform[] playerHealthTransform;

    private void Awake()
    {
        playerHealthTransform = new RectTransform[playerHealthImage.Length];

        for(int i = 0; i < playerHealthImage.Length; i++)
        {
            playerHealthTransform[i] = playerHealthImage[i].gameObject.GetComponent<RectTransform>();
        }
    }
    private void OnDestroy()
    {
        if (playerHealthTransform == null) return;

        for (int i = 0; i < playerHealthTransform.Length; i++)
        {
            playerHealthTransform[i]?.DOKill();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AnimateDamage();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AnimateDamageForAll();
        }
    }
    public void AnimateDamage()
    {
       for(int i = 0; i < playerHealthImage.Length; i++)
        {
            if (playerHealthImage[i].sprite == playerHealthSprite)
            {
                AnimateDamageSprite(playerHealthImage[i], playerHealthTransform[i]);
                break;
            }
        }
    }
    public void AnimateHealth()
    {
       for(int i = 0; i < playerHealthImage.Length; i++)
        {
            if (playerHealthImage[i].sprite == playerUnHealthSprite)
            {
                AnimateHealthSprite(playerHealthImage[i], playerHealthTransform[i]);
                break;
            }
        }
    }
    public void AnimateHeal()
    {
       for(int i = 0; i < playerHealthImage.Length; i++)
        {
            if (playerHealthImage[i].sprite == playerUnHealthSprite)
            {
                AnimateHealthSprite(playerHealthImage[i], playerHealthTransform[i]);
            }
            
                
            
        }
    }

    public void AnimateDamageForAll()
    {
        for(int i = 0; i < playerHealthImage.Length; i++)
        {
             AnimateDamageSprite(playerHealthImage[i], playerHealthTransform[i]);
        }
    }

    private void AnimateDamageSprite(Image activeImage, RectTransform activeImageTransform)
    {
        
        //activeImageTransform.DOKill();
        //activeImageTransform.localScale = Vector3.one;
        activeImageTransform.DOScale(0f, scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            Debug.Log("OnComplete REACHED!");

            activeImage.sprite  = playerUnHealthSprite;
            activeImageTransform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack);
        });
    }
     private void AnimateHealthSprite(Image passiveImage, RectTransform activeImageTransform)
    {
        
        activeImageTransform.DOScale(0f, scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            passiveImage.sprite  = playerHealthSprite;
           
            activeImageTransform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack);
        });
    }

    

}
