using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Background background;
    [SerializeField] private Pool pool;
    [SerializeField] private int sectorPrice;
    [SerializeField] private TMP_Text scoresText;
    [SerializeField] private TMP_Text highScoresText;
    [SerializeField] private int highScores;
    public event Action OnGameOver;
    public event Action OnStopGame;
    public event Action OnGameStart;

    private Coroutine _fixingSectors;
    private Coroutine _removeNotActive;
    
    private const float StopPoint = 5f;
    private IMaxYPosition _maxYPosition;
    private List<GameObject> _fixed;
    private Neighbor _neighbor;
    
    private int _prevFixedCount;
    private int _scores;
    


    private void Awake()
    {
        _fixed = new List<GameObject>();
        _neighbor = new Neighbor(_fixed);
        _maxYPosition = new MaxYPosition(_fixed);
    }

    void Start()
    {
        Time.timeScale = 0;
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

            if (_prevFixedCount != _fixed.Count)
            {
                _prevFixedCount = _fixed.Count;
                _neighbor.Find();
            }
            BucketHeight = _maxYPosition.Value();
            if (BucketHeight > StopPoint)
            {
                OnStopGame?.Invoke();
            }
            _fixed.Clear();
            _prevFixedCount = _fixed.Count;
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
        ScoresReset();
        BucketHeight = 0;
        Time.timeScale = 1;
        mainPanel.SetActive(false);
        ChangeBackground();
        OnGameStart?.Invoke();
    }

    private void ScoresReset()
    {
        _scores = 0;
        ScoreChanged(0); 
    }
   
    public void Exit()
    {
        Application.Quit();
    }

   private void OnEnable()
    {
        OnStopGame += StopGame;
        OnGameStart += GameStart;
        _neighbor.OnScoreChanged += ScoreChanged;

    }

    private void OnDisable()
    {
        OnStopGame -= StopGame;
        OnGameStart -= GameStart;
        _neighbor.OnScoreChanged -= ScoreChanged;
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        OnGameOver?.Invoke();
        gameOverPanel.SetActive(false);
        StopCoroutine(_fixingSectors);
        StopCoroutine(_removeNotActive);
        BucketHeight = 0;
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
        _removeNotActive = StartCoroutine(RemoveNotActive(.5f));
    }
    private void ChangeBackground()
    {
        background.ChangeBackground();
    }
    
    public float BucketHeight { get; private set; }

    private void ScoreChanged(int scores)
    {
        var newScores = _scores + sectorPrice * scores;
        _scores = newScores;
        scoresText.text = newScores.ToString();
        HighScoresChange(_scores);
    }

    private void HighScoresChange(int scores)
    {
        if (scores > highScores) highScores = scores;
        highScoresText.text = highScores.ToString();
    }

}
