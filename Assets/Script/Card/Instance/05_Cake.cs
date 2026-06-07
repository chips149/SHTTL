using UnityEngine;

[CardProperty(5,"人参宝宝","2D/Item_Other_RenShen_IMG/Item_Other_RenShenBaoBao_IMG","碰撞—碰撞后该球会携带一个果实，携带果实的球若触碰攻击类构筑，则会将该携带物发射到神兽嘴里吃掉，玩家恢复生命值")]
public class CakeCardData:CardData
{
    private readonly GameObject _prefab=Resources.Load<GameObject>("Prefab/Item/Cake");
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