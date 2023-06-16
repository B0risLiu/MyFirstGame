using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class MoveState : State
{
    [SerializeField] private float _secondsBetweenUpdates;

    private NavMeshAgent _meshAgent;
    private Animator _animator;
    private Vector3 _curentTargetPosition;
    private Coroutine _workingCouroutine;
    private WaitForSeconds _delay;

    private const string IsStopped = "IsStopped";

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _delay = new WaitForSeconds(_secondsBetweenUpdates);
    }

    private void Update()
    {
        if (_workingCouroutine == null)
            _workingCouroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _curentTargetPosition = Target.transform.position;
        _meshAgent.SetDestination(_curentTargetPosition);
        _meshAgent.isStopped = false;
        _animator.SetBool(IsStopped, false);
        yield return _delay;
        _workingCouroutine = null;
    }
}
