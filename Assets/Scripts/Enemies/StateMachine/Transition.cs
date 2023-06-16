using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public State TargetState => _targetState;
    public bool NeedTranzit { get; protected set; }
    protected DamageableObject Target { get; private set; }

    private void OnEnable()
    {
        NeedTranzit = false;
    }

    public void Init(DamageableObject target)
    {
        Target = target;
    }
}
