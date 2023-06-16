using UnityEngine;

public class Machinegun : FirearmWeapon
{
    [SerializeField] private int _bulletsPerShot;

    public override void Shoot(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        if (isSingleShotModeOn == true && isSpecialFireModeOn == false && WorkingCoroutine == null)
            WorkingCoroutine = StartCoroutine(Fire(_bulletsPerShot));
        else if (isSingleShotModeOn == false && isSpecialFireModeOn == true && WorkingCoroutine == null)
            WorkingCoroutine = StartCoroutine(Fire());
        else if (isSingleShotModeOn == false && isSpecialFireModeOn == false && WorkingCoroutine != null)
        {
            StopCoroutine(WorkingCoroutine);
            WorkingCoroutine = null;
        }
    }
}
