using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLevelSelector : MonoBehaviour
{
    public const string SelectedLevelPrefsKey = "SelectedLevelId";

    public static int SelectedLevelId { get; private set; }

    public int defaultLevelId;

    public string battleSceneName = "GameScene";

    private void Awake()
    {
        SelectedLevelId = PlayerPrefs.GetInt(SelectedLevelPrefsKey, defaultLevelId);
    }

    public void SelectLevel(int levelId)
    {
        SelectedLevelId = levelId;
        PlayerPrefs.SetInt(SelectedLevelPrefsKey, levelId);
        PlayerPrefs.Save();
    }

    public void EnterBattle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(battleSceneName);
    }
}
