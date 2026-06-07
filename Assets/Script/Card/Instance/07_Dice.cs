using Unity.VisualScripting;
using UnityEngine;

[CardProperty(7, "骰子", "2D/Item_Other_TouZi_IMG/IMG_6108", "碰撞—每次碰撞该构筑，都会随机在出球口落下1-6个球")]
public class DiceData : CardData
{
    private readonly GameObject _prefab = Resources.Load<GameObject>("Prefab/Item/Dice");
    public override void OnChosen()
    {
       GameState.Um.StartChosenArea(area =>
       {
           var pos = area.transform.position;
           Object.Instantiate(_prefab, pos, Quaternion.identity);
           Object.Destroy(area.gameObject);
           
           GameState.Bm.NewTurn();
       }); 
    }
}