using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [Header("场景")]
    public string mainMenuSceneName = "MainMenuScene";
    public string battleSceneName = "GameScene";

    /// <summary> 胜利界面的"下一关" </summary>
    public void OnContinue()
    {
        Time.timeScale = 1;

        LevelMonsterSpawner spawner = FindObjectOfType<LevelMonsterSpawner>();
        int nextLevelId = spawner.currentLevelId + 1;

        PlayerPrefs.SetInt(MainMenuLevelSelector.SelectedLevelPrefsKey, nextLevelId);
        PlayerPrefs.Save();

        SceneManager.LoadScene(battleSceneName);
    }

    /// <summary> 失败界面的"重来" </summary>
    public void OnRetry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(battleSceneName);
    }

    /// <summary> 两个面板共用的"返回主菜单" </summary>
    public void OnBackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
