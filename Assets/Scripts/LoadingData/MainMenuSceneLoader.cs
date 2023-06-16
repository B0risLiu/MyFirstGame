using IJunior.TypedScenes;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuSceneLoader : SceneLoader, ISceneLoadHandler<LoadingData>
{
    [SerializeField] private PanelManager _menuManager;
    [SerializeField] private Slider _enemySpawnAmount;
    [SerializeField] private Toggle _permanentDeath;
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _quality;
    [SerializeField] private AudioMixerGroup _mixer;

    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";

    private LoadingData _loadingData;

    private void Start()
    {
        _mixer.audioMixer.GetFloat(MasterVolume, out float masterVolumeValue);
        _masterVolume.value = masterVolumeValue;
        _mixer.audioMixer.GetFloat(MusicVolume, out float musicVolumeValue);
        _musicVolume.value = musicVolumeValue;
    }

    public void OnSceneLoaded(LoadingData argument)
    {
        _loadingData = argument;
        _enemySpawnAmount.value = _loadingData.EnemySpawnAmount;
        _quality.value = _loadingData.VideoQuality;
        _permanentDeath.isOn = _loadingData.IsPermanentDeathModeOn;
        StartOpeningAnimation();
    }

    public void OnNewGameClick()
    {
        if (_loadingData == null)
            _loadingData = new LoadingData();
        else
            _loadingData.ClearPlanetLoadingData();

        StartCoroutine(LoadingPlanet());
    }

    public void OnContinueClick()
    {
        if (_loadingData != null && _loadingData.IsPlanetLoadingDataExsist())
        {
            _menuManager.CloseCurrent();
            StartCoroutine(LoadingPlanet());
        }
    }

    public void OnSettingsClick()
    {
        if (_loadingData == null)
            _loadingData= new LoadingData();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnDificultySliderChanged(float value)
    {
        _loadingData.SetEnemySpawnAmount(value);
    }

    public void OnPermanentDeathToogleChanged(bool value)
    {
        _loadingData.SetPermanentDeathModeOn(value);
    }

    public void OnQualitySliderChanged(float value)
    {
        _loadingData.SetVideoQuality(value);
    }

    public void OnMasterVolumeSliderChanged(float value)
    {
        _mixer.audioMixer.SetFloat(MasterVolume, value);
    }

    public void OnMusicVolumeSliderChanged(float value)
    {
        _mixer.audioMixer.SetFloat(MusicVolume, value);
    }

    private IEnumerator LoadingPlanet()
    {
        StartClosingAnimation();

        while (IsAnimationComplete == false)
            yield return null;

        SceneLoading = Planet.LoadAsync(_loadingData);
        StartCoroutine(UpdateLoadingProgressBar());
    }
}
