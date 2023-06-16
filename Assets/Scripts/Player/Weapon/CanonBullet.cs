using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LifeTimer))]

public class CanonBullet : Bullet
{
    private ObjectPool _collisionEffect;

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

        ActivateEffect(_collisionEffect);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _workingTween?.Kill();
    }

    public override void AddCollicionEffectsPool(ObjectPool pool)
    {
        _collisionEffect = pool;
    }
}
