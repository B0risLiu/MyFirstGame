using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LifeTimer : MonoBehaviour
{
    [SerializeField] private float _lifetime;
    [SerializeField] private bool _isNeedToDisactivateOutOfCameraView;
    [SerializeField] private bool _isNeedToRemoveFromTerrain;

    private const int _deactivationDistance = 90;

    public event UnityAction LifetimeEnd;

    private void OnEnable()
    {
        if (_isNeedToDisactivateOutOfCameraView)
            StartCoroutine(DisactivateOutOfCameraView());
        else
            StartCoroutine(DisactivateWithPause(new WaitForSeconds(_lifetime)));
    }

    private IEnumerator DisactivateWithPause(WaitForSeconds pause)
    {
        yield return pause;
        LifetimeEnd?.Invoke();
        gameObject.SetActive(false);

        if(_isNeedToRemoveFromTerrain)
            transform.position = Vector3.zero;
    }

    private IEnumerator DisactivateOutOfCameraView()
    {
        while (Vector3.Distance(transform.position, Camera.main.transform.position) < _deactivationDistance)
            yield return null;

        StartCoroutine(DisactivateWithPause(null));
    }
}
