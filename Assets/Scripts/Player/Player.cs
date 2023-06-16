using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

public class Player : DamageableObject
{
    [SerializeField] private int _deathCost;
    [SerializeField] private float _portalEffectUpOffsetMultipicator;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _leftHendWeapon;
    [SerializeField] private Transform _rightHendWeapon;
    [SerializeField] private Transform _shouldersWeapon;
    [SerializeField] private Transform _backboneWeapon;
    [SerializeField] private LandingPoint _landingPoint;
    [SerializeField] private TextField _playerInfoPanel;

    private List<Weapon> _installedWeapons = new List<Weapon>();
    private PlayerInput _input;
    private bool _isFireAndMoveModesOn;
    private bool _isPermanentDeathOn;

    public int ResourcesAmount { get; private set; }
    public float PortalEffectOffset => _portalEffectUpOffsetMultipicator;
    public bool IsFireAndMoveModesOn => _isFireAndMoveModesOn;

    public event UnityAction<int> ResourcesAmountChanged;
    public event UnityAction<bool> FireAndMoveModesEnabled; 
    public event UnityAction GameOver;
    public event UnityAction ResetPlayer;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Teleport.performed += ctx => OnTelepotrClick();
        _input.Player.SetLeandingPoint.performed += ctx => OnSetLandingPoint();
        _input.Player.ShootShouldersWeapon.performed += ctx => OnShootShouldersWeapon();
        _input.Player.ShootBackboneWeapon.performed += ctx => OnShootBackboneWeapon();
        _input.Player.ShootLeftWeapon.performed += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootLeftWeapon(false, false);
            else if (ctx.interaction is TapInteraction)
                OnShootLeftWeapon();
        };
        _input.Player.ShootRightWeapon.performed += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootRightWeapon(false, false);
            else if (ctx.interaction is TapInteraction)
                OnShootRightWeapon();
        };
        _input.Player.ShootLeftWeapon.canceled += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootLeftWeapon(false, false);
        };
        _input.Player.ShootRightWeapon.canceled += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootRightWeapon(false, false);
        };
        _input.Player.ShootLeftWeapon.started += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootLeftWeapon(false, true);
        };
        _input.Player.ShootRightWeapon.started += ctx =>
        {
            if (ctx.interaction is SlowTapInteraction)
                OnShootRightWeapon(false, true);
        };
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Start()
    {
        ResourcesAmountChanged?.Invoke(ResourcesAmount);
        FireAndMoveModesEnabled?.Invoke(_isFireAndMoveModesOn);
        ResetHealth(Health);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void LoadPlayerDefaultData(bool isPermanentDethOn, int resurcesAmount)
    {
        ResetHealth();
        _isPermanentDeathOn = isPermanentDethOn;
        ResourcesAmount = resurcesAmount;
    }

    public void LoadPlayerData(int playerCurentHealth, int playerResourceAmount, bool isPlayerFireAndMoveModesOn, 
        bool isPermanentDethOn, Vector3 position, Quaternion rotation, List<Weapon> playerInstalledWeapon)
    {
        ResetHealth(playerCurentHealth);
        FireAndMoveModesEnable(isPlayerFireAndMoveModesOn);
        _isPermanentDeathOn = isPermanentDethOn;
        ResourcesAmount = playerResourceAmount;
        transform.position = position;
        transform.rotation = rotation;

        foreach (Weapon weapon in playerInstalledWeapon)
            TryAddWeapon(weapon.Lable, weapon.Pleacement);
    }

    public void SavePlayerData(LoadingData loadingData)
    {
        loadingData.SavePlayerData(Health, ResourcesAmount, _isFireAndMoveModesOn, _isPermanentDeathOn, transform.position, transform.rotation, _installedWeapons);
    }

    public void ShowMessage(string text)
    {
        _playerInfoPanel.ShowText(text);
    }

    public bool TryAddWeapon(WeaponLable lable, WeaponPleacement pleacement)
    {
        bool result = false;

        switch (pleacement)
        {
            case WeaponPleacement.LeftHend:
                result = TryInstallWeapon(lable, _leftHendWeapon);
                break;
            case WeaponPleacement.RightHend:
                result = TryInstallWeapon(lable, _rightHendWeapon);
                break;
            case WeaponPleacement.Shoulders:
                result = TryInstallWeapon(lable, _shouldersWeapon);
                break;
            case WeaponPleacement.Backbone:
                result = TryInstallWeapon(lable, _backboneWeapon);
                break;
        }

        return result;
    }

    public void ChangeResourcesAmount(int number)
    {
        ResourcesAmount += number;
        ResourcesAmountChanged?.Invoke(ResourcesAmount);
    }

    public Weapon GetWeapon(WeaponPleacement pleacement)
    {
        return _installedWeapons.FirstOrDefault(weapon => weapon.Pleacement == pleacement);
    }

    public void UninstallWeapon(WeaponPleacement pleacement)
    {
        Weapon alreadyInstalledWeapon = GetWeapon(pleacement);

        if (alreadyInstalledWeapon != null)
        {
            _installedWeapons.Remove(alreadyInstalledWeapon);
            alreadyInstalledWeapon.gameObject.SetActive(false);
        }
    }

    public void FireAndMoveModesEnable(bool value)
    {
        OnShootRightWeapon(false, false);
        OnShootLeftWeapon(false, false);
        _isFireAndMoveModesOn = value;
        _mover.MoveModeEnable(value);
        FireAndMoveModesEnabled?.Invoke(value);
    }

    protected override void Die()
    {
        FireAndMoveModesEnable(false);

        if (ResourcesAmount >= _deathCost && _isPermanentDeathOn == false)
        {
            ChangeResourcesAmount(-_deathCost);
            ResetHealth();
            ResetPlayer?.Invoke();
            ShowMessage("������� �������� ����������� ����������� � ���� ������������� ���������� �� ����. ��������� �������� �� ������ - " + _deathCost);
        }
        else
            GameOver?.Invoke();
    }

    private bool TryInstallWeapon(WeaponLable lable, Transform transform)
    {
        var weapons = transform.GetComponentsInChildren<Weapon>(true);

        foreach (Weapon weapon in weapons)
        {
            if (weapon.Lable == lable)
            {
                weapon.gameObject.SetActive(true);
                _installedWeapons.Add(weapon);
                return true;
            }
        }

        return false;
    }

    private void OnShootShouldersWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = GetWeapon(WeaponPleacement.Shoulders);

        if (weapon != null && _isFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootBackboneWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = GetWeapon(WeaponPleacement.Backbone);

        if (weapon != null && _isFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootLeftWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = GetWeapon(WeaponPleacement.LeftHend);

        if (weapon != null && _isFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootRightWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = GetWeapon(WeaponPleacement.RightHend);

        if (weapon != null && _isFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnTelepotrClick()
    {
        FireAndMoveModesEnable(!_isFireAndMoveModesOn);
    }

    private void OnSetLandingPoint()
    {
        _landingPoint.SetNewPosition(transform.position);
    }
}
