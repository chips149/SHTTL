/// <summary>
/// 单局战斗统计数据，关卡开始时自动清零。
/// </summary>
public static class BattleStats
{
    public static int MarbleSpawned { get; set; }
    public static int ItemsPlaced { get; set; }
    public static int EnemiesDefeated { get; set; }

    public static void Reset()
    {
        MarbleSpawned = 0;
        ItemsPlaced = 0;
        EnemiesDefeated = 0;
    }
}
