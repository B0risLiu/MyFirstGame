using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions = new List<Transition>();

    public bool IsStateEnded { get; protected set; }
    protected DamageableObject Target { get; private set; }

    public void Enter(DamageableObject target)
    {
        if (enabled == false)
        {
            enabled = true;
            Target = target;

            foreach (var transition in _transitions)
            {
                transition.enabled = true;
                transition.Init(Target);
            }
        }
    }

    public void Exit()
    {
        if (enabled == true)
        {
            foreach (var transition in _transitions)
                transition.enabled = false;

            enabled = false;
        }
    }

    public State GetNextState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.NeedTranzit)
                return transition.TargetState;
        }

        return null;
    }
}
