using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLevelSelector : MonoBehaviour
{
    public const string SelectedLevelPrefsKey = "SelectedLevelId";

    public static int SelectedLevelId { get; private set; }

    [Header("基本设置")]
    public int defaultLevelId = 0;
    public string battleSceneName = "GameScene";
    public int pageSize = 5;

    [Header("UI引用")]
    public Transform levelButtonContainer;      // 存放关卡按钮的容器
    public Button prevPageBtn;
    public Button nextPageBtn;

    private int _currentPage;
    private int _totalPages = 1;
    private int _previousSelectedIndex = -1;

    // 并行列表，同一索引对应同一个按钮
    private readonly List<Button> _levelButtons = new List<Button>();
    private readonly List<Image> _levelImages = new List<Image>();
    private readonly List<int> _levelIds = new List<int>();
    private readonly List<Sprite> _normalSprites = new List<Sprite>();
    private readonly List<Sprite> _selectedSprites = new List<Sprite>();

    private void Awake()
    {
        SelectedLevelId = PlayerPrefs.GetInt(SelectedLevelPrefsKey, defaultLevelId);
    }

    private void Start()
    {
        CollectButtons();
        CalculateTotalPages();
        UpdateDisplay();
        ApplySelection();     // 初始化选中状态
        SetupDragHandler();

        if (prevPageBtn != null)
            prevPageBtn.onClick.AddListener(PrevPage);
        if (nextPageBtn != null)
            nextPageBtn.onClick.AddListener(NextPage);
    }


    private void CollectButtons()
    {
        _levelButtons.Clear();
        _levelImages.Clear();
        _levelIds.Clear();
        _normalSprites.Clear();
        _selectedSprites.Clear();

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

            Image img = btnObj.GetComponent<Image>();
            if (img == null) continue;

            int levelNum = levelId + 1;
            Sprite normal = Resources.Load<Sprite>($"UI/MainMenu/Level/{levelNum}关空");
            Sprite selected = Resources.Load<Sprite>($"UI/MainMenu/Level/{levelNum}关");

            _levelButtons.Add(btn);
            _levelImages.Add(img);
            _levelIds.Add(levelId);
            _normalSprites.Add(normal);
            _selectedSprites.Add(selected);
        }
    }

    private void CalculateTotalPages()
    {
        _totalPages = _levelButtons.Count > 0
            ? Mathf.CeilToInt((float)_levelButtons.Count / pageSize)
            : 1;
    }


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

        if (prevPageBtn != null)
            prevPageBtn.interactable = _currentPage > 0;
        if (nextPageBtn != null)
            nextPageBtn.interactable = _currentPage < _totalPages - 1;
    }

    public void NextPage()
    {
        if (_currentPage < _totalPages - 1)
        {
            _currentPage++;
            UpdateDisplay();
            ApplySelection();
        }
    }

    public void PrevPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdateDisplay();
            ApplySelection();
        }
    }

    // ========== 选中逻辑 ==========

    public void SelectLevel(int levelId)
    {
        int newIndex = _levelIds.IndexOf(levelId);
        if (newIndex < 0) return;

        if (_previousSelectedIndex >= 0 && _previousSelectedIndex < _levelImages.Count)
            SetImageSprite(_previousSelectedIndex, false);

        SetImageSprite(newIndex, true);
        _previousSelectedIndex = newIndex;

        SelectedLevelId = levelId;
        PlayerPrefs.SetInt(SelectedLevelPrefsKey, levelId);
        PlayerPrefs.Save();
    }

    private void ApplySelection()
    {
        int start = _currentPage * pageSize;
        int end = Mathf.Min(start + pageSize, _levelButtons.Count);

        _previousSelectedIndex = -1;

        for (int i = start; i < end; i++)
        {
            bool isSelected = _levelIds[i] == SelectedLevelId;
            SetImageSprite(i, isSelected);
            if (isSelected)
                _previousSelectedIndex = i;
        }
    }

    private void SetImageSprite(int index, bool selected)
    {
        if (index < 0 || index >= _levelImages.Count) return;
        _levelImages[index].sprite = selected ? _selectedSprites[index] : _normalSprites[index];
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
            return num - 1;
        return -1;
    }

    private void SetupDragHandler()
    {
        if (levelButtonContainer == null) return;

        if (levelButtonContainer.GetComponent<Graphic>() == null)
        {
            Image img = levelButtonContainer.gameObject.AddComponent<Image>();
            img.color = Color.clear;
        }

        LevelPageDragHandler handler = levelButtonContainer.GetComponent<LevelPageDragHandler>();
        if (handler == null)
            handler = levelButtonContainer.gameObject.AddComponent<LevelPageDragHandler>();

        handler.OnSwipeLeft = (data) => NextPage();
        handler.OnSwipeRight = (data) => PrevPage();
    }

    public void EnterBattle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(battleSceneName);
    }
}
