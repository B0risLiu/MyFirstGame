using UnityEngine;

public abstract class Bullet : Ammunition
{
    protected Vector3 Direction;

    public override void SetPositionAndActivate(Vector3 position, Quaternion rotation, Transform target)
    {
        transform.SetPositionAndRotation(position, rotation);
        SetDirection(target);
        gameObject.SetActive(true);
    }

    protected void ActivateEffect(ObjectPool pool)
    {
        if (pool.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out BulletCollisionEffect effect))
            effect.SetPositionAndActivate(transform.position, transform.rotation);
    }

    private void SetDirection(Transform target)
    {
        Vector3 spread = new Vector3(Random.Range(-Spread, Spread), 0, Random.Range(-Spread, Spread));
        Direction = (target.position - transform.position).normalized * MaxDistanse + transform.position + spread;
    }
}
