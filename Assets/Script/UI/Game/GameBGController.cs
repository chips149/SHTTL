using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 控制 CanvasAfterScreen 中 BG 图片的切换。
/// 支持按关卡 ID 配置不同的背景图，拓展时只需在 Inspector 中添加映射即可。
/// </summary>
[RequireComponent(typeof(Image))]
public class GameBGController : MonoBehaviour
{
    [System.Serializable]
    public struct LevelBGSetting
    {
        public int levelId;       // 关卡 ID
        public Sprite bgSprite;   // 该关卡使用的背景图（null 表示保持默认）
    }

    [Header("关卡→背景映射")]
    [Tooltip("按关卡 ID 配置不同的背景图。未配置的关卡保持默认背景。")]
    public List<LevelBGSetting> levelBGSettings = new List<LevelBGSetting>();

    private Image _bgImage;

    void Awake()
    {
        _bgImage = GetComponent<Image>();
        ApplyBG(GameState.currentLevel);
    }

    /// <summary>
    /// 根据关卡 ID 切换背景图。支持运行时动态调用。
    /// </summary>
    public void ApplyBG(int levelId)
    {
        if (_bgImage == null) return;

        foreach (var setting in levelBGSettings)
        {
            if (setting.levelId == levelId && setting.bgSprite != null)
            {
                _bgImage.sprite = setting.bgSprite;
                return;
            }
        }

        // 未匹配到配置：保持 Inspector 中设置的默认背景
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器下预览指定关卡的背景效果（通过右键菜单调用）。
    /// </summary>
    [ContextMenu("应用第10关背景")]
    void PreviewLevel10() => ApplyBG(10);

    [ContextMenu("应用第15关背景")]
    void PreviewLevel15() => ApplyBG(15);

    [ContextMenu("重置为默认背景")]
    void ResetToDefault()
    {
        if (_bgImage == null) _bgImage = GetComponent<Image>();
        // 将 sprite 设回 SerializedProperty 中保存的原始值
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
