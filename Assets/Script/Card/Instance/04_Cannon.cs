using UnityEngine;

[CardProperty(4, "大炮", "2D/Item_Other_DaPao_IMG/Item_Other_DaPao_IMG", "触发构筑发射炮弹攻击并击退")]
public class ConnonCardData : CardData
{
    private readonly GameObject _prefab=Resources.Load<GameObject>("Prefab/Item/Cannon");
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
