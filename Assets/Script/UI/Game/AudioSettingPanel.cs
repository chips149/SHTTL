using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂在 SettingPanel 上，将两个 Slider 连接到 AudioManager。
/// SoundSlider → 音效音量，VFXSlider → 背景音乐音量。
/// </summary>
public class AudioSettingPanel : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider;   // 背景音乐（VFXSlider）
    [SerializeField] private Slider _sfxSlider;   // 音效（SoundSlider）

    private void Start()
    {
        if (_bgmSlider != null)
        {
            _bgmSlider.value = PlayerPrefs.GetFloat(AudioManager.BgmVolKey, 0.8f);
            _bgmSlider.onValueChanged.AddListener(AudioManager.SetBgmVolume);
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SfxVolKey, 0.8f);
            _sfxSlider.onValueChanged.AddListener(AudioManager.SetSfxVolume);
        }
    }
}
