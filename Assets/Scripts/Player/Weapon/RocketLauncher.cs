using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RocketLauncher : RechargeableWeapon
{
    [SerializeField] private Transform[] _rocketsStartPositions;
    [SerializeField] private float _delayBetweenRocketsStarts;
    [Header("AudioSettings")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _fireSounds;

    private Coroutine _workingCoroutine;

    public override event UnityAction<float> RechargeStarted;
    public override event UnityAction<bool> WeaponEnabled;

    private void OnEnable()
    {
        WeaponEnabled?.Invoke(true);
    }

    private void Start()
    {
        WeaponEnabled?.Invoke(true);
    }

    private void OnDisable()
    {
        WeaponEnabled?.Invoke(false);

        if (_workingCoroutine != null)
        {
            StopCoroutine(_workingCoroutine);
            _workingCoroutine = null;
        }
    }

    public override void Shoot(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        if (isSingleShotModeOn && _workingCoroutine == null)
            _workingCoroutine = StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        for (int i = 0; i < _rocketsStartPositions.Length; i++)
        {
            if (AmmunitionPool.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out Ammunition ammunition))
                ammunition.SetPositionAndActivate(_rocketsStartPositions[i].position, _rocketsStartPositions[i].rotation, Target);

            _audioSource.PlayOneShot(_fireSounds[Random.Range(0, _fireSounds.Count)]);
            yield return new WaitForSeconds(_delayBetweenRocketsStarts);
        }

        _workingCoroutine = StartCoroutine(Recharge());
    }

    private IEnumerator Recharge()
    {
        RechargeStarted?.Invoke(DelayBetweenShots);
        yield return new WaitForSeconds(DelayBetweenShots);
        _workingCoroutine = null;
    }
}
