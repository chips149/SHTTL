using UnityEngine;

[CardProperty(3, "弓箭","2D/Item_Other_GongJian_IMG/Item_Other_GongJian_IMG", "触发构筑发射箭矢攻击")]
public class  ArrowCardData: CardData
{
    private readonly GameObject _prefab=Resources.Load<GameObject>("Prefab/Item/Arrow");
    public override void OnChosen()
    {
        GameState.Um.StartChosenArea(area =>
        {
            // 创建 prefab
            var pos = area.transform.position;
            Object.Instantiate(_prefab, pos, Quaternion.identity);
            Object.Destroy(area.gameObject);

            // 结束
            GameState.Bm.NewTurn();
        }); 
    }
}