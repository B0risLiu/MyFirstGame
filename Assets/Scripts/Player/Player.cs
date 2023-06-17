using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : DamageableObject
{
    [SerializeField] private int _deathCost;
    [SerializeField] private float _portalEffectUpOffsetMultipicator;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _leftHendWeapon;
    [SerializeField] private Transform _rightHendWeapon;
    [SerializeField] private Transform _shouldersWeapon;
    [SerializeField] private Transform _backboneWeapon;
    [SerializeField] private LandingPoint _landingPoint;
    [SerializeField] private TextField _playerInfoPanel;

    private List<Weapon> _installedWeapons = new List<Weapon>();
    private PlayerInput _input;
    private bool _isPermanentDeathOn;

    public int ResourcesAmount { get; private set; }
    public bool IsFireAndMoveModesOn { get; protected set; }
    public float PortalEffectOffset => _portalEffectUpOffsetMultipicator;

    public event UnityAction<int> ResourcesAmountChanged;
    public event UnityAction<bool> FireAndMoveModesEnabled; 
    public event UnityAction GameOver;
    public event UnityAction ResetPlayer;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Teleport.performed += ctx => OnTelepotrClick();
        _input.Player.SetLeandingPoint.performed += ctx => OnSetLandingPoint();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void Start()
    {
        ResourcesAmountChanged?.Invoke(ResourcesAmount);
        FireAndMoveModesEnabled?.Invoke(IsFireAndMoveModesOn);
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
        loadingData.SavePlayerData(Health, ResourcesAmount, IsFireAndMoveModesOn, _isPermanentDeathOn, transform.position, transform.rotation, _installedWeapons);
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
        IsFireAndMoveModesOn = value;
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
            ShowMessage("Техника получила критические повреждения и была автоматически возвращена на базу. Потрачено ресурсов на ремонт - " + _deathCost);
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

    private void OnTelepotrClick()
    {
        FireAndMoveModesEnable(!IsFireAndMoveModesOn);
    }

    private void OnSetLandingPoint()
    {
        _landingPoint.SetNewPosition(transform.position);
    }
}
