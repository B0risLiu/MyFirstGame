using DG.Tweening;
using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private DamageableObject _damageableObject;
    private Camera _camera;

    private void OnEnable()
    {
        _camera = Camera.main;
        _damageableObject.HealthChanged += OnValueChanged;
    }

    private void LateUpdate()
    {
        transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
    }

    private void OnDisable()
    {
        _damageableObject.HealthChanged -= OnValueChanged;
        WorkingTween?.Kill();
    }
}
