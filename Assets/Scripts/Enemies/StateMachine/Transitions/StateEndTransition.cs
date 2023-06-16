using UnityEngine;

public class StateEndTransition : Transition
{
    [SerializeField] private State _curentState;

    private void Update()
    {
        if (_curentState.IsStateEnded)
            NeedTranzit = true;
    }
}
