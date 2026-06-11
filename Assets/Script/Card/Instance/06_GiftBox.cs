using UnityEngine;

[CardProperty(6,"礼盒","2D/Item_Other_LiHe_IMG/Item_Other_LiHe_IMG","触发构筑生成3个\"彩蛋球\"，彩蛋球攻击伤害*2")]
public class GiftBoxCardData:CardData
{
    private readonly GameObject _prefab=Resources.Load<GameObject>("Prefab/Item/GiftBox");
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