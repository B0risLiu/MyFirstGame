using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MainBase))]
[RequireComponent(typeof(AudioSource))]

public class Teleport : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _basePortalPoint;
    [SerializeField] private LandingPoint _landingPoint;
    [SerializeField] private ParticleSystem _portalEffect;
    [SerializeField] private Animator _fadeTransitionAnimator;
    [SerializeField] private float _portalEffectDuration;
    [SerializeField] private AudioClip[] _portalSounds;

    private const string Fade = "Fade";

    private AudioSource _audioSource;
    private MainBase _mainBase;
    private PlayerInput _input;
    private FollowingCamera _camera;
    private Coroutine _workingCoroutine;
    private WaitForSeconds _delay;

    public event UnityAction<DamageableObject> BuldingTeleported;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Teleport.performed += ctr => OnTeleportClick();
        _delay = new WaitForSeconds(_portalEffectDuration);
        _camera = Camera.main.GetComponent<FollowingCamera>();
        _mainBase = GetComponent<MainBase>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _player.ResetPlayer += OnResetPlayer;
    }

    private void OnDisable()
    {
        _input.Disable();
        _player.ResetPlayer -= OnResetPlayer;
    }

    private void OnTeleportClick()
    {
        if (_workingCoroutine == null)
        {
            if (_mainBase.IsMainBasePanelActive)
                _workingCoroutine = StartCoroutine(SendPlayerToPlanet());
            else
                _workingCoroutine = StartCoroutine(SendPlayerToMainBase());
        }
    }

    public void SendBuildingToPosition(Building building, Vector3 position, bool isTeleportEffectOn)
    {
        if (_workingCoroutine == null && isTeleportEffectOn)
            _workingCoroutine = StartCoroutine(SendBuilding(building, position));
        else if (isTeleportEffectOn == false)
            SetBuilding(building, position);

        BuldingTeleported?.Invoke(building);
    }

    private IEnumerator SendPlayerToMainBase()
    {
        SetPortalEffectToPositionAndPlay(_player.transform.position + Vector3.up * _player.PortalEffectOffset);
        _camera.FollowingModeOn(false);
        _fadeTransitionAnimator.SetTrigger(Fade);
        yield return _delay;
        _player.transform.position = _basePortalPoint.position;
        _player.transform.rotation = _basePortalPoint.rotation;
        _mainBase.SetMainBasePanelActive(true);
        _camera.SetCameraToMainBase();
        _workingCoroutine = null;
    }

    private IEnumerator SendPlayerToPlanet()
    {
        _fadeTransitionAnimator.SetTrigger(Fade);
        yield return _delay;
        _mainBase.SetMainBasePanelActive(false);
        _camera.SetCameraToLandingPoint(_landingPoint.transform);
        SetPortalEffectToPositionAndPlay(_landingPoint.transform.position + Vector3.up * _player.PortalEffectOffset);
        yield return _delay;
        _player.transform.position = _landingPoint.transform.position;
        _camera.FollowingModeOn(true);
        _workingCoroutine = null;
    }

    private IEnumerator SendBuilding(Building building, Vector3 position)
    {
        SetPortalEffectToPositionAndPlay(position + Vector3.up * building.PortalEffectOffset);
        yield return _delay;
        SetBuilding(building, position);
        _workingCoroutine = null;
    }

    private void SetPortalEffectToPositionAndPlay(Vector3 newPosition)
    {
        _portalEffect.transform.position = newPosition;
        _portalEffect.Play();

        foreach (AudioClip audioClip in _portalSounds)
            _audioSource.PlayOneShot(audioClip);
    }

    private void SetBuilding(Building building, Vector3 position)
    {
        building.gameObject.SetActive(true);
        building.transform.position = position;
    }

    private void OnResetPlayer()
    {
        _workingCoroutine = StartCoroutine(SendPlayerToMainBase());
    }
}
