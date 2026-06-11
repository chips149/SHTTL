using UnityEngine;

[CardProperty(1, "弹绳","2D/Item_Other_TanSheng_IMG/Item_Other_TanSheng_IMG", "触发把球向上弹开")]
public class SkipRopeCardData : CardData
{
    private readonly GameObject _prefab = Resources.Load<GameObject>("Prefab/Item/SkipRope");

    public override void OnChosen()
    {
        GameState.Um.StartChosenColliderArea(area =>
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