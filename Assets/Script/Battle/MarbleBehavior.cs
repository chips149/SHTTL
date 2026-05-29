using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarbleBehavior : MonoBehaviour
{
    private static Sprite _sprite;
    private Vector3 _marbleScale;

    public BattleManager bm;
    public ParticleSystem eggPrefab;
    private SpriteRenderer _renderer;
    private Rigidbody2D _rb;
    private GameObject _cupcakeFollower;
    private CakeFly _cakeFly;
    private static readonly Vector3 _cakeOffset = new Vector3(0, 0.3f, 0);
    
    private static readonly string CupcakePrefabPath = "Prefab/Bullet/WatchOutForTheCupcake";
    
    // property
    public bool hasCake;
    public bool isClone;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite ??= Resources.Load<Sprite>("2D/Item_KEY_Qiu_IMG/Item_KEY_Qiu_IMG");
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = 0; 
    }

    void OnEnable()
    {
        // 只有克隆弹珠（彩蛋）才显示彩带拖尾
        if (eggPrefab != null)
        {
            eggPrefab.gameObject.SetActive(isClone);
        }
    }
    void Start()
    {
        _marbleScale = transform.localScale;
    }

    private Vector3 scale;

    private void OnCollisionEnter2D()
    {
        transform.localScale = new Vector3(0.5f, 0.47f, 0.5f);
        Invoke(nameof(Recover), 0.1f);
    }

    void Recover()
    {
        transform.localScale = _marbleScale;
    }

    public void Tick(float dt)
    {
        if (!hasCake || _cupcakeFollower == null) return;

        _cupcakeFollower.transform.position = transform.position + _cakeOffset;
    }
    

    public void AttachCake()
    {
        if (hasCake) return;
        hasCake = true;

        GameObject prefab = Resources.Load<GameObject>(CupcakePrefabPath);
        _cupcakeFollower = Instantiate(prefab, transform.position + _cakeOffset, Quaternion.identity);

        _cakeFly = _cupcakeFollower.GetComponent<CakeFly>();
        _cakeFly.enabled = false;
    }

    public void DetachCake()
    {
        if (!hasCake || _cupcakeFollower == null) return;
        hasCake = false;

        _cakeFly.enabled = true;

        _cupcakeFollower = null;
        _cakeFly = null;
    }

    //  TODO: 归位
    private void OnDisable()
    {
        _renderer.sprite = _sprite;
        isClone = false;
        hasCake = false;
        eggPrefab.gameObject.SetActive(false);

        if (_cupcakeFollower != null)
        {
            Destroy(_cupcakeFollower);
            _cupcakeFollower = null;
            _cakeFly = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            bm.PushMarble(this);
        }
    }
}