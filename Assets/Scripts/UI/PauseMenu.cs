using UnityEngine;

[RequireComponent(typeof(MainBase))]

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Player _player;

    private MainBase _mainBase;
    private PlayerInput _input;
    private bool _isGameOver;

    private void Awake()
    {
        _mainBase = GetComponent<MainBase>();
        _input = new PlayerInput();
        _input.Player.PauseMenu.performed += ctx => OnPauseMenuClick();
    }

    private void OnEnable()
    {
        _input.Enable();
        _player.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _input.Disable();
        _player.GameOver -= OnGameOver;
    }

    public void OnPauseMenuClick()
    {
        if (_isGameOver == false)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);

            if (_mainBase.IsMainBasePanelActive == false)
                _player.FireAndMoveModesEnable(!_pauseMenu.activeSelf);
        }
    }

    private void OnGameOver()
    {
        _isGameOver = true;
    }
}
