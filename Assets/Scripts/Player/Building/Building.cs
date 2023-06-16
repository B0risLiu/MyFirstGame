using UnityEngine;

public abstract class Building : DamageableObject
{
    [SerializeField] private int _price;
    [SerializeField] private float _portalEffectOffset;

    public int Price => _price;
    public float PortalEffectOffset => _portalEffectOffset;
}
