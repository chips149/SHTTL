using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScene : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenuScene";

    private void Start()
    {
        AudioManager.PlayBGM(Resources.Load<AudioClip>("Sound/bgm/背景音乐"));
    }

    /// <summary>给"开始游戏"按钮绑定此方法</summary>
    public void OnStartGame()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
