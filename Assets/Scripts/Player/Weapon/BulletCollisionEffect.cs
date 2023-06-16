using Assets.Scripts.Enum;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeTimer))]

public class BulletCollisionEffect : MonoBehaviour
{
    [SerializeField] private MaterialName _collisionMaterial;

    private List<ObjectPool> _subeffects = new();

    public MaterialName CollisionMaterial => _collisionMaterial;

    public void SetPositionAndActivate(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);;

        foreach (ObjectPool pool in _subeffects)
        {
            if (pool.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out BulletCollisionSubeffect subeffect))
                subeffect.SetPositionAndActivate(position, rotation);
        }
    }

    public void AddBulletCollisinSubeffectPool(ObjectPool pool)
    {
        _subeffects.Add(pool);
    }
}
