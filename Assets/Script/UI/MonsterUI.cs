using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    public Image monsterImage;

    public float scaleAdd = 0.2f;
    public float maxScale = 1f;
    public float scaleSpeed = 2f;

    public int attackDamage = 5;

    private int _stage;
    private float intervalTime = 5f;

    private float _targetScale;
    private bool _isScaling;

    void Start()
    {
        monsterImage.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
        _targetScale = 0.05f;
    }

    void Update()
    {
        intervalTime += Time.deltaTime;

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

        if (intervalTime >= 5f)
        {
            intervalTime = 0;
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
        IBeHit beHit = GameState.Player.GetComponent<IBeHit>();
        beHit?.BeHit(new BeHitData
        {
            damage = attackDamage,
            from = "Monster"
        });
    }

    public void Knockback()
    {
        if (_stage > 1)
        {
            _stage--;
        }

        _targetScale = 0.05f + _stage * scaleAdd;
        _targetScale = Mathf.Min(_targetScale, maxScale);
        _isScaling = true;
    }
}