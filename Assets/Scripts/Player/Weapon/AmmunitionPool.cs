using UnityEngine;

public class AmmunitionPool : ObjectPool
{
    [SerializeField] private ObjectPool[] _bulletCollisionEffectPools;

    private void Start()
    {
        foreach (var item in Pool)
        {
            if (item.gameObject.TryGetComponent(out Ammunition ammunition))
            {
                foreach (var pool in _bulletCollisionEffectPools)
                    ammunition.AddCollicionEffectsPool(pool);
            }
        }
    }
}
