using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanelObject;
    [SerializeField] private Button closeButton;
    [SerializeField] private float animationDuration = 0.3f;
    [Header("Reward Scripts")] 
    [SerializeField] private GetCoin getCoin;       
    [SerializeField] private GetDiamond getDiamond; 
    [SerializeField] private GetKey getKey;         
    [SerializeField] private GetExp getExp;

    [Header("Total UI Texts")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text expText;

    private RectTransform panelTransform;

    private void Awake()
    {
        panelTransform = rewardPanelObject.GetComponent<RectTransform>();
    }

    private void Start()
    {
        closeButton.onClick.AddListener(OnCollectButtonClicked);

        if (GameSessionData.HasPendingReward)
            ShowRewardPanel();
        else
            rewardPanelObject.SetActive(false);
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(OnCollectButtonClicked);
    }

    private void ShowRewardPanel()
    {
        rewardPanelObject.SetActive(true);
        panelTransform.localScale = Vector3.zero;

        // ✅ Cumulative total dikhao
        // totalCoinText.text    = GameSessionData.TotalCoins.ToString();
        // totalDiamondText.text = GameSessionData.TotalDiamonds.ToString();
        // totalKeyText.text     = GameSessionData.TotalKeys.ToString();
        // totalExpText.text     = GameSessionData.TotalExperience.ToString();
        coinText.text    = GameSessionData.UncollectedCoins.ToString();
        diamondText.text = GameSessionData.UncollectedDiamonds.ToString();
        keyText.text     = GameSessionData.UncollectedKeys.ToString();
        expText.text     = GameSessionData.UncollectedExperience.ToString();

        panelTransform.DOScale(1f, animationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void OnCollectButtonClicked()
    {
        getCoin?.RewardCoin();
        getDiamond?.RewardDiamond();
        getKey?.RewardKey();
        getExp?.RewardExp();

        panelTransform.DOScale(0f, animationDuration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                rewardPanelObject.SetActive(false);
                
            });
    }
}
