using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(AudioSource))]

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _chassisRotationSpeed;
    [SerializeField] private Rig _rig;
    [SerializeField] private List<AudioClip> _footsteps;

    private const string IsWalking = "IsWalking";
    private const string IsWalkingBack = "IsWalkingBack";

    private Rigidbody _rigidBodey;
    private Animator _animator;
    private AudioSource _audioSource;
    private PlayerInput _input;
    private bool _isPlayerCanMove;

    private void Awake()
    {
        _rigidBodey = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        if (_isPlayerCanMove)
        {
            Vector2 input = _input.Player.Move.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y);
            float angle = Vector3.Angle(transform.forward, direction);
            Move(direction, angle);
            RotateChassis(direction, angle);
        }
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void MoveModeEnable(bool value)
    {
        _isPlayerCanMove = value;
        _rig.weight = _isPlayerCanMove ? 1 : 0;

        if (_isPlayerCanMove == false && _animator != null)
            Move(Vector3.zero, 0);
    }

    public void Footstep()
    {
        AudioClip randomFootstep = _footsteps[Random.Range(0, _footsteps.Count)];
        _audioSource.PlayOneShot(randomFootstep);
    }

    private void Move(Vector3 direction, float angle)
    {
        if (direction == Vector3.zero)
        {
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsWalkingBack, false);
        }
        else
        {
            if (angle == 180)
                _animator.SetBool(IsWalkingBack, true);
            else
            {
                _animator.SetBool(IsWalkingBack, false);
                _animator.SetBool(IsWalking, true);
            }

            Vector3 offset = direction * _moveSpeed * Time.deltaTime;
            _rigidBodey.MovePosition(_rigidBodey.position + offset);
        }
    }

    private void RotateChassis(Vector3 direction, float angle)
    {
        if (angle > 0 && angle != 180)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _chassisRotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
