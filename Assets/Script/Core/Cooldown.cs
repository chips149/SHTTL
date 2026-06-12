using UnityEngine;

public class Cooldown : MonoBehaviour
{
    [SerializeField] private float coolTime = 2f;
    public float CoolTime => coolTime;

    [Header("Shader冷却")]
    [SerializeField] private string _shaderProp = "_Cooldown";
    [SerializeField] private SpriteRenderer _targetRenderer;

    private MaterialPropertyBlock _propBlock;

    private float _readyTime = -999f;

    public bool IsOnCooldown => Time.time < _readyTime;

    public float GetNormalizedProgress()
    {
        if (!IsOnCooldown) return 1f;
        float remaining = _readyTime - Time.time;
        float elapsed = coolTime - remaining;
        return Mathf.Clamp01(elapsed / coolTime);
    }

    private void Awake()
    {
        if (_targetRenderer == null)
            _targetRenderer = GetComponent<SpriteRenderer>();

        if (_targetRenderer != null)
            _propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
       
            _targetRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(_shaderProp, GetNormalizedProgress());
            _targetRenderer.SetPropertyBlock(_propBlock);
    }

    private void OnEnable()  => BattleManager.OnCardSelectPhase += Reset;
    private void OnDisable() => BattleManager.OnCardSelectPhase -= Reset;

    public bool CanActivate() => Time.time >= _readyTime;
    public void Begin() => _readyTime = Time.time + coolTime;
    private void Reset() => _readyTime = -999f;
}
