using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField] private Neighbor _neighbor;
    [SerializeField] private Game _game;
    [SerializeField] private Bucket _bucket;
    [SerializeField] private int _sectorPrice;
    [SerializeField] private TMP_Text _scoresText;
    [SerializeField] private TMP_Text _highScoresText;
    [SerializeField] private int _highScores;
    
    private int _scores;
    private int _tmpScores;

    private bool _increment;

    private Coroutine _coroutine;
    private GameData _gameData;
    public event Action<GameData> OnHighScoreChange; 

    private void StartChecking()
    {
        _coroutine = StartCoroutine(CheckIncomingScoresAmount());
    }

    private void StopChecking()
    {
        StopCoroutine(_coroutine);
    }

    private void OnEnable()
    {
        _neighbor.OnScoreChanged += ScoreChanged;
        _game.OnGameStart += ScoresReset;
        _game.OnGameStart += StartChecking;
        _game.OnNewGameData += GameDataChange;
        _bucket.OnOverflowBucket += StopChecking;
    }

    private void OnDisable()
    {
        _neighbor.OnScoreChanged -= ScoreChanged;
        _game.OnGameStart -= ScoresReset;
        _game.OnGameStart -= StartChecking;
        _game.OnNewGameData -= GameDataChange;
        _bucket.OnOverflowBucket -= StopChecking;
    }
    
    private void ScoreChanged(int scores)
    {
        _tmpScores += _sectorPrice * scores;
    }

    private void GameDataChange(GameData data)
    {
        _gameData = data;
        _highScores = _gameData._highScores;
        UpdateUIHighScores(_highScores);
    }

    private void UpdateUIHighScores(int highScores)
    {
        _highScoresText.text = highScores.ToString();
    }

    private void HighScoresChange(int scores)
    {
        if (scores > _highScores) _highScores = scores;
        UpdateUIHighScores(_highScores); 
        _gameData._highScores = _highScores;
        OnHighScoreChange?.Invoke(_gameData);
    }
    
    private void ScoresReset()
    {
        _scores = 0;
        _scoresText.text = _scores.ToString(); 
    }

    private IEnumerator IncrementScores()
    {
        while (_tmpScores > 0)
        {
            yield return null;
            _scores += 10;
            _tmpScores -= 10;
            _scoresText.text = _scores.ToString();
        }
        HighScoresChange(_scores);
        _increment = false;
    }

    private IEnumerator CheckIncomingScoresAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            if (_tmpScores > 0 && !_increment)
            {
                _increment = true;
                StartCoroutine(IncrementScores());
            }
        }
    }
}