using System.Collections.Generic;

public class TerrainData
{
    public List<int> DeadVegetationIDs { get; private set; }
    public List<EffectData> EffectsData { get; private set; }
    public Dictionary<int, int> ResourceDepositsIDAndAmount { get; private set; }

    public TerrainData(List<int> deadVegetationIDs, List<EffectData> effectsData, Dictionary<int, int> resourceDepositsIDAndAmount)
    {
        DeadVegetationIDs = deadVegetationIDs;
        EffectsData = effectsData;
        ResourceDepositsIDAndAmount = resourceDepositsIDAndAmount;
    }
}
