using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    public TMP_Text waveText;
    public Slider waveSlider;

    private LevelMonsterSpawner _spawner;
    private int _totalWaves;
    private int _currentWave;

    private void Awake()
    {
        _spawner = FindObjectOfType<LevelMonsterSpawner>();
        _spawner.OnLevelStarted += OnLevelStarted;
        _spawner.OnMonsterSpawned += OnMonsterSpawned;
        _spawner.OnLevelCleared += OnLevelCleared;
    }

    private void OnLevelStarted(LevelConfig level)
    {
        _totalWaves = level.waves.Count;
        UpdateUI();
    }

    private void OnMonsterSpawned(MonsterWaveConfig _, MonsterHealth monster)
    {
        _currentWave++;
        UpdateUI();
    }

    private void OnLevelCleared(LevelConfig _)
    {
        waveSlider.value = 1f;
    }

    private void UpdateUI()
    {
        waveText.text = $"{_currentWave}/{_totalWaves}";
        waveSlider.value = (float)_currentWave / _totalWaves;
    }

    private void OnDestroy()
    {
        _spawner.OnLevelStarted -= OnLevelStarted;
        _spawner.OnMonsterSpawned -= OnMonsterSpawned;
        _spawner.OnLevelCleared -= OnLevelCleared;
    }
}
