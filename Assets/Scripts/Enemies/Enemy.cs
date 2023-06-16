using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : DamageableObject
{
    [SerializeField] private EnemyName _name;
    [SerializeField] private int _damage;
    [SerializeField] private float _delayBetweenAttacks;
    [SerializeField] private float _visionRadius;
    [SerializeField] protected List<AudioClip> Footsteps;

    private List<DamageableObject> _possibleTargets = new();

    public int Damage => _damage;
    public float DelayBetweenAttacks => _delayBetweenAttacks;
    public EnemyName Name => _name;
    public DamageableObject Target { get; private set; }

    public abstract void AttackTarget(float attackTime);

    public abstract void Footstep();

    public void AddTarget(DamageableObject targets)
    {
        _possibleTargets.Add(targets);
    }

    public void RemoveTarget(DamageableObject target)
    {
        _possibleTargets.Remove(target);
    }

    public void SetTarget()
    {
        List<DamageableObject> targetsInVisionRadius = _possibleTargets.Where(target => Vector3.Distance(transform.position, target.transform.position) < _visionRadius).ToList();

        if (targetsInVisionRadius.Count > 0)
        {
            float minDistanse = targetsInVisionRadius.Min(target => Vector3.Distance(transform.position, target.transform.position));
            Target = targetsInVisionRadius.First(target => Vector3.Distance(transform.position, target.transform.position) == minDistanse);
        }
        else
        {
            foreach (DamageableObject target in _possibleTargets)
            {
                if (target.TryGetComponent(out Player player))
                    Target = target;
            }
        }
    }
}
