using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _gameOverPanel;

    private void OnEnable()
    {
        _player.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _player.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        _gameOverPanel.SetActive(true);
    }
}
