using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    public Image monsterImage;
    public Animator animator;

    public float scaleAdd = 0.3f;
    public float maxScale = 1.3f;
    public float scaleSpeed = 2f;

    public int attackDamage = 5;
    public float attackAnimDuration = 0.3f;

    public float initScale = 0.05f;

    private static readonly int MoveHash = Animator.StringToHash("Move");
    private static readonly int AtkHash = Animator.StringToHash("ATK");
    private static readonly int BeHitHash = Animator.StringToHash("BeHit");

    private int _stage;
    private float _intervalTime;

    private float _targetScale;
    private bool _isScaling;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        monsterImage.transform.localScale = new Vector3(initScale, initScale, 1f);
        _targetScale = initScale;
    }

    void Update()
    {
        _intervalTime += Time.deltaTime;

        if (_isScaling)
        {
            float current = monsterImage.transform.localScale.x;
            float smooth = Mathf.Lerp(current, _targetScale, scaleSpeed * Time.deltaTime);
            monsterImage.transform.localScale = new Vector3(smooth, smooth, 1f);

            if (Mathf.Abs(smooth - _targetScale) < 0.01f)
            {
                _isScaling = false;
            }
        }
        if (animator != null)
            animator.SetBool(MoveHash, _isScaling);

        if (_intervalTime >= 5f)
        {
            _intervalTime = 0;
            _stage++;

            if (_stage <= 4)
            {
                StartScale();
            }
            else
            {
                Attack();
            }
        }
    }

    void StartScale()
    {
        _targetScale = monsterImage.transform.localScale.x + scaleAdd;
        _targetScale = Mathf.Min(_targetScale, maxScale);
        _isScaling = true;
    }

    private void Attack()
    {
        if (animator != null)
            animator.SetTrigger(AtkHash);
        Invoke(nameof(OnAttackHit), attackAnimDuration);
    }

    public void OnAttackHit()
    {
        IBeHit beHit = GameState.Player?.GetComponent<IBeHit>();
        beHit?.BeHit(new BeHitData
        {
            damage = attackDamage,
            from = "Monster"
        });
    }

    public void PlayBeHit()
    {
        if (animator != null)
            animator.SetTrigger(BeHitHash);
    }

    public void Knockback()
    {
        if (_stage > 1)
        {
            _stage--;
        }

        _targetScale = initScale + _stage * scaleAdd;
        _targetScale = Mathf.Min(_targetScale, maxScale);
        _isScaling = true;
    }
}