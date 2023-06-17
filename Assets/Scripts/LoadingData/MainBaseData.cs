using System.Collections.Generic;
using UnityEngine;

public class MainBaseData
{
    public bool IsMainBasePanelActive { get; private set; }
    public WeaponCard RightHand { get; private set; }
    public WeaponCard LeftHand { get; private set; }
    public WeaponCard Backbone { get; private set; }
    public List<WeaponCard> WeaponStorage { get; private set; }
    public Dictionary<Vector3, int> ActiveResourceCollectorsPositionsWithHealth { get; private set; }

    public MainBaseData(bool isMainBasePanelActive, WeaponCard rightHand, WeaponCard leftHand, WeaponCard backbone, List<WeaponCard> weaponStorage, Dictionary<Vector3, int> activeResourceCollectorsPositionsWithHealth)
    {
        IsMainBasePanelActive = isMainBasePanelActive;
        RightHand = rightHand;
        LeftHand = leftHand;
        Backbone = backbone;
        WeaponStorage = weaponStorage;
        ActiveResourceCollectorsPositionsWithHealth = activeResourceCollectorsPositionsWithHealth;
    }
}
