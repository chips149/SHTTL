using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardViewer : MonoBehaviour
{
    private static AudioClip _chooseCardSfx;
    private static AudioClip ChooseCardSfx => _chooseCardSfx ??= Resources.Load<AudioClip>("Sound/SFX/ChooseCard");
    private DrawCardPanel _drawCardPanel;
    private int _index;

    private CardData _cardData;

    public Image img;
    public Text description;
    public Text nameText;
    public TMP_Text cooldownText;

    private Button _btn;

    private UserAreaManager _um;


    public void Initialize(DrawCardPanel drawCardPanel, int index, CardData data)
    {
        _um = ModulesManager.Get<UserAreaManager>();
        _drawCardPanel = drawCardPanel;
        _index = index;
        _cardData = data;

        nameText.text = _cardData.name;
        description.text = _cardData.description;

        Sprite sprite = Resources.Load<Sprite>(_cardData.imgPath);
        img.sprite = sprite;

        ShowCooldown();

        _btn = GetComponent<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClick);
    }

    private void ShowCooldown()
    {
        if (cooldownText == null) return;

        cooldownText.text = $"{GetCardCoolTime(_cardData.id)}s";
        cooldownText.gameObject.SetActive(true);
    }

    private static float GetCardCoolTime(int cardId)
    {
        string prefabPath = cardId switch
        {
            0 => "Prefab/Item/FuckMachine",
            2 => "Prefab/Item/Portal",
            3 => "Prefab/Item/Arrow",
            4 => "Prefab/Item/Cannon",
            5 => "Prefab/Item/Cake",
            6 => "Prefab/Item/GiftBox",
            7 => "Prefab/Item/Dice",
            _ => null,
        };

        if (prefabPath == null) return 0;

        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null) return 0;

        Cooldown cd = prefab.GetComponent<Cooldown>();
        return cd != null ? cd.CoolTime : 0;
    }

    private void OnClick()
    {
        AudioManager.PlaySFX(ChooseCardSfx);
        _drawCardPanel.CloseDrawCardPanel();
        _cardData.OnChosen();
    }
}
