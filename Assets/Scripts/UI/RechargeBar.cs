using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RechargeBar : MonoBehaviour
{
    [SerializeField] private RechargeableWeapon _weapon;
    [SerializeField] private GameObject _bar;
    [SerializeField] private Image _fillingArea;
    
    private Tween _workingTween;

    private void OnEnable()
    {
        _weapon.RechargeStarted += StartRecharge;
        _weapon.WeaponEnabled += RechargeBarEnabled;
    }

    private void OnDisable()
    {
        _weapon.RechargeStarted -= StartRecharge;
        _weapon.WeaponEnabled -= RechargeBarEnabled;
        _workingTween?.Kill();
    }

    public void StartRecharge(float time)
    {
        _fillingArea.fillAmount = 1;
        _workingTween = _fillingArea.DOFillAmount(0, time);
    }

    public void RechargeBarEnabled(bool value)
    {
        _workingTween?.Kill();
        _fillingArea.fillAmount = 0;
        _bar.SetActive(value);
    }
}
