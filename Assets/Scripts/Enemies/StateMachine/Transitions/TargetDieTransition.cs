public class TargetDieTransition : Transition
{
    private void Update()
    {
        if (Target.IsDead)
            NeedTranzit = true;
    }
}
