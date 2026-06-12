using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MainMenuLevelSelector : MonoBehaviour
{
    public const string SelectedLevelPrefsKey = "SelectedLevelId";

    public static int SelectedLevelId { get; private set; }

    [Header("基本设置")]
    public int defaultLevelId;
    public string battleSceneName = "GameScene";
    public int pageSize = 5;                    // 每页显示的关卡数

    [Header("UI引用")]
    public Transform levelButtonContainer;      // 存放关卡按钮的容器（默认找 "LevelImage"）
    public Button prevPageBtn;                  // 上一页按钮
    public Button nextPageBtn;                  // 下一页按钮

    private int _currentPage;
    private int _totalPages = 1;
    private List<Button> _levelButtons = new List<Button>();
    private List<int> _levelIds = new List<int>();

    private void Awake()
    {
        SelectedLevelId = PlayerPrefs.GetInt(SelectedLevelPrefsKey, defaultLevelId);
    }

    private void Start()
    {
        CollectButtons();
        CalculateTotalPages();
        UpdateDisplay();
        SetupDragHandler();

        if (prevPageBtn != null)
            prevPageBtn.onClick.AddListener(PrevPage);
        if (nextPageBtn != null)
            nextPageBtn.onClick.AddListener(NextPage);
    }

    /// <summary>
    /// 在关卡容器上自动添加拖拽处理器 + 射线检测 Graphic
    /// </summary>
    private void SetupDragHandler()
    {
        if (levelButtonContainer == null) return;

        // 容器必须有 Graphic（Image/RawImage等）才能接收 EventSystem 事件
        if (levelButtonContainer.GetComponent<Graphic>() == null)
        {
            Image img = levelButtonContainer.gameObject.AddComponent<Image>();
            img.color = Color.clear; // 透明，不影响视觉
        }

        // 获取或添加拖拽触发组件
        LevelPageDragHandler handler = levelButtonContainer.GetComponent<LevelPageDragHandler>();
        if (handler == null)
            handler = levelButtonContainer.gameObject.AddComponent<LevelPageDragHandler>();

        handler.OnSwipeLeft = (data) => NextPage();
        handler.OnSwipeRight = (data) => PrevPage();
    }

    /// <summary>
    /// 收集所有关卡按钮并解析对应的关卡ID
    /// </summary>
    private void CollectButtons()
    {
        _levelButtons.Clear();
        _levelIds.Clear();

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

            _levelButtons.Add(btn);
            _levelIds.Add(levelId);
        }
    }

    private void CalculateTotalPages()
    {
        _totalPages = _levelButtons.Count > 0
            ? Mathf.CeilToInt((float)_levelButtons.Count / pageSize)
            : 1;
    }

    /// <summary>
    /// 更新当前页的按钮显示与交互状态
    /// </summary>
    private void UpdateDisplay()
    {
        int start = _currentPage * pageSize;
        int end = Mathf.Min(start + pageSize, _levelButtons.Count);

        for (int i = 0; i < _levelButtons.Count; i++)
        {
            bool visible = i >= start && i < end;
            _levelButtons[i].gameObject.SetActive(visible);

            if (visible)
            {
                bool unlocked = LevelProgressManager.IsLevelUnlocked(_levelIds[i]);
                _levelButtons[i].interactable = unlocked;
            }
        }

        // 翻页按钮只有可翻时才可点击
        if (prevPageBtn != null)
            prevPageBtn.interactable = _currentPage > 0;
        if (nextPageBtn != null)
            nextPageBtn.interactable = _currentPage < _totalPages - 1;
    }

    // ========== 翻页 ==========

    public void NextPage()
    {
        if (_currentPage < _totalPages - 1)
        {
            _currentPage++;
            UpdateDisplay();
        }
    }

    public void PrevPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdateDisplay();
        }
    }

    // ========== 工具 ==========

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
