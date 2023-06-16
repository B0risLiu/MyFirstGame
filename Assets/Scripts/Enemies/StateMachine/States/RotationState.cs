using System.Collections;
using UnityEngine;

public class RotationState : State
{
    [SerializeField] private float _speed;

    private Coroutine _workingCoroutine;

    private void Update()
    {
        if (_workingCoroutine == null)
            _workingCoroutine = StartCoroutine(Rotate());
    }

    private void OnDisable()
    {
        IsStateEnded = false;
    }

    private IEnumerator Rotate()
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, _speed, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
        yield return null;
        IsStateEnded = true;
        _workingCoroutine = null;
    }
}
