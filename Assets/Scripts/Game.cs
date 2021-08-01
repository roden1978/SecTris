using System;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Background _background;
    [SerializeField] private Bucket _bucket;
   
    public event Action OnGameOver;
    public event Action OnGameStart;

    private void Start()
    {
        Time.timeScale = 0;
        _mainPanel.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        _mainPanel.SetActive(false);
        ChangeBackground();
        OnGameStart?.Invoke();
    }
    public void Exit()
    {
        Application.Quit();
    }

   private void OnEnable()
    {
        _bucket.OnOverflowBucket += StopGame;
    }

    private void OnDisable()
    {
        _bucket.OnOverflowBucket -= StopGame;
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        OnGameOver?.Invoke();
        _gameOverPanel.SetActive(false);
    }

    private void StopGame()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void ChangeBackground()
    {
        _background.ChangeBackground();
    }
    
}
