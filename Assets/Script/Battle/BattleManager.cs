using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    /// <summary>进入选卡阶段时触发，所有构筑应在此重置冷却</summary>
    public static event System.Action OnCardSelectPhase;

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

    public void Start()
    {
        BattleStats.Reset();

        _marblePool = new ObjectPool<MarbleBehavior>(
            ActionOnCreate,
            ActionOnGet,
            ActionOnRelease,
            null);

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

    #endregion

    public void PushMarble(MarbleBehavior marble)
    {
        if (!marble.gameObject.activeSelf) return;
        _marblePool.Release(marble);
    }

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

    private void EndTurn()
    {
        Time.timeScale = 0;
        OnCardSelectPhase?.Invoke();
        drawCardPanel.OpenDrawCardPanel();
    }

    public MarbleBehavior CreateNewMarble()
    {
        var marble = _marblePool.Get();

        if (marble == null)
        {
            marble = Instantiate(marblePrefab);
            marble.bm = this;
            _allMarbles.Add(marble);
            _marbles.Add(marble);
        }

        BattleStats.MarbleSpawned++;
        return marble;
    }

    public void OnDestroy()
    {
        foreach (var marble in new List<MarbleBehavior>(_allMarbles))
        {
            if (marble != null)
                Destroy(marble.gameObject);
        }
        _allMarbles.Clear();
        _marbles.Clear();
    }
}