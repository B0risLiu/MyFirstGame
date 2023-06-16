using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]

public class MeleeEnemy : Enemy
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] protected AttackingBodyPart AttackingBodyPart;
    [SerializeField] private AudioClip _successfulAttackSound;
    [SerializeField] private Collider[] _activeColliders;

    private const string Strike = "Strike";
    private const string Death = "Death";
    private const string Idle = "Idle";
    private const int AttackLayer = 1;

    private Animator _animator;
    private AudioSource _audioSource;
    private Collider _mainCollider;
    private Coroutine _workingCoroutine;
    private bool _isPlayerHitted;
    private bool _isPlayerTakeDamage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _mainCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        AttackingBodyPart.PlayerIsHitted += OnPlayerIsHitted;
        ResetEnemy();
    }

    private void OnDisable()
    {
        AttackingBodyPart.PlayerIsHitted -= OnPlayerIsHitted;
    }

    public override void AttackTarget(float attackTime)
    {
        if (_workingCoroutine == null)
        {
            _workingCoroutine = StartCoroutine(Attack(attackTime));
            _animator.SetTrigger(Strike);
        }
    }

    public override void Footstep()
    {
        AudioClip randomFootstep = Footsteps[Random.Range(0, Footsteps.Count)];
        _audioSource.PlayOneShot(randomFootstep);
    }

    protected override void Die()
    {
        StartCoroutine(PlayDead());
    }

    private IEnumerator PlayDead()
    {
        _animator.Play(Death);
        _animator.SetLayerWeight(AttackLayer, 0);
        _healthBar.gameObject.SetActive(false);
        BodyCollidersEnable(false);
        yield return new WaitForSeconds(CorpseTime);
        gameObject.SetActive(false);
    }

    private void ResetEnemy()
    {
        _animator.Play(Idle);
        _animator.SetLayerWeight(AttackLayer, 1);
        _healthBar.gameObject.SetActive(true);
        BodyCollidersEnable(true);
        ResetHealth();
    }

    private void BodyCollidersEnable(bool value)
    {
        _mainCollider.enabled = value;

        foreach (Collider collider in _activeColliders)
            collider.enabled = value;
    }

    private void OnPlayerIsHitted()
    {
        _isPlayerHitted = true;
    }

    private void ResetAttack()
    {
        _isPlayerTakeDamage = false;
        _isPlayerHitted = false;
        _workingCoroutine = null;
    }

    private IEnumerator Attack(float attackTime)
    {
        for (float i = 0; i < attackTime; i += Time.deltaTime)
        {
            if (_isPlayerHitted && _isPlayerTakeDamage == false)
            {
                Target.TakeDamage(Damage);
                _isPlayerTakeDamage = true;
                _audioSource.PlayOneShot(_successfulAttackSound);
            }
            yield return null;
        }

        ResetAttack();
    }
}
