using Assets.Scripts.Enum;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private Image _icon;

    private Transform _canvas;
    private Transform _parent;
    private WeaponPleacement _parentWeaponPlacement;
    private MainBase _mainBase;

    public WeaponCard WeaponCardInfo { get; private set; }

    public void Init(WeaponCard weaponCard, Transform canvas, MainBase mainBase, Transform parent, WeaponPleacement weaponPleacement)
    {
        WeaponCardInfo = weaponCard;
        _icon.sprite = weaponCard.Icon;
        _canvas = canvas;
        _mainBase = mainBase;
        _parent = parent;
        _parentWeaponPlacement = weaponPleacement;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_canvas, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
            transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.TryGetFirstComponentUnderPointer(eventData, out DropContainer dropContainer) && dropContainer.Location != _parent)
        {
            if (_mainBase.TryInstallWeapon(WeaponCardInfo.Label, dropContainer.WeaponPleacement, _parentWeaponPlacement, WeaponCardInfo.Price))
            {
                transform.SetParent(dropContainer.Location, false);
                _parent = dropContainer.Location;
                _parentWeaponPlacement = dropContainer.WeaponPleacement;
            }
            else
                transform.SetParent(_parent, false);
        }
        else
            transform.SetParent(_parent, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _mainBase.ShowInfo("Информация об оружии", WeaponCardInfo.Description + "\nЦена оружия - " + WeaponCardInfo.Price);
    }
}
