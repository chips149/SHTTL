using Framework;
using UnityEngine;
using UnityEngine.UI;

public class CardViewer : MonoBehaviour
{
    private DrawCardPanel _drawCardPanel;
    private int _index;

    private CardData _cardData;

    public Image img;
    public Text description;

    private Button _btn;

    private UserAreaManager _um;


    public void Initialize(DrawCardPanel drawCardPanel, int index, CardData data)
    {
        _um = ModulesManager.Get<UserAreaManager>();
        // 显示卡片相关
        _drawCardPanel = drawCardPanel;
        _index = index;
        _cardData = data;
        
        description.text = _cardData.description;

        Sprite sprite = Resources.Load<Sprite>(_cardData.imgPath);
        img.sprite = sprite; 


        // 组件组合
        _btn = GetComponent<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClick);
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // 卡片生效
    private void OnClick()
    {
        _drawCardPanel.CloseDrawCardPanel();
        _cardData.OnChosen();
    }
}


