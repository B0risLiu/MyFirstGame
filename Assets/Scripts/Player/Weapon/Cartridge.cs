using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LifeTimer))]

public class Cartridge : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetToLocationAndActivate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }
}
