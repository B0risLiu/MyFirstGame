using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FirearmWeapon : Weapon
{
    [SerializeField] private ObjectPool _cartridgePool;
    [SerializeField] private float _cartridgeUpOffsetMultiplicator;
    [Header("AudioSettings")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _fireSounds;

    protected Coroutine WorkingCoroutine;

    protected IEnumerator Fire(int bulletsAmount = Int32.MaxValue)
    {
        var pause = new WaitForSeconds(DelayBetweenShots);

        for (int i = 0; i < bulletsAmount; i++)
        {
            GameObject poolObject;

            if (_cartridgePool.TryGetObject(out poolObject) && poolObject.TryGetComponent(out Cartridge cartridge))
                cartridge.SetToLocationAndActivate(transform.position + transform.up * _cartridgeUpOffsetMultiplicator, transform.rotation);

            if (AmmunitionPool.TryGetObject(out poolObject) && poolObject.TryGetComponent(out Ammunition ammunition))
                ammunition.SetPositionAndActivate(transform.position + transform.forward, transform.rotation, Target);

            if (GunFireEffect.TryGetObject(out poolObject))
                poolObject.SetActive(true);

            _audioSource.PlayOneShot(_fireSounds[UnityEngine.Random.Range(0, _fireSounds.Count)]);
            yield return pause;
        }

        WorkingCoroutine = null;
    }
}
