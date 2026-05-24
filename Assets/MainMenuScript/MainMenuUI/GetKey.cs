using DG.Tweening;
using TMPro;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform keyStartPosition;
    [SerializeField] private RectTransform keyEndPosition;
    [SerializeField] private RectTransform keyCount;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject keyParticleEffect;

    private ParticleSystem keyParticle;
    private bool isAnimating = false;
    private int keysCompleted = 0;
    private int totalKeysThisSession = 0;

    private void Start()
    {
        keyText.text = GameSessionData.DisplayedKeys.ToString();
    }

    public void RewardKey()
    {
        if (isAnimating) return;
        totalKeysThisSession = GameSessionData.UncollectedKeys;
        if (totalKeysThisSession <= 0) return;

        isAnimating = true;
        keysCompleted = 0;

        float delay = 0f;
        Vector3 startPos = ConvertUIWorldPosition(keyStartPosition);
        Vector3 endPos = ConvertUIWorldPosition(keyEndPosition);

        keyParticleEffect.transform.position = endPos;
        keyParticle = keyParticleEffect.GetComponent<ParticleSystem>();

        for (int i = 0; i < totalKeysThisSession; i++)
        {
            SpawnKey(startPos, endPos, ref delay);
            delay += 0.08f;
        }
    }

    private void SpawnKey(Vector3 startPos, Vector3 endPos, ref float delay)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.8f, 0.8f),
            Random.Range(-0.8f, 0.8f),
            0f);
        Vector3 scatterPosition = startPos + randomOffset;

        GameObject key = Instantiate(keyPrefab, startPos, Quaternion.identity);
        key.transform.localScale = Vector3.zero;

        // ✅ Key ke liye rotation alag — Z axis pe rotate (2D key feel)
        key.transform.DORotate(
            new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(delay);

        Sequence keySequence = DOTween.Sequence();
        keySequence.SetDelay(delay)
            .Append(key.transform.DOScale(0.5f, 0.15f).SetEase(Ease.OutBack))
            .Append(key.transform.DOScale(0.5f, 0.1f).SetEase(Ease.InOutQuad))
            .Join(key.transform.DOMove(scatterPosition, 0.35f).SetEase(Ease.OutCubic))
            .AppendInterval(0.05f)
            .Append(key.transform.DOMove(endPos, 0.45f).SetEase(Ease.InBack))
            .Join(key.transform.DOScale(0.3f, 0.45f).SetEase(Ease.InQuad))
            .AppendCallback(() => OnKeyReached(key));
    }

    private void OnKeyReached(GameObject key)
    {
        if (key == null) return;

        keyParticle.Play();

        Transform keyCountTransform = keyCount.transform;
        keyCountTransform.DOKill();
        keyCountTransform.localScale = Vector3.one;
        keyCountTransform.DOPunchScale(Vector3.one * 0.5f, 0.35f, 8, 0.5f)
            .OnComplete(() => keyCountTransform.localScale = Vector3.one);

        var keyImage = keyCount.GetComponent<UnityEngine.UI.Image>();
        if (keyImage != null)
        {
            keyImage.DOKill();
            keyImage.DOColor(new Color(1f, 0.85f, 0f), 0.1f) // ✅ Gold — key color
                .OnComplete(() => keyImage.DOColor(Color.white, 0.2f));
        }

        GameSessionData.DisplayedKeys += 1;
        keyText.text = GameSessionData.DisplayedKeys.ToString();

        key.transform.DOKill();
        Destroy(key);

        keysCompleted++;
        if (keysCompleted >= totalKeysThisSession)
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
