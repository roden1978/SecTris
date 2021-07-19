using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Pillar pillar;
    [SerializeField] private Background background;
    [SerializeField] private Pool pool;
    
    public event Action OnGameOver;
    
    private const float StopPoint = 4.7f;
    private IMaxYPosition _maxYPosition;
    private List<GameObject> _fixed;
    private float _bucketHeight;

    private bool _isGameOver;
    void Start()
    {
        _fixed = new List<GameObject>();
        Time.timeScale = 0;
        if(buttonPanel.activeInHierarchy == false)
            buttonPanel.SetActive(true);
        _maxYPosition = new MaxYPosition(_fixed);
    }

    private void FixedUpdate()
    {
        if(!_isGameOver)
        {
            var active = pool.GetAllActive();
            foreach (var sector in active)
            {
                var rb = sector.transform.GetComponent<Rigidbody>();
                if (rb.isKinematic)
                    _fixed.Add(sector);
            }

            _bucketHeight = _maxYPosition.Value();
            if (_bucketHeight > StopPoint)
            {
                _isGameOver = true;
                OnGameOver?.Invoke();
            }
            _fixed.Clear();
        }
    }

    
    public void StartGame()
    {
        _bucketHeight = 0;
        _isGameOver = false;
        Time.timeScale = 1;
        buttonPanel.SetActive(false);
        pillar.StartSpawn();
        ChangeBackground();
    }

    public void Exit()
    {
        Application.Quit();
    }

   private void OnEnable()
    {
        OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        _bucketHeight = 0;
        _fixed.Clear();
        if(gameOverPanel.activeInHierarchy == false)
            gameOverPanel.SetActive(true);
    }


 

    private void ChangeBackground()
    {
        background.ChangeBackground();
    }
    
    public float BucketHeight => _bucketHeight;

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }
}
