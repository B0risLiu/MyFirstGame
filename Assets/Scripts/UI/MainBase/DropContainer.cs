using Assets.Scripts.Enum;
using System.Collections.Generic;
using UnityEngine;

public class DropContainer : MonoBehaviour
{
    [SerializeField] private WeaponPleacement _weaponPleacement;
    [SerializeField] private Transform _containerLocation;

    public WeaponPleacement WeaponPleacement => _weaponPleacement;
    public Transform Location => _containerLocation;

    public List<T> GetContainerData<T>()
    {
        var result = new List<T>();
        _containerLocation.gameObject.GetComponentsInChildren(result);
        return result;
    }
}
