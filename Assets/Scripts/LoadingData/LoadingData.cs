using System.Collections.Generic;
using UnityEngine;

public class LoadingData
{
    public bool IsPermanentDeathModeOn { get; private set; }
    public float EnemySpawnAmount { get; private set; }
    public float VideoQuality { get; private set; }
    public int PlayerResourcesAmount { get; private set; }

    private PlanetLoadingData _planetLoadingData;

    public LoadingData()
    {
        IsPermanentDeathModeOn = false;
        EnemySpawnAmount = 0.5f;
        VideoQuality = 1f;
        PlayerResourcesAmount = 500;
    }

    public bool IsPlanetLoadingDataExsist()
    {
        return _planetLoadingData != null;
    }

    public void ClearPlanetLoadingData()
    {
        _planetLoadingData = null;
    }

    public void SetEnemySpawnAmount(float value)
    {
        EnemySpawnAmount = value;
    }

    public void SetPermanentDeathModeOn(bool value)
    {
        IsPermanentDeathModeOn = value;
    }

    public void SetVideoQuality(float value)
    {
        VideoQuality = value;
    }

    public void SavePlayerData(int playerCurentHealth, int playerResourceAmount, bool isPlayerFireAndMoveModesOn, 
        bool isPermanentDethOn, Vector3 position, Quaternion rotation, List<Weapon> playerInstalledWeapon)
    {
        CheckPlanetLoadingData();
        _planetLoadingData.PlayerDataStorage = new PlayerData(playerCurentHealth, playerResourceAmount, 
            isPlayerFireAndMoveModesOn, isPermanentDethOn, position, rotation, playerInstalledWeapon);
    }

    public void SaveCameraData(Vector3 position, Quaternion rotation, bool isCameraFollowingModeOn)
    {
        CheckPlanetLoadingData();
        _planetLoadingData.CameraDataStorage = new CameraData(position, rotation, isCameraFollowingModeOn);
    }

    public void SaveMainBaseData(bool isMainBasePanelActive, WeaponCard rightHand, WeaponCard leftHand, WeaponCard backbone, 
        List<WeaponCard> weaponStorage, Dictionary<Vector3, int> activeResourceCollectorsPositionsWithHealth)
    {
        CheckPlanetLoadingData();
        _planetLoadingData.MainBaseDataStorage = new MainBaseData(isMainBasePanelActive, rightHand, leftHand, backbone, 
            weaponStorage, activeResourceCollectorsPositionsWithHealth);
    }

    public void SaveSpawnerData(List<EnemyData> enemiesData)
    {
        CheckPlanetLoadingData();
        _planetLoadingData.SpawnerDataStorage = new SpawnerData(enemiesData);
    }

    public void SaveTerrainData(List<int> deadVegetationIDs, List<EffectData> effectsData, Dictionary<int, int> resourceDepositsIDAndAmount)
    {
        CheckPlanetLoadingData();
        _planetLoadingData.TerrainDataStorage = new TerrainData(deadVegetationIDs, effectsData, resourceDepositsIDAndAmount);
    }

    public void LoadPlayerData(Player player)
    {
        if (_planetLoadingData != null)
            player.LoadPlayerData(_planetLoadingData.PlayerDataStorage.CurentHealth, _planetLoadingData.PlayerDataStorage.ResourceAmount, 
                _planetLoadingData.PlayerDataStorage.IsPlayerFireAndMoveModesOn, _planetLoadingData.PlayerDataStorage.IsPermanentDethOn,
                _planetLoadingData.PlayerDataStorage.Position, _planetLoadingData.PlayerDataStorage.Rotation,
                _planetLoadingData.PlayerDataStorage.InstalledWeapon);
        else
            player.LoadPlayerDefaultData(IsPermanentDeathModeOn, PlayerResourcesAmount);
    }

    public void LoadCameraData(FollowingCamera followingCamera)
    {
        if (_planetLoadingData != null)
        {
            followingCamera.SetCameraToLocation(_planetLoadingData.CameraDataStorage.Position, _planetLoadingData.CameraDataStorage.Rotation);
            followingCamera.FollowingModeOn(_planetLoadingData.CameraDataStorage.IsCameraFollowingModeOn);
        }
    }

    public void LoadMainBaseData(MainBase mainBase)
    {
        if (_planetLoadingData != null)
            mainBase.LoadMainBaseData(_planetLoadingData.MainBaseDataStorage.IsMainBasePanelActive, _planetLoadingData.MainBaseDataStorage.RightHand, 
                _planetLoadingData.MainBaseDataStorage.LeftHand, _planetLoadingData.MainBaseDataStorage.Backbone, _planetLoadingData.MainBaseDataStorage.WeaponStorage, 
                _planetLoadingData.MainBaseDataStorage.ActiveResourceCollectorsPositionsWithHealth);
        else
            mainBase.LoadMainBaseDefaultData();
    }

    public void LoadSpawnerData(EnemySpawner enemySpawner)
    {
        if (_planetLoadingData != null)
            enemySpawner.LoadSpawnerData(_planetLoadingData.SpawnerDataStorage.EnemiesData);
    }

    public void LoadTerrainData(TerrainLoader terrainLoader)
    {
        if (_planetLoadingData != null)
            terrainLoader.LoadTerrainData(_planetLoadingData.TerrainDataStorage.DeadVegetationIDs, _planetLoadingData.TerrainDataStorage.EffectsData, _planetLoadingData.TerrainDataStorage.ResourceDepositsIDAndAmount);

    }

    private void CheckPlanetLoadingData()
    {
        if (_planetLoadingData == null)
            _planetLoadingData = new PlanetLoadingData();
    }
}
