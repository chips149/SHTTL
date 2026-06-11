using UnityEngine;

[CardProperty(5,"人参宝宝","2D/Item_Other_RenShen_IMG/Item_Other_RenShenBaoBao_IMG","触发球获得果实，若球触发攻击后，吃掉果实回血")]
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