using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class InteractableVegetation : MonoBehaviour
{
    [SerializeField] private float _spead;
    [SerializeField] private bool _isGravityUsed;

    private Vector3 _startPosition;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isGravityUsed && collision.gameObject.TryGetComponent(out DamageableObject actor))
            _rigidbody.useGravity = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_isGravityUsed && collision.gameObject.TryGetComponent(out DamageableObject actor))
            _rigidbody.useGravity = false;
    }

    private void Update()
    {
        if (transform.position != _startPosition)
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, _spead * Time.deltaTime);
    }
}
