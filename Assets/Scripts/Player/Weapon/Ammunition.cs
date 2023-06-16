using UnityEngine;

public abstract class Ammunition : MonoBehaviour
{
    [SerializeField] protected int Damage;
    [SerializeField] protected float Spread;
    [SerializeField] protected float FlyingTime;
    [SerializeField] protected float MaxDistanse;

    abstract public void SetPositionAndActivate(Vector3 position, Quaternion rotation, Transform target);

    abstract public void AddCollicionEffectsPool(ObjectPool pool);
}
