using UnityEngine;

public class DistanceMoreThenTransition : Transition
{
    [SerializeField] private float _range;

    private void Update()
    {
        if (Target.IsDead == false && Vector3.Distance(transform.position, Target.transform.position) >= _range)
            NeedTranzit = true;
    }
}
