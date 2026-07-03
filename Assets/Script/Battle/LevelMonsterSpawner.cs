using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class LevelConfigCollection
{
    public List<LevelConfig> levels = new();
}

[Serializable]
public class LevelConfig
{
    public int levelId;
    public string levelName;
    public List<MonsterWaveConfig> waves = new();
}

[Serializable]
public class MonsterWaveConfig
{
    public int enemyId;
    public int hp;
    public int damage;
}

[Serializable]
public class MonsterPrefabMapping
{
    public int enemyId;
    public GameObject prefab;
}

public class LevelMonsterSpawner : MonoBehaviour
{
    [Header("Level")]
    public int currentLevelId;
    public bool useSelectedLevelFromMainMenu = true;
    public string levelConfigResourcePath = "LevelConfig/levels_fixed";

    [Header("Spawn")]
    public Transform spawnPoint;
    public List<MonsterPrefabMapping> monsterPrefabs = new();
    [Tooltip("显示剩余敌人提示后延迟刷怪的时间（秒）")]
    public float preSpawnDelay = 1.5f;

    [Header("Result")]
    public GameObject victoryPanel;
    public bool pauseGameWhenLevelCleared = true;

    [Header("UI")]
    public Slider monsterHpSlider;
    public TMP_Text monsterHpText;

    [Header("VFX")]
    public ParticleSystem spawnEffect;

    public event Action<LevelConfig> OnLevelStarted;
    public event Action<MonsterWaveConfig, MonsterHealth> OnMonsterSpawned;
    public event Action<MonsterWaveConfig> OnWaveStarting;
    public event Action<LevelConfig> OnLevelCleared;

    private readonly Dictionary<int, GameObject> _prefabMap = new();
    private LevelConfig _currentLevel;
    private int _currentWaveIndex;
    private MonsterHealth _currentMonster;
    private bool _levelEnded;

    private void Awake()
    {
        CachePrefabMap();

        if (spawnPoint == null)
        {
            spawnPoint = FindSpawnPoint();
        }
    }

    private void Start()
    {
        if (useSelectedLevelFromMainMenu)
        {
            currentLevelId = PlayerPrefs.GetInt(MainMenuLevelSelector.SelectedLevelPrefsKey, currentLevelId);
        }

        StartLevel(currentLevelId);

        AudioManager.PlayBGM(Resources.Load<AudioClip>("Sound/bgm/GameSence"));
    }

    public void StartLevel(int levelId)
    {
        _currentLevel = LoadLevel(levelId);
        _currentWaveIndex = 0;
        _levelEnded = false;

        if (_currentLevel == null)
        {
            return;
        }

        if (spawnPoint == null)
        {
            return;
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        OnLevelStarted?.Invoke(_currentLevel);
        SpawnNextMonster();
    }

    private LevelConfig LoadLevel(int levelId)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(levelConfigResourcePath);
        if (textAsset == null)
        {
            return null;
        }

        string json = textAsset.text.Trim();
        if (json.StartsWith("["))
        {
            json = $"{{\"levels\":{json}}}";
        }

        json = RemoveTrailingCommas(json);

        LevelConfigCollection collection = JsonUtility.FromJson<LevelConfigCollection>(json);
        if (collection?.levels == null)
        {
            return null;
        }

        return collection.levels.Find(level => level.levelId == levelId);
    }

    private static string RemoveTrailingCommas(string json)
    {
        return System.Text.RegularExpressions.Regex.Replace(json, @",\s*(?=[}\]])", "");
    }

    private void SpawnNextMonster()
    {
        if (_levelEnded)
        {
            return;
        }

        if (_currentLevel.waves == null || _currentWaveIndex >= _currentLevel.waves.Count)
        {
            ClearLevel();
            return;
        }

        MonsterWaveConfig wave = _currentLevel.waves[_currentWaveIndex];
        GameObject prefab = GetMonsterPrefab(wave.enemyId);
        if (prefab == null)
        {
            return;
        }

        // 先弹出剩余敌人提示，延迟后再真正刷怪
        OnWaveStarting?.Invoke(wave);
        StartCoroutine(DelayedSpawn(wave, prefab));
    }

    private IEnumerator DelayedSpawn(MonsterWaveConfig wave, GameObject prefab)
    {
        yield return new WaitForSeconds(preSpawnDelay);

        GameObject monster = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnPoint.parent);
        _currentMonster = monster.GetComponent<MonsterHealth>();

        if (spawnEffect != null)
        {
            Vector3 effectPos = spawnPoint.position + new Vector3(0f, 0f, -1f);
            ParticleSystem effect = Instantiate(spawnEffect, effectPos, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetimeMultiplier);
        }
        if (_currentMonster == null)
        {
            Destroy(monster);
            yield break;
        }

        _currentMonster.OnHealthChanged += OnMonsterHealthChanged;
        _currentMonster.Configure(wave.hp, false);
        _currentMonster.OnDeath += HandleMonsterDeath;

        MonsterUI monsterUI = monster.GetComponent<MonsterUI>();
        if (monsterUI != null)
        {
            monsterUI.attackDamage = wave.damage;
        }

        _currentWaveIndex++;
        OnMonsterSpawned?.Invoke(wave, _currentMonster);
    }

    private GameObject GetMonsterPrefab(int enemyId)
    {
        if (_prefabMap.TryGetValue(enemyId, out GameObject prefab) && prefab != null)
        {
            return prefab;
        }

        string fallbackPath = enemyId switch
        {
            1 => "Prefab/Monster/ZhengMonster",
            2 => "Prefab/Monster/ManMonster",
            3 => "Prefab/Monster/ZhuYanMonster",
            _ => null
        };

        return string.IsNullOrEmpty(fallbackPath) ? null : Resources.Load<GameObject>(fallbackPath);
    }

    private void OnMonsterHealthChanged(int cur, int max)
    {
        if (monsterHpSlider != null)
        {
            monsterHpSlider.value = (float)cur / max;
        }
        if (monsterHpText != null)
        {
            monsterHpText.text = $"{cur}/{max}";
        }
    }

    private void HandleMonsterDeath()
    {
        BattleStats.EnemiesDefeated++;

        MonsterHealth deadMonster = _currentMonster;
        if (deadMonster != null)
        {
            deadMonster.OnDeath -= HandleMonsterDeath;
            deadMonster.OnHealthChanged -= OnMonsterHealthChanged;
        }

        StartCoroutine(SpawnNextMonsterAfterDeath(deadMonster));
    }

    private IEnumerator SpawnNextMonsterAfterDeath(MonsterHealth deadMonster)
    {
        yield return null;

        if (deadMonster != null)
        {
            Destroy(deadMonster.gameObject);
        }

        _currentMonster = null;
        SpawnNextMonster();
    }

    private void ClearLevel()
    {
        _levelEnded = true;

        // 记录关卡通关
        LevelProgressManager.CompleteLevel(currentLevelId);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (pauseGameWhenLevelCleared)
        {
            Time.timeScale = 0;
        }

        OnLevelCleared?.Invoke(_currentLevel);
    }

    private void CachePrefabMap()
    {
        _prefabMap.Clear();
        foreach (MonsterPrefabMapping mapping in monsterPrefabs)
        {
            if (mapping == null || mapping.prefab == null)
            {
                continue;
            }

            _prefabMap[mapping.enemyId] = mapping.prefab;
        }
    }

    private Transform FindSpawnPoint()
    {
        string[] names = { "spawnpoint", "SpawnPoint", "MonsterSpawnPoint" };
        foreach (string pointName in names)
        {
            GameObject point = GameObject.Find(pointName);
            if (point != null)
            {
                return point.transform;
            }
        }

        return null;
    }
}
