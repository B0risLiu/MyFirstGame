using Assets.Scripts.Enum;
using UnityEngine;

public class EffectData
{
    public WeaponLable WeaponWhichHasBeenShooted { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public TerrainChangeData TerrainChangeData { get; private set; }

    public EffectData(WeaponLable weaponWhichHasBeenShooted, Vector3 position, Quaternion rotation, TerrainChangeData terrainChangeData)
    {
        WeaponWhichHasBeenShooted = weaponWhichHasBeenShooted;
        Position = position;
        Rotation = rotation;
        TerrainChangeData = terrainChangeData;
    }
}
