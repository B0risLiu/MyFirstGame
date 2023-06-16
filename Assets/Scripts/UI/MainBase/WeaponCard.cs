using Assets.Scripts.Enum;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponCard", menuName = "WeaponCards", order = 51)]

public class WeaponCard : ScriptableObject
{
    [SerializeField] private WeaponLable _label;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private WeaponView _viewTemplate;

    public WeaponLable Label => _label;
    public string Description => _description;
    public int Price => _price;
    public Sprite Icon => _icon;
    public WeaponView ViewTemplate => _viewTemplate;
}
