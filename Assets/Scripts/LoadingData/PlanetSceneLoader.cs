using IJunior.TypedScenes;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlanetSceneLoader : SceneLoader, ISceneLoadHandler<LoadingData>
{
    [SerializeField] private Player _player;
    [SerializeField] private MainBase _mainBase;
    [SerializeField] private ResourceDetector _resourceDetector;
    [SerializeField] private FollowingCamera _followingCamera;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TerrainLoader _terrainLoader;
    [SerializeField] private PostProcessVolume _postProcessVolume;

    private LoadingData _loadingData;

    public void OnSceneLoaded(LoadingData argument)
    {
        _loadingData = argument;
        _loadingData.LoadMainBaseData(_mainBase);
        _loadingData.LoadCameraData(_followingCamera);
        _loadingData.LoadPlayerData(_player);
        _loadingData.LoadSpawnerData(_enemySpawner);
        _loadingData.LoadTerrainData(_terrainLoader);
        _enemySpawner.SetDelayBetweenWawes(_loadingData.EnemySpawnAmount);
        _postProcessVolume.weight = _loadingData.VideoQuality;
        StartOpeningAnimation();
    }

    public void MainMenuLoad()
    {
        _player.SavePlayerData(_loadingData);
        _followingCamera.SaveCameraData(_loadingData);
        _mainBase.SaveMainBaseData(_loadingData);
        _enemySpawner.SaveSpawnerData(_loadingData);
        _terrainLoader.SaveTerrainData(_loadingData);
        StartCoroutine(LoadMainMenu());
    }

    public void OnGameOverMainMenuLoad()
    {
        _loadingData.ClearPlanetLoadingData();
        _terrainLoader.DiscardTerrainChanges();
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        StartClosingAnimation();

        while (IsAnimationComplete == false)
            yield return null;

        SceneLoading = MainMenu.LoadAsync(_loadingData);
        StartCoroutine(UpdateLoadingProgressBar());
    }
}
