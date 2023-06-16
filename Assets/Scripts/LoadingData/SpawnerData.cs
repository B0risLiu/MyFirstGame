using System.Collections.Generic;

public class SpawnerData
{
    public List<EnemyData> EnemiesData { get; private set; }

    public SpawnerData(List<EnemyData> enemiesData)
    {
        EnemiesData = enemiesData;
    }
}
