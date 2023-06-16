using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class DeathState : State
{
    private NavMeshAgent _meshAgent;
    private Animator _animator;

    private const string IsStopped = "IsStopped";

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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
