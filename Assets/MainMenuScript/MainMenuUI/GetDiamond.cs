using DG.Tweening;
using TMPro;
using UnityEngine;

public class GetDiamond : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform diamondStartPosition;
    [SerializeField] private RectTransform diamondEndPosition;
    [SerializeField] private RectTransform diamondCount;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private GameObject diamondPrefab;
    [SerializeField] private GameObject diamondParticleEffect;

    private ParticleSystem diamondParticle;
    private bool isAnimating = false;
    private int diamondsCompleted = 0;
    private int totalDiamondsThisSession = 0;

    private void Start()
    {
        diamondText.text = GameSessionData.DisplayedDiamonds.ToString();
    }

    public void RewardDiamond()
    {
        if (isAnimating) return;
        totalDiamondsThisSession = GameSessionData.UncollectedDiamonds;
        if (totalDiamondsThisSession <= 0) return;

        isAnimating = true;
        diamondsCompleted = 0;

        float delay = 0f;
        Vector3 startPos = ConvertUIWorldPosition(diamondStartPosition);
        Vector3 endPos = ConvertUIWorldPosition(diamondEndPosition);

        diamondParticleEffect.transform.position = endPos;
        diamondParticle = diamondParticleEffect.GetComponent<ParticleSystem>();

        for (int i = 0; i < totalDiamondsThisSession; i++)
        {
            SpawnDiamond(startPos, endPos, ref delay);
            delay += 0.08f;
        }
    }

    private void SpawnDiamond(Vector3 startPos, Vector3 endPos, ref float delay)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.8f, 0.8f),
            Random.Range(-0.8f, 0.8f),
            0f);
        Vector3 scatterPosition = startPos + randomOffset;

        GameObject diamond = Instantiate(diamondPrefab, startPos, Quaternion.identity);
        diamond.transform.localScale = Vector3.zero;

        diamond.transform.DORotate(
            new Vector3(0, 360, 0), 0.4f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(delay);

        Sequence diamondSequence = DOTween.Sequence();
        diamondSequence.SetDelay(delay)
            .Append(diamond.transform.DOScale(0.5f, 0.15f).SetEase(Ease.OutBack))
            .Append(diamond.transform.DOScale(0.5f, 0.1f).SetEase(Ease.InOutQuad))
            .Join(diamond.transform.DOMove(scatterPosition, 0.35f).SetEase(Ease.OutCubic))
            .AppendInterval(0.05f)
            .Append(diamond.transform.DOMove(endPos, 0.45f).SetEase(Ease.InBack))
            .Join(diamond.transform.DOScale(0.3f, 0.45f).SetEase(Ease.InQuad))
            .AppendCallback(() => OnDiamondReached(diamond));
    }

    private void OnDiamondReached(GameObject diamond)
    {
        if (diamond == null) return;

        diamondParticle.Play();

        Transform diamondCountTransform = diamondCount.transform;
        diamondCountTransform.DOKill();
        diamondCountTransform.localScale = Vector3.one;
        diamondCountTransform.DOPunchScale(Vector3.one * 0.5f, 0.35f, 8, 0.5f)
            .OnComplete(() => diamondCountTransform.localScale = Vector3.one);

        var diamondImage = diamondCount.GetComponent<UnityEngine.UI.Image>();
        if (diamondImage != null)
        {
            diamondImage.DOKill();
            diamondImage.DOColor(new Color(0.5f, 0.8f, 1f), 0.1f) // ✅ Light blue — diamond color
                .OnComplete(() => diamondImage.DOColor(Color.white, 0.2f));
        }

        GameSessionData.DisplayedDiamonds += 1;
        diamondText.text = GameSessionData.DisplayedDiamonds.ToString();

        diamond.transform.DOKill();
        Destroy(diamond);

        diamondsCompleted++;
        if (diamondsCompleted >= totalDiamondsThisSession)
        {
            isAnimating = false;
            GameSessionData.ClearPendingReward();
        }
    }

    private Vector3 ConvertUIWorldPosition(RectTransform uiElement)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, uiElement.position);
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, uiCamera.nearClipPlane + 0.1f));
        return worldPos;
    }
}

