using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class RecoillessCannon : FirearmWeapon
{
    [SerializeField] private int _bulletsPerShot;

    public override void Shoot(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        if (isSingleShotModeOn && WorkingCoroutine == null)
            WorkingCoroutine = StartCoroutine(Fire(_bulletsPerShot));
    }
}
