using UnityEngine;

[CardProperty(3, "弓箭","2D/Item_Other_GongJian_IMG/Item_Other_GongJian_IMG", "碰撞—每次碰撞该构筑，该构筑都会发射一个箭矢攻击敌人")]
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