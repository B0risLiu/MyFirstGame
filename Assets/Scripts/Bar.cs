using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public abstract class Bar : MonoBehaviour
{
    [SerializeField] protected Image FillingArea;
    [SerializeField] protected float BarChangingTime;

    protected Tween WorkingTween;

    public void OnValueChanged(float newValue)
    {
        WorkingTween?.Kill();
        WorkingTween = FillingArea.DOFillAmount(newValue, BarChangingTime);
    }
}
