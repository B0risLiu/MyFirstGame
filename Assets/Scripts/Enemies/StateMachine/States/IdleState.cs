using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]

public class IdleState : State
{
    private NavMeshAgent _meshAgent;
    private Animator _animator;
    private Enemy _enemy;

    private const string IsStopped = "IsStopped";

    private void Awake()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.SetTarget();
    }

    private void Update()
    {
        if (_meshAgent.isStopped == false)
        {
            _meshAgent.isStopped = true;
            _animator.SetBool(IsStopped, true);
        }
    }
}
