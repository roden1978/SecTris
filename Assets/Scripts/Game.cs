using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Background background;
    [SerializeField] private Pool pool;
    
    public event Action OnGameOver;
    public event Action OnStopGame;
    public event Action OnGameStart;

    private Coroutine _fixingSectors;
    
    private const float StopPoint = 4.7f;
    private IMaxYPosition _maxYPosition;
    private List<GameObject> _fixed;
    private float _bucketHeight;

    void Start()
    {
        _fixed = new List<GameObject>();
        Time.timeScale = 0;
        _maxYPosition = new MaxYPosition(_fixed);
        mainPanel.SetActive(true);
    }

    private IEnumerator FixingSectors()
    {
        while(true)
        {
            yield return new WaitForSeconds(.1f);
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
                OnStopGame?.Invoke();
            }
        }
    }

    
    public void StartGame()
    {
        _bucketHeight = 0;
        Time.timeScale = 1;
        mainPanel.SetActive(false);
        ChangeBackground();
        OnGameStart?.Invoke();
    }

    public void Exit()
    {
        Application.Quit();
    }

   private void OnEnable()
    {
        OnStopGame += StopGame;
        OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        OnStopGame -= StopGame;
        OnGameStart -= GameStart;
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        OnGameOver?.Invoke();
        gameOverPanel.SetActive(false);
        StopCoroutine(_fixingSectors);
        _bucketHeight = 0;
        _fixed.Clear();
    }

    private void StopGame()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void GameStart()
    {
        _fixingSectors = StartCoroutine(FixingSectors());
    }
    private void ChangeBackground()
    {
        background.ChangeBackground();
    }
    
    public float BucketHeight => _bucketHeight;

}
