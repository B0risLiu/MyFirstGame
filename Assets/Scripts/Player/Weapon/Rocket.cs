using DG.Tweening;
using UnityEngine;

public class Rocket : Ammunition
{
    [SerializeField] private float _maxFlyHeight;
    [SerializeField] private float _targetUpOffset;

    private ObjectPool _collisionEffect;
    private Tween _workingTween;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ammunition ammunition))
            return;
        else if (collision.gameObject.TryGetComponent(out DamageableObject actor))
            actor.TakeDamage(Damage);

        ActivateEffect();
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

    public override void SetPositionAndActivate(Vector3 position, Quaternion rotation, Transform target)
    {
        transform.SetPositionAndRotation(position, rotation);
        Vector3 spread = new Vector3(Random.Range(-Spread, Spread), _targetUpOffset, Random.Range(-Spread, Spread));
        Vector3 targetPosition = target.position + spread;
        gameObject.SetActive(true);
        _workingTween = transform.DOPath(GetPath(targetPosition), FlyingTime, PathType.CatmullRom).SetLookAt(0.05f);
    }

    private void ActivateEffect()
    {
        if (_collisionEffect.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out BulletCollisionEffect effect))
            effect.SetPositionAndActivate(transform.position, transform.rotation);
    }

    private Vector3[] GetPath(Vector3 targetPosition)
    {
        Vector3[] waypoints = new Vector3[3];
        Vector3 vectorToTarget = targetPosition - transform.position;
        waypoints[0] = transform.position + Vector3.up * _maxFlyHeight / 2;
        waypoints[1] = waypoints[0] + vectorToTarget / 2 + Vector3.up * _maxFlyHeight / 2;
        waypoints[2] = (waypoints[1] + Vector3.down * _maxFlyHeight) + (vectorToTarget - vectorToTarget / 2);
        return waypoints;
    }
}
