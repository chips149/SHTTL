using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 新手教程面板，只会在第一关显示。
/// 3 张教程图片依次切换，点击进入下一张，看完最后一张关闭。
/// </summary>
public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Image[] _tutorialImages;
    private int _currentIndex;
    private LevelMonsterSpawner _spawner;

    private void Awake()
    {
        SetAllImages(false);
        gameObject.SetActive(false);

        _spawner = FindFirstObjectByType<LevelMonsterSpawner>();
        if (_spawner != null)
            _spawner.OnLevelStarted += OnLevelStarted;
    }

    private void OnDestroy()
    {
        if (_spawner != null)
            _spawner.OnLevelStarted -= OnLevelStarted;
    }

    private void OnLevelStarted(LevelConfig config)
    {
        if (config.levelId != 0) return;

        _currentIndex = 0;
        ShowCurrentImage();
        gameObject.SetActive(true);
    }

    /// <summary>点击切换到下一张</summary>
    public void Next()
    {
        _tutorialImages[_currentIndex].gameObject.SetActive(false);

        _currentIndex++;
        if (_currentIndex >= _tutorialImages.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        ShowCurrentImage();
    }

    private void ShowCurrentImage()
    {
        for (int i = 0; i < _tutorialImages.Length; i++)
            _tutorialImages[i].gameObject.SetActive(i == _currentIndex);
    }

    private void SetAllImages(bool active)
    {
        foreach (var img in _tutorialImages)
        {
            if (img != null)
                img.gameObject.SetActive(active);
        }
    }
}
