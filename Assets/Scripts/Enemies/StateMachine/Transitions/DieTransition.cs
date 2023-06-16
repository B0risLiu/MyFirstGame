using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class DieTransition : Transition
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_enemy.IsDead)
            NeedTranzit = true;
    }
}
