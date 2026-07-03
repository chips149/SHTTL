using TMPro;
using UnityEngine;

public class VictoryStatsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _marbleText;
    [SerializeField] private TMP_Text _itemText;
    [SerializeField] private TMP_Text _enemyText;

    private void OnEnable()
    {
        if (_marbleText != null)
            _marbleText.text = $"{BattleStats.MarbleSpawned}";
        if (_itemText != null)
            _itemText.text = $"{BattleStats.ItemsPlaced}";
        if (_enemyText != null)
            _enemyText.text = $"{BattleStats.EnemiesDefeated}";
    }
}
