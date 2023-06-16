using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Player))]

public class ResourceDetector : MonoBehaviour
{
    [SerializeField] private float _distanceOfFinding;
    [SerializeField] private float _maxDelayBetweenBeep;
    [SerializeField] private AudioClip _soundOfSearch;
    [SerializeField] private MainBase _mainBase;

    private bool _isSearching;
    private bool _isResourceFound;
    private float _resourceMaxDistance;
    private ResourceDeposit _resourceDeposit;
    private AudioSource _audioSource;
    private Coroutine _workingCoroutine;
    private Player _player;
    private PlayerInput _input;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
        _input = new PlayerInput();
        _input.Player.Action.performed += ctr => OnAction();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Update()
    {
        if (_isSearching && _workingCoroutine == null)
        {
            float distance = Vector3.Distance(transform.position, _resourceDeposit.transform.position);
            float delayMultiplicator = distance / _resourceMaxDistance;
            _workingCoroutine = StartCoroutine(Beep(new WaitForSeconds(_maxDelayBetweenBeep * delayMultiplicator + _soundOfSearch.length)));
            _isResourceFound = distance <= _distanceOfFinding ? true : false;

            if (_isResourceFound)
                _player.ShowMessage("������������� �������! ��� ��������� ����������� ��������� ������� F. ���������� �������� - " + _mainBase.ResourseCollectorPrice);
        }
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void StartSearch(ResourceDeposit resource)
    {
        _isSearching =true;
        _resourceDeposit = resource;
        _resourceMaxDistance = Vector3.Distance(transform.position, resource.transform.position);
        _player.ShowMessage("���������� �������. ��������� ���������� ������������ ��� ����� ������ �������� ���������.");
    }

    public void StopSearch()
    {
        _isSearching = false;
    }

    private void OnAction()
    {
        if (_isSearching && _isResourceFound && _mainBase.TryInstallCollector(_resourceDeposit.transform.position))
        {
            StopSearch();
            _player.ShowMessage("�������� ������� ��������!");
        }
        else if (_isSearching && _isResourceFound == false)
            _player.ShowMessage("���� �������� ���������. ��������� ����� � �������������.");
    }

    private IEnumerator Beep(WaitForSeconds delay)
    {
        _audioSource.PlayOneShot(_soundOfSearch);
        yield return delay;
        _workingCoroutine = null;
    }
}
