using UnityEngine.Events;

public abstract class RechargeableWeapon : Weapon
{
    public abstract event UnityAction<float> RechargeStarted;
    public abstract event UnityAction<bool> WeaponEnabled;
}
