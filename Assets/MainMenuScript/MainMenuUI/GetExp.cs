using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetExp : MonoBehaviour
{
    
    
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI expCountText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject expParticleEffect; // ✅ Level complete particle

    [Header("Setting")]
    [SerializeField] private float expPerLevel = 900f;
    [SerializeField] private float sliderAnimDuration = 0.5f;

    private const string KEY_CURRENT_EXP = "CurrentExp";
    private const string KEY_CURRENT_LEVEL = "CurrentLevel";

    private int currentExp;
    private int currentLevel;
    private bool hasRewarded = false;
    private ParticleSystem levelCompleteParticle;

    private void Start()
    {
        currentExp   = PlayerPrefs.GetInt(KEY_CURRENT_EXP, 0);
        currentLevel = PlayerPrefs.GetInt(KEY_CURRENT_LEVEL, 1);

        expSlider.minValue = 0f;
        expSlider.maxValue = 1f;
        expSlider.interactable = false;

        if (expParticleEffect != null)
            levelCompleteParticle = expParticleEffect.GetComponent<ParticleSystem>();

        UpdateUIInstant(); // ✅ Bina animation ke load karo
    }

    private void OnEnable()
    {
        hasRewarded = false;
    }

    public void RewardExp()
    {
        if (hasRewarded) return;
        hasRewarded = true;

        int expToAdd = GameSessionData.UncollectedExperience;
        if (expToAdd <= 0) return;

        float previousSliderValue = currentExp / expPerLevel; // ✅ Pehle ki value save karo

        currentExp += expToAdd;

        // ✅ Level up hoga ya nahi check karo
        bool willLevelUp = currentExp >= (int)expPerLevel;

        if (willLevelUp)
        {
            // ✅ Pehle 0.5 se 1 tak animate karo phir level up
            AnimateSliderToFull(previousSliderValue, () =>
            {
                // Slider full hone ke baad
                CheckLevelUp();
                SaveData();
                UpdateUIAfterLevelUp();
            });
        }
        else
        {
            // ✅ Normal — seedha new value tak animate karo
            SaveData();
            float targetValue = currentExp / expPerLevel;
            AnimateSliderTo(previousSliderValue, targetValue, () =>
            {
                UpdateUIText();
            });
        }
    }

    private void AnimateSliderToFull(float fromValue, System.Action onComplete)
    {
        
        expSlider.value = fromValue;
        expSlider.DOValue(1f, sliderAnimDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                
                if (levelCompleteParticle != null)
                    levelCompleteParticle.Play();

                onComplete?.Invoke();
            });
    }

    private void AnimateSliderTo(float fromValue, float toValue, System.Action onComplete = null)
    {
        expSlider.value = fromValue;
        expSlider.DOValue(toValue, sliderAnimDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke());
    }

    private void UpdateUIAfterLevelUp()
    {
        // ✅ Slider 0 pe reset karo
        expSlider.DOKill();
        expSlider.value = 0f;

        // ✅ Agar abhi bhi exp bacha hai toh 0 se naye value tak animate
        float newTargetValue = currentExp / expPerLevel;
        if (newTargetValue > 0)
        {
            expSlider.DOValue(newTargetValue, sliderAnimDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }

        UpdateUIText();
        OnLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentExp >= (int)expPerLevel)
        {
            currentExp -= (int)expPerLevel;
            currentLevel++;
        }
    }

    private void OnLevelUp()
    {
        Debug.Log($"Level Up! New Level: {currentLevel}");

        if (levelText != null)
        {
            levelText.transform.DOKill();
            levelText.transform.localScale = Vector3.one;
            levelText.transform.DOPunchScale(Vector3.one * 0.5f, 0.4f, 8, 0.5f)
                .SetUpdate(true)
                .OnComplete(() => levelText.transform.localScale = Vector3.one);

            levelText.DOKill();
            levelText.DOColor(Color.yellow, 0.15f)
                .SetUpdate(true)
                .OnComplete(() => levelText.DOColor(Color.white, 0.3f));
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(KEY_CURRENT_EXP, currentExp);
        PlayerPrefs.SetInt(KEY_CURRENT_LEVEL, currentLevel);
        PlayerPrefs.Save();
    }

    private void UpdateUIInstant()
    {
        if (expCountText != null)
            expCountText.text = currentExp.ToString();

        if (levelText != null)
            levelText.text = $"Lv.{currentLevel:000}";

        expSlider.value = currentExp / expPerLevel;
    }

    private void UpdateUIText()
    {
        if (expCountText != null)
            expCountText.text = currentExp.ToString();

        if (levelText != null)
            levelText.text = $"Lv.{currentLevel:000}";
    }

}
