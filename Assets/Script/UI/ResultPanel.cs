using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenuScene";
    public string battleSceneName = "GameScene";

    public void OnContinue()
    {
        Time.timeScale = 1;

        LevelMonsterSpawner spawner = FindObjectOfType<LevelMonsterSpawner>();
        int nextLevelId = spawner.currentLevelId + 1;

        PlayerPrefs.SetInt(MainMenuLevelSelector.SelectedLevelPrefsKey, nextLevelId);
        PlayerPrefs.Save();

        SceneManager.LoadScene(battleSceneName);
    }

    public void OnRetry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(battleSceneName);
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
