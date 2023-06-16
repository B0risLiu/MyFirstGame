using UnityEngine;

public class NotInFrontOfTargetTransition : Transition
{
    [SerializeField] private float _angle;

    private void Update()
    {
        Vector3 vectorToTarget = (Target.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, vectorToTarget) >= _angle)
            NeedTranzit = true;
    }
}
