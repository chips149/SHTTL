using UnityEngine;

public class PrivacyPolicyPanel : MonoBehaviour
{
    /// <summary>拒绝按钮调用，退出游戏</summary>
    public void Reject()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
