using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Background _background;
    [SerializeField] private Pool _pool;
   
    [SerializeField] private Neighbor _neighbor;
    public event Action OnGameOver;
    public event Action OnStopGame;
    public event Action OnGameStart;

    public event Action<float> OnChangeBucketHeight;

    private Coroutine _fixingSectors;
    private Coroutine _removeNotActive;
    
    private const float StopPoint = 5f;
    private IBucketHeight _bucketHeight;
    private List<GameObject> _fixed;
    
    private int _prevFixedCount;
    private float _prevBucketHeight;
    private float _currentBucketHeight;
    private void Awake()
    {
        _fixed = new List<GameObject>();
        _bucketHeight = new BucketHeight(_fixed);
    }

    private void Start()
    {
        Time.timeScale = 0;
        _mainPanel.SetActive(true);
    }

   private IEnumerator FixingSectors(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            var active = _pool.GetAllActive();
            foreach (var sector in active)
            {
                var sectorRigidbody = sector.transform.GetComponent<Rigidbody>();
                if (sectorRigidbody.isKinematic)
                    _fixed.Add(sector);
            }
            
            _currentBucketHeight = _bucketHeight.Value();
            if (Math.Abs(_prevBucketHeight - _currentBucketHeight) > 0)
            {
                _prevBucketHeight = _currentBucketHeight;
                Debug.Log(_currentBucketHeight);
                OnChangeBucketHeight?.Invoke(_currentBucketHeight);
            }
            if (_currentBucketHeight > StopPoint)
            {
                OnChangeBucketHeight?.Invoke(0f);
                OnStopGame?.Invoke();
            }
            
            if (_prevFixedCount != _fixed.Count)
            {
                _prevFixedCount = _fixed.Count;
                _neighbor.Find(_fixed);
            }
            
            _prevFixedCount = _fixed.Count;
            _fixed.Clear();
        }
    }
    private bool NotActive(GameObject sector)
    {
        return !sector.activeInHierarchy;
    }
    private IEnumerator RemoveNotActive(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            _fixed.RemoveAll(NotActive);
        }
    }
    
    public void StartGame()
    {
        _currentBucketHeight = 0;
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
        _gameOverPanel.SetActive(false);
        StopCoroutine(_fixingSectors);
        StopCoroutine(_removeNotActive);
        _currentBucketHeight = 0;
        _prevFixedCount = 0;
        _fixed.Clear();
    }

    private void StopGame()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void GameStart()
    {
        _fixingSectors = StartCoroutine(FixingSectors(.1f));
        _removeNotActive = StartCoroutine(RemoveNotActive(.5f));
    }
    private void ChangeBackground()
    {
        _background.ChangeBackground();
    }
    
}
