using UnityEngine;

public class BulletCollisionEffectPool : ObjectPool
{
    [SerializeField] private ObjectPool[] _bulletCollisionSubeffectPools;

    private void Start()
    {
        foreach (var item in Pool)
        {
            if (item.gameObject.TryGetComponent(out BulletCollisionEffect bulletCollicionEffect))
            {
                foreach (var pool in _bulletCollisionSubeffectPools)
                    bulletCollicionEffect.AddBulletCollisinSubeffectPool(pool);
            }
        }
    }
}
