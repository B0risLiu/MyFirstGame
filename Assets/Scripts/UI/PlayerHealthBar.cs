using DG.Tweening;
using UnityEngine;

public class PlayerHealthBar : Bar
{
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.HealthChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnValueChanged;
        WorkingTween?.Kill();
    }
}
