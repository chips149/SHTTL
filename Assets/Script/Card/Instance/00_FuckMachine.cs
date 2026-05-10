using UnityEngine;

[CardProperty(0, "砰砰器", "2D/Item_Other_PengPengQi_IMG/Item_Other_PengPengQi1_IMG_DaiJi", "碰撞—将该球弹开")]
public class FuckMachineCardData : CardData
{
    private readonly GameObject _prefab = Resources.Load<GameObject>("Prefab/Item/FuckMachine");

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