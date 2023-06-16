using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int CurentHealth { get; private set; }
    public int ResourceAmount { get; private set; }
    public bool IsPlayerFireAndMoveModesOn { get; private set; }
    public bool IsPermanentDethOn { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public List<Weapon> InstalledWeapon { get; private set; }

    public PlayerData(int playerCurentHealth, int playerResourceAmount, bool isPlayerFireAndMoveModesOn, bool isPermanentDethOn, Vector3 position, Quaternion rotation, List<Weapon> playerInstalledWeapon)
    {
        CurentHealth = playerCurentHealth;
        ResourceAmount = playerResourceAmount;
        IsPlayerFireAndMoveModesOn = isPlayerFireAndMoveModesOn;
        IsPermanentDethOn = isPermanentDethOn;
        Position = position;
        Rotation = rotation;
        InstalledWeapon = playerInstalledWeapon;
    }
}
