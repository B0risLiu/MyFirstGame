using UnityEngine;

public class AmbientSwitcher : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _planetAmbient;
    [SerializeField] private AudioSource _baseAmbient;

    private void OnEnable()
    {
        _player.FireAndMoveModesEnabled += OnFireAndMoveModesEnabled;
    }

    private void OnDisable()
    {
        _player.FireAndMoveModesEnabled -= OnFireAndMoveModesEnabled;
    }

    private void OnFireAndMoveModesEnabled(bool value)
    {
        if (value == false)
        {
            _baseAmbient.Play();
            _planetAmbient.Stop();
        }
        else
        {
            _baseAmbient.Stop();
            _planetAmbient.Play();
        }
    }
}
