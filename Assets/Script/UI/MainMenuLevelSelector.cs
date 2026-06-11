using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLevelSelector : MonoBehaviour
{
    public const string SelectedLevelPrefsKey = "SelectedLevelId";

    public static int SelectedLevelId { get; private set; }

    public int defaultLevelId;
    public string battleSceneName = "GameScene";

    public Transform levelButtonContainer;

    private void Awake()
    {
        SelectedLevelId = PlayerPrefs.GetInt(SelectedLevelPrefsKey, defaultLevelId);
    }

    private void Start()
    {
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        if (levelButtonContainer == null)
        {
            levelButtonContainer = transform.Find("LevelImage");
            if (levelButtonContainer == null) return;
        }

        for (int i = 0; i < levelButtonContainer.childCount; i++)
        {
            GameObject btnObj = levelButtonContainer.GetChild(i).gameObject;
            Button btn = btnObj.GetComponent<Button>();
            if (btn == null) continue;

            int levelId = ParseLevelId(btnObj.name);
            if (levelId < 0) continue;

            bool unlocked = LevelProgressManager.IsLevelUnlocked(levelId);
            btn.interactable = unlocked;
        }
    }

    private static int ParseLevelId(string name)
    {
        string digits = "";
        foreach (char c in name)
        {
            if (char.IsDigit(c))
                digits += c;
        }
        if (int.TryParse(digits, out int num))
            return num - 1; // "Level1" → 0
        return -1;
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
