using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [Header("动画参数")]
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private float randomOffsetRadius = 0.6f;

    public static Vector2 TextRectSize = new Vector2(1, 1);
    public static float FontSize = 24;
    public static Vector3 ParentScale = new Vector3(0.05f, 0.05f, 0.05f);
    public static float HeavyScaleMultiplier = 3f;

    private static readonly Queue<DamagePopup> Pool = new();
    private static GameObject _sharedPrefab;
    private static Transform _poolRoot;

    private TMP_Text _tmpText;
    private CanvasGroup _canvasGroup;
    private float _startTime;

    private void Awake()
    {
        _tmpText = GetComponentInChildren<TMP_Text>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 文字渲染层级前移
        _tmpText.GetComponent<Renderer>().sortingOrder = 100;
    }

    public static DamagePopup Spawn(Vector3 worldPosition, float damage, float? customOffsetRadius = null, bool isHeavy = false)
    {
        DamagePopup popup = GetFromPool();
        popup.Show(worldPosition, damage, customOffsetRadius, isHeavy);
        return popup;
    }

    private static DamagePopup GetFromPool()
    {
        while (Pool.Count > 0)
        {
            DamagePopup p = Pool.Dequeue();
            p.transform.SetParent(null);
            p.gameObject.SetActive(true);
            return p;
        }

        if (_sharedPrefab == null)
        {
            _sharedPrefab = new GameObject("DamagePopup");
            _sharedPrefab.SetActive(false);

            _sharedPrefab.AddComponent<CanvasGroup>();

            GameObject textGo = new GameObject("Text");
            textGo.transform.SetParent(_sharedPrefab.transform, false);
            TMP_Text tmp = textGo.AddComponent<TextMeshPro>();
            tmp.fontSize = FontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.rectTransform.sizeDelta = TextRectSize;

            _sharedPrefab.AddComponent<DamagePopup>();
        }

        GameObject go = Instantiate(_sharedPrefab);
        go.transform.localScale = ParentScale;
        go.SetActive(true);
        return go.GetComponent<DamagePopup>();
    }

    private void Show(Vector3 worldPosition, float damage, float? customOffsetRadius, bool isHeavy = false)
    {
        float radius = customOffsetRadius ?? randomOffsetRadius;
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        Vector3 finalPos = worldPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
        transform.position = finalPos;

        int displayDamage = Mathf.RoundToInt(damage);
        _tmpText.text = displayDamage.ToString();

        if (isHeavy)
        {
            _tmpText.color = Color.red;
            transform.localScale = ParentScale * HeavyScaleMultiplier;
        }
        else
        {
            _tmpText.color = Color.white;
            transform.localScale = ParentScale;
        }

        _canvasGroup.alpha = 1f;
        _startTime = Time.time;
    }

    private void Update()
    {
        float elapsed = Time.time - _startTime;

        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

        if (elapsed < fadeDuration)
        {
            _canvasGroup.alpha = 1f - (elapsed / fadeDuration);
        }
        else
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        Pool.Enqueue(this);

        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("[DamagePopupPool]").transform;
            _poolRoot.gameObject.SetActive(false);
        }
        transform.SetParent(_poolRoot, false);
    }

    public static void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DamagePopup p = GetFromPool();
            p.ReturnToPool();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_tmpText == null) _tmpText = GetComponentInChildren<TMP_Text>();
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }
#endif
}
