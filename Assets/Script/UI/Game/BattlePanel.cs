using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    public Image remainImage;
    public TMP_Text remainText;

    public float textDisplayDuration = 1f;

    private LevelMonsterSpawner _spawner;
    private int _totalWaves;
    private int _waveIndex;

    private void Awake()
    {
        _spawner = FindFirstObjectByType<LevelMonsterSpawner>();
        if (_spawner == null) return;

        _spawner.OnLevelStarted += OnLevelStarted;
        _spawner.OnWaveStarting += OnWaveStarting;
    }

    private void OnDestroy()
    {
        if (_spawner == null) return;
        _spawner.OnLevelStarted -= OnLevelStarted;
        _spawner.OnWaveStarting -= OnWaveStarting;
    }

    private void OnLevelStarted(LevelConfig config)
    {
        _totalWaves = config.waves.Count;
        _waveIndex = 0;
    }

    private void OnWaveStarting(MonsterWaveConfig wave)
    {
        _waveIndex++;
        int remaining = _totalWaves - _waveIndex;


        remainText.text = $"{remaining}";
        remainImage.gameObject.SetActive(true);

        StartCoroutine(HideTextAfterDelay());
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(textDisplayDuration);
        remainImage.gameObject.SetActive(false);
    }
}
