using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]

public class AttackState : State
{
    [SerializeField] private float _attackTime;

    private const string IsStopped = "IsStopped";

    private Enemy _enemy;
    private Animator _animator;
    private NavMeshAgent _meshAgent;
    private Coroutine _workingCoroutine;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<Animator>();
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_workingCoroutine == null)
        {
            _workingCoroutine = StartCoroutine(Attack());
            _meshAgent.isStopped = true;
            _animator.SetBool(IsStopped, true);
        }
    }

    private void OnDisable()
    {
        if ( _workingCoroutine != null)
        {
            StopCoroutine(_workingCoroutine);
            _workingCoroutine = null;
        }
    }

    private IEnumerator Attack()
    {
        var delay = new WaitForSeconds(_enemy.DelayBetweenAttacks + _attackTime);
        _enemy.AttackTarget(_attackTime);
        yield return delay;
        _workingCoroutine = null;
    }
}
