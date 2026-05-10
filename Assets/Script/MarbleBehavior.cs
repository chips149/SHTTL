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
    
    // property
    public bool hasCake;
    public bool isClone;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite ??= Resources.Load<Sprite>("2D/Item_KEY_Qiu_IMG/Item_KEY_Qiu_IMG");
        _renderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
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
    }

    //  TODO: 归位
    private void OnDisable()
    {
        _renderer.sprite = _sprite;
        isClone = false;
        hasCake = false;
        eggPrefab .gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            bm.PushMarble(this);
        }
    }
}