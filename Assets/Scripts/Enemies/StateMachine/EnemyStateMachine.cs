using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;

    private DamageableObject _target;

    public State CurentState { get; private set; }

    private void OnEnable()
    {
        _target = GetComponent<Enemy>().Target;
        CurentState = _firstState;
        CurentState.Enter(_target);
    }

    private void Update()
    {
        State nextState = CurentState.GetNextState();

        if (nextState != null)
            Transit(nextState);
    }

    private void OnDisable()
    {
        CurentState.Exit();
    }

    private void Transit(State nextState)
    {
        CurentState.Exit();
        CurentState = nextState;
        CurentState.Enter(_target);
    }
}
