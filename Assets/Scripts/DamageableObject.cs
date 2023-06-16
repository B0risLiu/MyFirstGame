using UnityEngine;
using UnityEngine.Events;

public abstract class DamageableObject : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] protected float CorpseTime;

    public int Health { get; private set; }
    public bool IsDead { get; protected set; }

    public event UnityAction<float> HealthChanged;
    public event UnityAction<DamageableObject> ObjectDied;

    public void TakeDamage(int damage)
    {
        if (IsDead == false)
        {
            Health -= damage;

            if (Health <= 0)
            {
                HealthChanged?.Invoke(0);
                IsDead = true;
                Die();
                ObjectDied?.Invoke(gameObject.GetComponent<DamageableObject>());
            }
            else
                HealthChanged?.Invoke((float)Health / _maxHealth);
        }
    }

    public void ResetHealth()
    {
        Health = _maxHealth;
        IsDead = false;
        HealthChanged?.Invoke(1);
    }

    public void ResetHealth(int health)
    {
        Health = health;
        IsDead = false;
        HealthChanged?.Invoke((float)Health / _maxHealth);
    }

    protected abstract void Die(); 
}
