using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    public Image remainImage;
    public TMP_Text remainText;
    public Button backButton;
    public string mainMenuSceneName = "MainMenuScene";

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

        if (backButton != null)
            backButton.onClick.AddListener(OnBackToMenu);
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
        int remaining = _totalWaves - _waveIndex;
        _waveIndex++;

        remainText.text = $"{remaining}";
        StartCoroutine(ShowAndHideAnimation());
    }

    private void OnBackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator ShowAndHideAnimation()
    {
        remainImage.transform.localScale = Vector3.zero;
        remainImage.gameObject.SetActive(true);

        float duration = 0.2f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float s = Mathf.SmoothStep(0, 1, t / duration);
            remainImage.transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        remainImage.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(textDisplayDuration);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float s = Mathf.SmoothStep(1, 0, t / duration);
            remainImage.transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        remainImage.transform.localScale = Vector3.zero;

        remainImage.gameObject.SetActive(false);
    }
}
