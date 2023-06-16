using Assets.Scripts.Enum;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponLable _lable;
    [SerializeField] private WeaponPleacement _pleacement;
    [SerializeField] protected float DelayBetweenShots;
    [SerializeField] protected Transform Target;
    [SerializeField] protected ObjectPool AmmunitionPool;
    [SerializeField] protected ObjectPool GunFireEffect;

    public WeaponLable Lable => _lable;
    public WeaponPleacement Pleacement => _pleacement;

    public abstract void Shoot(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false);
}
