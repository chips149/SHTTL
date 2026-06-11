using UnityEngine;

[CardProperty(2, "传送门","2D/Item_Other_ChuanSong_IMG/Item_Other_ChuanSong1_IMG", "触发球传到上方随机传送门里")]
public class PortalCardData : CardData
{
    private readonly GameObject _prefab = Resources.Load<GameObject>("Prefab/Item/Portal");
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