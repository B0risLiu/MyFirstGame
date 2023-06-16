using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class VegetationAnimationRandomizer : MonoBehaviour
{
    private Animator _animator;

    private const string Offset = "Offset";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(Offset, Random.Range(0, 1));
    }
}
