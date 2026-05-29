using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLevelSelector : MonoBehaviour
{
    public const string SelectedLevelPrefsKey = "SelectedLevelId";

    public static int SelectedLevelId { get; private set; }

    [Header("Default")]
    public int defaultLevelId;

    [Header("Scene")]
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
        Debug.Log($"已选择关卡：{levelId}");
    }

    public void EnterBattle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(battleSceneName);
    }
}
