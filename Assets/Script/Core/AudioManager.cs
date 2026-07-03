using UnityEngine;

/// <summary>
/// 音频管理器，自动创建、跨场景持久化。
/// 提供 BGM 和 SFX 两路音量控制，音量值保存在 PlayerPrefs 中。
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    private float _bgmVolume = 0.8f;

    public const string BgmVolKey = "Audio_BgmVol";
    public const string SfxVolKey = "Audio_SfxVol";

    public static float SfxVolume { get; private set; } = 0.8f;

    private static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("[AudioManager]");
                _instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;

        _bgmVolume = PlayerPrefs.GetFloat(BgmVolKey, 0.8f);
        SfxVolume = PlayerPrefs.GetFloat(SfxVolKey, 0.8f);
        _bgmSource.volume = _bgmVolume;
    }

    // ========== 公共 API ==========

    /// <summary>设置背景音乐音量 (0~1)</summary>
    public static void SetBgmVolume(float vol)
    {
        var inst = Instance;
        inst._bgmVolume = vol;
        inst._bgmSource.volume = vol;
        PlayerPrefs.SetFloat(BgmVolKey, vol);
        PlayerPrefs.Save();
    }

    /// <summary>设置音效音量 (0~1)</summary>
    public static void SetSfxVolume(float vol)
    {
        SfxVolume = vol;
        PlayerPrefs.SetFloat(SfxVolKey, vol);
        PlayerPrefs.Save();
    }

    /// <summary>播放背景音乐（自动跳过相同 clip）</summary>
    public static void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        var inst = Instance;
        if (inst._bgmSource.clip == clip && inst._bgmSource.isPlaying) return;
        inst._bgmSource.clip = clip;
        inst._bgmSource.Play();
    }

    /// <summary>播放一次音效</summary>
    public static void PlaySFX(AudioClip clip)
    {
        if (clip == null || SfxVolume <= 0f) return;
        Instance._sfxSource.PlayOneShot(clip, SfxVolume);
    }
}
