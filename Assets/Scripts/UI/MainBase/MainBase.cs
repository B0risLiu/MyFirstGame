using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MainBase : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Teleport _teleport;
    [SerializeField] private GameObject _mainBasePanel;
    [SerializeField] private TextField _lable;
    [SerializeField] private TextField _description;
    [SerializeField] private DropContainer _mainBaseWeaponContainer;
    [SerializeField] private DropContainer _leftHandContainer;
    [SerializeField] private DropContainer _rightHandContainer;
    [SerializeField] private Transform _buildingsContainer;
    [SerializeField] private ResourceCollector _collectorTemplate;

    [SerializeField] private List<WeaponCard> _weaponCards = new();
    [SerializeField] private List<AudioClip> _weaponIstallSounds = new();

    private AudioSource _audioSource;

    public int ResourseCollectorPrice => _collectorTemplate.Price;
    public bool IsMainBasePanelActive => _mainBasePanel.activeSelf;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetMainBasePanelActive(bool value)
    {
        _mainBasePanel.SetActive(value);
    }

    public void LoadMainBaseDefaultData()
    {
        foreach (var weaponCard in _weaponCards)
            AddWeapon(weaponCard, _mainBaseWeaponContainer, WeaponPleacement.Base);
    }

    public void LoadMainBaseData(bool isMainBasePanelActive, WeaponCard rightHand, WeaponCard leftHand, List<WeaponCard> weaponStorage, Dictionary<Vector3, int> activeResourceCollectorsPositionsWithHealth)
    {
        if (leftHand != null)
            AddWeapon(leftHand, _leftHandContainer, WeaponPleacement.LeftHend);

        if (rightHand != null)
            AddWeapon(rightHand, _rightHandContainer, WeaponPleacement.RightHend);

        foreach (WeaponCard weapon in weaponStorage)
            AddWeapon(weapon, _mainBaseWeaponContainer, WeaponPleacement.Base);

        foreach (var pair in activeResourceCollectorsPositionsWithHealth)
        {
            ResourceCollector collector = Instantiate(_collectorTemplate, _buildingsContainer);
            collector.SetOwner(_player);
            collector.ResetHealth(pair.Value);
            _teleport.SendBuildingToPosition(collector, pair.Key, false);
        }

        SetMainBasePanelActive(isMainBasePanelActive);
    }

    public void SaveMainBaseData(LoadingData loadingData)
    {
        WeaponCard lefthand = _leftHandContainer.GetContainerData<WeaponView>().FirstOrDefault()?.WeaponCardInfo;
        WeaponCard rightHand = _rightHandContainer.GetContainerData<WeaponView>().FirstOrDefault()?.WeaponCardInfo;
        List<WeaponCard> weaponStorage = _mainBaseWeaponContainer.GetContainerData<WeaponView>().Select(viev => viev.WeaponCardInfo).ToList();
        Dictionary<Vector3, int> activeResourceDeposits = GetActiveResourceCollectorsPositionsWithHealth();
        loadingData.SaveMainBaseData(_mainBasePanel.activeSelf, rightHand, lefthand, weaponStorage, activeResourceDeposits);
    }

    public bool TryInstallWeapon(WeaponLable weapon, WeaponPleacement newPleacement, WeaponPleacement parentPlacemrnt, int price)
    {
        if (newPleacement == WeaponPleacement.Base)
        {
            BuyPlayersItem(parentPlacemrnt, price);
            return true;
        }
        else if (parentPlacemrnt != WeaponPleacement.Base && newPleacement != WeaponPleacement.Base)
        {
            return RemovePlayersItem(weapon, newPleacement, parentPlacemrnt);
        }
        else
        {
            return SellItemToPlayer(weapon, newPleacement, price); 
        }

    }

    public bool TryInstallCollector(Vector3 position)
    {
        if (_player.ResourcesAmount >= ResourseCollectorPrice)
        {
            _player.ChangeResourcesAmount(-ResourseCollectorPrice);
            ResourceCollector collector = Instantiate(_collectorTemplate, _buildingsContainer);
            collector.SetOwner(_player);
            _teleport.SendBuildingToPosition(collector, position, true);
            return true;
        }
        else
        {
            _player.ShowMessage("У Вас не хватает русурсов на установку здания.");
            return false;
        }
    }

    public void ShowInfo(string lable, string description)
    {
        _lable.ShowText(lable);
        _description.ShowText(description);
    }

    private void AddWeapon(WeaponCard weaponCard, DropContainer container, WeaponPleacement weaponPleacement)
    {
        WeaponView view = Instantiate(weaponCard.ViewTemplate, container.Location);
        view.Init(weaponCard, transform, GetComponent<MainBase>(), container.Location, weaponPleacement);
    }

    private Dictionary<Vector3, int> GetActiveResourceCollectorsPositionsWithHealth()
    {
        var result = new Dictionary<Vector3, int>();
        ResourceCollector[] activeCollectors = _buildingsContainer.GetComponentsInChildren<ResourceCollector>();

        foreach (ResourceCollector collector in activeCollectors)
            result[collector.transform.position] = collector.Health;

        return result;
    }

    private void BuyPlayersItem(WeaponPleacement parentPlacemrnt, int price)
    {
        _player.ChangeResourcesAmount(price);
        _player.UninstallWeapon(parentPlacemrnt);
        _audioSource.PlayOneShot(_weaponIstallSounds[Random.Range(0, _weaponIstallSounds.Count)]);
    }

    private bool SellItemToPlayer(WeaponLable weapon, WeaponPleacement newPleacement, int price)
    {
        if (IsWeaponPlacementFree(newPleacement) && IsMoneyEnough(price) && TryAddWeaponToPlayer(weapon, newPleacement))
        {
            _player.ChangeResourcesAmount(-price);
            _audioSource.PlayOneShot(_weaponIstallSounds[Random.Range(0, _weaponIstallSounds.Count)]);
            return true;
        }
        else
            return false;
    }

    private bool RemovePlayersItem(WeaponLable weapon, WeaponPleacement newPleacement, WeaponPleacement parentPlacemrnt)
    {
        if (IsWeaponPlacementFree(newPleacement) && TryAddWeaponToPlayer(weapon, newPleacement))
        {
            _player.UninstallWeapon(parentPlacemrnt);
            _audioSource.PlayOneShot(_weaponIstallSounds[Random.Range(0, _weaponIstallSounds.Count)]);
            return true;
        }
        else
            return false;
    }

    private bool IsWeaponPlacementFree(WeaponPleacement weaponPleacement)
    {
        if (_player.GetWeapon(weaponPleacement) == null)
            return true;
        else
        {
            ShowInfo("Внимание!", "Оружейный слот занят. Снимите имеющиеся оружие.");
            return false;
        }
    }

    private bool IsMoneyEnough(int price)
    {
        if (_player.ResourcesAmount >= price)
            return true;
        else
        {
            ShowInfo("Внимание!", "У Вас не хватает ресурсов на производство данного оружия");
            return false;
        }
    }

    private bool TryAddWeaponToPlayer(WeaponLable weapon, WeaponPleacement weaponPleacement)
    {
        if (_player.TryAddWeapon(weapon, weaponPleacement))
            return true;
        else
        {
            ShowInfo("Внимание!", "Данное оружие не предназначенно для установки в этот слот");
            return false;
        }
    }
}
