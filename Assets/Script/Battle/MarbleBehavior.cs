using System;
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

    private readonly Dictionary<string, GameObject> _followers = new();

    private static readonly Vector3 _cakeOffset = new Vector3(0, 0.2f, 0);
    private const float _cakeSmoothTime = 0.12f;
    private const string CupcakePrefabPath = "Prefab/Bullet/WatchOutForTheCupcake";

    public bool hasCake;
    public bool isClone;


    public void AttachFollower(string key, GameObject prefab, Vector3 offset,
                               float smoothTime, Action<GameObject> onAttach = null)
    {
        if (_followers.ContainsKey(key)) return;

        GameObject go = FollowerPool.Get(prefab);
        go.transform.position = transform.position + offset;

        Follower follower = go.GetComponent<Follower>();
        if (follower == null) follower = go.AddComponent<Follower>();
        follower.StartFollow(transform, offset, smoothTime);

        onAttach?.Invoke(go);
        _followers[key] = go;
    }

    public void DetachFollower(string key, Action<GameObject> onDetach = null)
    {
        if (!_followers.TryGetValue(key, out GameObject go)) return;
        _followers.Remove(key);

        Follower follower = go.GetComponent<Follower>();
        if (follower != null)
        {
            follower.StopFollow();
            follower.OnDetach();
        }

        onDetach?.Invoke(go);
    }

    public void ClearFollowers()
    {
        foreach (var kv in _followers)
        {
            var go = kv.Value;
            if (go != null) Destroy(go);
        }
        _followers.Clear();
    }


    public void AttachCake()
    {
        if (hasCake) return;
        hasCake = true;

        GameObject prefab = Resources.Load<GameObject>(CupcakePrefabPath);
        AttachFollower("cake", prefab, _cakeOffset, _cakeSmoothTime, go =>
        {
            go.GetComponent<CakeFly>().enabled = false;
        });
    }

    public void DetachCake()
    {
        if (!hasCake) return;
        hasCake = false;

        DetachFollower("cake", go =>
        {
            CakeFly fly = go.GetComponent<CakeFly>();
            if (fly != null)
            {
                fly.enabled = true;
                go.transform.SetParent(null);
            }
        });
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite ??= Resources.Load<Sprite>("2D/Item_KEY_Qiu_IMG/Item_KEY_Qiu_IMG");
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = 0; 
    }

    void OnEnable()
    {
        eggPrefab.gameObject.SetActive(isClone);
    }

    void Start()
    {
        _marbleScale = transform.localScale;
    }

    private void OnCollisionEnter2D()
    {
        transform.localScale = new Vector3(0.5f, 0.47f, 0.5f);
        Invoke(nameof(Recover), 0.1f);
    }

    void Recover()
    {
        transform.localScale = _marbleScale;
    }

    public void Tick(float dt) { }

    private void OnDisable()
    {
        _renderer.sprite = _sprite;
        isClone = false;
        hasCake = false;
        eggPrefab.gameObject.SetActive(false);
        ClearFollowers();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            bm.PushMarble(this);
        }
    }
}