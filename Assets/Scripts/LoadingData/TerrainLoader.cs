using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Terrain))]

public class TerrainLoader : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Header("Vegetation Uptate Settings")]
    [SerializeField] private float _delayBetweenVegetationUpdates;
    [SerializeField] private int _activeVegetationSpaceWidth;
    [SerializeField] private int _activeVegetationSpaceHeight;
    [Header("Effects Pools")]
    [SerializeField] private List<ObjectPool> _effects;

    private Terrain _terrain;
    private Coroutine _workingCouroutine;

    private List<DestroyableVegetation> _destroyableVegetations = new List<DestroyableVegetation>();
    private List<ResourceDeposit> _resourceDeposits = new List<ResourceDeposit>();
    private List<int> _activeVegetationIDs = new List<int>();
    private int[,] _defaultDensityMap;
    private List<int>[,] _vegetationMap;
    private WaitForSeconds _delay;

    private List<int> _deadVegetationIDs;
    private List<EffectData> _effectsData;
    private Dictionary<int, int> _resourceDepositsIDAndAmount;

    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
        _delay = new WaitForSeconds(_delayBetweenVegetationUpdates);
        _defaultDensityMap = CopyMap(_terrain.terrainData.GetDetailLayer(0, 0, _terrain.terrainData.detailWidth, _terrain.terrainData.detailHeight, 0));
        gameObject.GetComponentsInChildren(true, _destroyableVegetations);
        gameObject.GetComponentsInChildren(_resourceDeposits);
        DrawVegetationsMap();
        AssignID(_resourceDeposits);
    }

    private void OnEnable()
    {
        if (_deadVegetationIDs != null)
            DisactivateVegetations();
        if (_resourceDepositsIDAndAmount != null)
            LoadResourcesAmount();
        if (_effectsData != null)
        {
            SetEffects();
            LoadTerrainChanges();
        }
    }

    private void Update()
    {
        if (_workingCouroutine == null)
            _workingCouroutine = StartCoroutine(UpdateActiveVegetations());
    }

    public void LoadTerrainData(List<int> deadVegetationIDs, List<EffectData> effectsData, Dictionary<int, int> resourceDepositsIDAndAmount)
    {
        _deadVegetationIDs = deadVegetationIDs;
        _effectsData = effectsData;
        _resourceDepositsIDAndAmount = resourceDepositsIDAndAmount;
    }

    public void SaveTerrainData(LoadingData loadingData)
    {
        List<int> deadVegetationIDs = GetDeadVegetationIDs();
        List<EffectData> effectsData = GetEffectsData();
        Dictionary<int, int> resourceDepositsIDAndAmount = GetResourceDepositsIDAndAmount();
        DiscardTerrainChanges();
        loadingData.SaveTerrainData(deadVegetationIDs, effectsData, resourceDepositsIDAndAmount);
    }

    public void DiscardTerrainChanges()
    {
        _terrain.terrainData.SetDetailLayer(0, 0, 0, _defaultDensityMap);
    }

    private void LoadTerrainChanges()
    {
        foreach (var effectData in _effectsData)
            _terrain.terrainData.SetDetailLayer((int)effectData.TerrainChangeData.TerrainPosition.x, (int)effectData.TerrainChangeData.TerrainPosition.y, 
                0, effectData.TerrainChangeData.ChangedDensityMap);
    }

    private void AssignID<T>(List<T> values) where T : IIDable
    {
        for (int i = 0; i < values.Count; i++)
            values[i].AssignID(i);
    }

    private List<EffectData> GetEffectsData()
    {
        return _effects
            .SelectMany(pool => pool.GetAllActive())
            .Select(obj => new EffectData(obj.GetComponent<Explosion>().WeaponWichHasBeenSooted, obj.transform.position, obj.transform.rotation, obj.GetComponent<Explosion>().TerrainChangeData))
            .ToList();
    }

    private List<int> GetDeadVegetationIDs()
    {
        return _activeVegetationIDs.Where(id => _destroyableVegetations[id].IsDead).ToList();
    }

    private Dictionary<int, int> GetResourceDepositsIDAndAmount()
    {
        Dictionary<int, int> result = new Dictionary<int, int>();

        foreach (ResourceDeposit resourceDeposit in _resourceDeposits)
            result[resourceDeposit.ID] = resourceDeposit.ResourceAmount;

        return result;
    }

    private void DisactivateVegetations()
    {
        foreach (int id in _deadVegetationIDs)
            _destroyableVegetations[id].Kill();
    }

    private void LoadResourcesAmount()
    {
        foreach (ResourceDeposit resourceDeposit in _resourceDeposits)
        {
            foreach (var pair in _resourceDepositsIDAndAmount)
            {
                if (resourceDeposit.ID == pair.Key)
                    resourceDeposit.SetResourceAmount(pair.Value);
            }
        }
    }

    private void SetEffects()
    {
        foreach (ObjectPool objectPool in _effects)
        {
            foreach (EffectData effectData in _effectsData)
            {
                if (objectPool.TryGetObject(out GameObject poolObject) 
                    && poolObject.gameObject.TryGetComponent(out Explosion effect) 
                    && effectData.WeaponWhichHasBeenShooted == effect.WeaponWichHasBeenSooted)
                {
                    effect.SaveTerrainChangeData(effectData.TerrainChangeData);
                    poolObject.transform.SetPositionAndRotation(effectData.Position, effectData.Rotation);
                    poolObject.SetActive(true);
                }
            }
        }
    }

    private int[,] CopyMap(int[,] map)
    {
        int[,] mapCopy = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                mapCopy[x, y] = map[x, y];
            }
        }

        return mapCopy;
    }

    private void DrawVegetationsMap()
    {
        VegetationMapInit();

        for (int i = 0; i < _destroyableVegetations.Count; i++)
        {
            int x = (int)_destroyableVegetations[i].transform.position.x;
            int z = (int)_destroyableVegetations[i].transform.position.z;
            _vegetationMap[x, z].Add(i);
        }
    }

    private void VegetationMapInit()
    {
        _vegetationMap = new List<int>[(int)_terrain.terrainData.size.x, (int)_terrain.terrainData.size.z];

        for (int x = 0; x < _vegetationMap.GetLength(0); x++)
        {
            for (int z = 0; z < _vegetationMap.GetLength(1); z++)
                _vegetationMap[x, z] = new List<int>();
        }
    }

    private IEnumerator UpdateActiveVegetations()
    {
        List<int> vegetationIDsInCameraView = GetVegetationIDsInCameraView();
        List<int> vegetationIDsWhichHaveToStayActive = vegetationIDsInCameraView.Intersect(_activeVegetationIDs).ToList();
        List<int> vegetationIDsWhichNeedToActivate = vegetationIDsInCameraView.Except(vegetationIDsWhichHaveToStayActive).ToList();
        List<int> vegetationIDsWhichNeedToDisactivate = _activeVegetationIDs.Except(vegetationIDsWhichHaveToStayActive).ToList();
        VegetationsEnabled(true, vegetationIDsWhichNeedToActivate);
        VegetationsEnabled(false, vegetationIDsWhichNeedToDisactivate);
        _activeVegetationIDs = vegetationIDsInCameraView;
        yield return _delay;
        _workingCouroutine = null;
    }

    private List<int> GetVegetationIDsInCameraView()
    {
        int playerX = (int)_player.transform.position.x;
        int playerZ = (int)_player.transform.position.z;
        List<int> result = new List<int>();

        for (int x = playerX - _activeVegetationSpaceWidth / 2; x < playerX + _activeVegetationSpaceWidth / 2; x++)
        {
            for (int z = playerZ - _activeVegetationSpaceHeight / 2; z < playerZ + _activeVegetationSpaceHeight / 2; z++)
            {
                foreach (int ID in _vegetationMap[x, z])
                    result.Add(ID);
            }
        }

        return result;
    }

    private void VegetationsEnabled(bool value, List<int> vegetationIDs)
    {
        foreach (int id in vegetationIDs)
        {
            if (_destroyableVegetations[id].IsDead == false)
                _destroyableVegetations[id].gameObject.SetActive(value);
        }
    }
}
