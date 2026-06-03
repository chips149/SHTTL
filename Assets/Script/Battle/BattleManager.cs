using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public MarbleBehavior marblePrefab;
    public DrawCardPanel drawCardPanel;
    private ObjectPool<MarbleBehavior> _marblePool;
    public Animator ani;
    private Transform _spawnPoint;

    private readonly List<MarbleBehavior> _marbles = new();
    private readonly HashSet<MarbleBehavior> _allMarbles = new();

    private void Awake()
    {
        _spawnPoint = GameObject.Find("BallSpawnPoint").transform;
        
        GameState.Bm = this;
        GameState.Um = ModulesManager.Get<UserAreaManager>();
        GameState.Player = FindObjectOfType<Player>();
        GameState.MonsterUI = FindObjectOfType<MonsterUI>();
    }

    // TODO:  世界之初
    public void Start()
    {
        _marblePool = new ObjectPool<MarbleBehavior>(
            ActionOnCreate,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy);

        EndTurn();
    }

    private void Update()
    {
        foreach (var marble in _marbles)
        {
            marble.Tick(Time.deltaTime);
        }
    }

    #region 池方法

    private MarbleBehavior ActionOnCreate()
    {
        var marble = Instantiate(marblePrefab);
        marble.bm = this;
        _allMarbles.Add(marble);
        return marble;
    }

    private void ActionOnGet(MarbleBehavior marble)
    {
        if (marble == null) return;

        _marbles.Add(marble);
        marble.gameObject.SetActive(true);
    }


    private void ActionOnRelease(MarbleBehavior marble)
    {
        marble.gameObject.SetActive(false);
        _marbles.Remove(marble);

        if (_marbles.Count == 0)
        {
            EndTurn();
        }
    }

    private void ActionOnDestroy(MarbleBehavior marble)
    {
        _allMarbles.Remove(marble);
        if (marble != null)
            Destroy(marble.gameObject);
    }

    #endregion

    public void PushMarble(MarbleBehavior marble)
    {
        if (!marble.gameObject.activeSelf) return;
        _marblePool.Release(marble);
    }


    // TODO: 一轮开始
    public void NewTurn()
    {
        PlayShootAnimation();
        
        Time.timeScale = 1;
    }

    public void PlayShootAnimation()
    {
        ani.SetTrigger("Shoot");
    }

    public void CreateNewMarbleAndSetPos()
    {
        var marble = CreateNewMarble();
        var x = Random.Range(-0.15f, 0.15f);
        var y = _spawnPoint.localPosition.y;

        marble.transform.position = new Vector3(x, y, -0.5f); 
    }

    // TODO: 一轮结束
    private void EndTurn()
    {
        Time.timeScale = 0;
        
        drawCardPanel.OpenDrawCardPanel();
    }

    public MarbleBehavior CreateNewMarble()
    {
        var marble = _marblePool.Get();

        // 池取出的弹珠可能已被销毁（场景卸载时序问题），重新创建
        if (marble == null)
        {
            marble = Instantiate(marblePrefab);
            marble.bm = this;
            _allMarbles.Add(marble);
            _marbles.Add(marble);
        }

        return marble;
    }

    public void OnDestroy()
    {
        // 销毁所有弹珠（池中空闲的 + 活跃的）
        _marblePool?.Clear();

        foreach (var marble in _allMarbles.ToArray())
        {
            if (marble != null)
                Destroy(marble.gameObject);
        }
        _allMarbles.Clear();
        _marbles.Clear();
    }
}