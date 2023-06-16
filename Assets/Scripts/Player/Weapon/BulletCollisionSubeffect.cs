using UnityEngine;

public abstract class BulletCollisionSubeffect : MonoBehaviour
{
    public abstract void SetPositionAndActivate(Vector3 position, Quaternion rotation);
}
