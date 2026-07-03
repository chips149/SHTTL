using TMPro;
using UnityEngine;

/// <summary>
/// 挂在 VectoryPanel 上，关卡结算时显示统计数据。
/// 需要在 VectoryPanel 下创建三个 TMP_Text 子物体并拖入引用。
/// </summary>
public class VictoryStatsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _marbleText;
    [SerializeField] private TMP_Text _itemText;
    [SerializeField] private TMP_Text _enemyText;

    private void OnEnable()
    {
        if (_marbleText != null)
            _marbleText.text = $"生成的弹珠数: {BattleStats.MarbleSpawned}";
        if (_itemText != null)
            _itemText.text = $"安装的构筑数: {BattleStats.ItemsPlaced}";
        if (_enemyText != null)
            _enemyText.text = $"击败的敌人: {BattleStats.EnemiesDefeated}";
    }
}
