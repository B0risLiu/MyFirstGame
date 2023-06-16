using UnityEngine;

public class TerrainChangeData
{
    public Vector2 TerrainPosition { get; private set; }
    public int[,] ChangedDensityMap { get; private set; }

    public TerrainChangeData(Vector2 terrainPosition, int[,] changedDensityMap)
    {
        TerrainPosition = terrainPosition;
        ChangedDensityMap = changedDensityMap;
    }
}
