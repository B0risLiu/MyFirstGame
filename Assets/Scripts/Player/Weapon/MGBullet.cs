using Assets.Scripts.Enum;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LifeTimer))]

public class MGBullet : Bullet
{
    private ObjectPool _bulletFleshCollisionEffect;
    private ObjectPool _bulletStoneCollisionnEffect;
    private ObjectPool _bulletWoodCollisionnEffect;
    private ObjectPool _bulletMetalCollisionEffect;

    private Tween _workingTween;

    private void OnEnable()
    {
        _workingTween = transform.DOMove(Direction, FlyingTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ammunition ammunition))
            return;
        else if (collision.gameObject.TryGetComponent(out DamageableObject actor))
            actor.TakeDamage(Damage);

        ChooseAndActivateEffect(collision);        
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _workingTween?.Kill();
    }

    public override void AddCollicionEffectsPool(ObjectPool pool)
    {
        if (pool.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out BulletCollisionEffect effect))
        {
            switch (effect.CollisionMaterial)
            {
                case MaterialName.Flesh:
                    _bulletFleshCollisionEffect = pool;
                    break;
                case MaterialName.Stone:
                    _bulletStoneCollisionnEffect = pool;
                    break;
                case MaterialName.Wood:
                    _bulletWoodCollisionnEffect = pool;
                    break;
                case MaterialName.Metal:
                    _bulletMetalCollisionEffect = pool;
                    break;
            }
        }
    }

    private void ChooseAndActivateEffect(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CollisionMaterial material))
        {
            switch (material.Name)
            {
                case MaterialName.Flesh:
                    ActivateEffect(_bulletFleshCollisionEffect);
                    break;
                case MaterialName.Stone:
                    ActivateEffect(_bulletStoneCollisionnEffect);
                    break;
                case MaterialName.Wood:
                    ActivateEffect(_bulletWoodCollisionnEffect);
                    break;
                case MaterialName.Metal:
                    ActivateEffect(_bulletMetalCollisionEffect);
                    break;
            }
        }
    }
}
