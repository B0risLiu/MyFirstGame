using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerShootController : MonoBehaviour
{
    [SerializeField] private Player _player;

    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
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
        _player.FireAndMoveModesEnabled += OnFireAndMoveModesEnabled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _player.FireAndMoveModesEnabled -= OnFireAndMoveModesEnabled;
    }

    private void OnShootShouldersWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = _player.GetWeapon(WeaponPleacement.Shoulders);

        if (weapon != null && _player.IsFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootBackboneWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = _player.GetWeapon(WeaponPleacement.Backbone);

        if (weapon != null && _player.IsFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootLeftWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = _player.GetWeapon(WeaponPleacement.LeftHend);

        if (weapon != null && _player.IsFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnShootRightWeapon(bool isSingleShotModeOn = true, bool isSpecialFireModeOn = false)
    {
        Weapon weapon = _player.GetWeapon(WeaponPleacement.RightHend);

        if (weapon != null && _player.IsFireAndMoveModesOn)
            weapon.Shoot(isSingleShotModeOn, isSpecialFireModeOn);
    }

    private void OnFireAndMoveModesEnabled(bool value)
    {
        OnShootRightWeapon(false, false);
        OnShootLeftWeapon(false, false);
    }
}
