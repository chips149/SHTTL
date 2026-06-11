using UnityEngine;

public static class LevelProgressManager
{
    private const string CompletedPrefix = "LevelCompleted_";

    public static bool IsLevelUnlocked(int levelId)
    {
        if (levelId == 0) return true;
        return IsLevelCompleted(levelId - 1);
    }

    public static bool IsLevelCompleted(int levelId)
    {
        return PlayerPrefs.GetInt(CompletedPrefix + levelId, 0) == 1;
    }

    public static void CompleteLevel(int levelId)
    {
        int current = PlayerPrefs.GetInt(CompletedPrefix + levelId, 0);
        if (current == 1) return; // 已记录过

        PlayerPrefs.SetInt(CompletedPrefix + levelId, 1);
        PlayerPrefs.Save();

    }


}
