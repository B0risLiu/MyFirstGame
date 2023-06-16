using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableVegetation : DamageableObject
{
    [SerializeField] private bool _isVegetationDestroyedAfterCollision;
    [SerializeField] private ParticleSystem _deathEffect;
    [Header("Meshes")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Parts Settings")]
    [SerializeField] private int _minPartsAmount;
    [SerializeField] private int _maxPartsAmount;
    [SerializeField] private float _partsUpOffset;
    [SerializeField] private List<ObjectPool> patrsPool;
    
    private WaitForSeconds _delay = new WaitForSeconds(3);
    private Coroutine _workingCouroutine;
    
    private void Awake()
    {
        ResetHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isVegetationDestroyedAfterCollision && collision.gameObject.TryGetComponent(out DamageableObject actor))
            TakeDamage(Health);
    }

    private void OnDisable()
    {
        if (_workingCouroutine != null)
            StopCoroutine(_workingCouroutine);
    }

    public void Kill()
    {
        IsDead = true;
    }

    protected override void Die()
    {
        if (_deathEffect != null)
            _deathEffect.Play();

        SetParts();
        _workingCouroutine = StartCoroutine(DisactivateWithDelay());
    }

    private void SetParts()
    {
        foreach (ObjectPool pool in patrsPool)
        {
            int partsAmount = Random.Range(_minPartsAmount, _maxPartsAmount);

            for (int i = 0; i < partsAmount; i++)
            {
                if (pool.TryGetObject(out GameObject poolObject))
                {
                    poolObject.transform.position = transform.position + Vector3.up * _partsUpOffset;
                    poolObject.transform.rotation = Random.rotation;
                    poolObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator DisactivateWithDelay()
    {
        ActiveMeshEnabled(false);
        yield return _delay;
        ActiveMeshEnabled(true);
        _workingCouroutine = null;
        gameObject.SetActive(false);
    }

    private void ActiveMeshEnabled(bool value)
    {
        if (_meshRenderer != null)
            _meshRenderer.enabled = value;
        if (_skinnedMeshRenderer != null)
            _skinnedMeshRenderer.enabled = value;
    }
}
