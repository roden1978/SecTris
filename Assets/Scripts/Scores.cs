using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField] private Neighbor _neighbor;
    [SerializeField] private Game _game;
    [SerializeField] private int _sectorPrice;
    [SerializeField] private TMP_Text _scoresText;
    [SerializeField] private TMP_Text _highScoresText;
    [SerializeField] private int _highScores;
    
    private int _scores;
    private int _tmpScores;

    private bool _increment;

    private Coroutine _coroutine;

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
        _game.OnStopGame += StopChecking;
    }

    private void OnDisable()
    {
        _neighbor.OnScoreChanged -= ScoreChanged;
        _game.OnGameStart -= ScoresReset;
        _game.OnGameStart -= StartChecking;
        _game.OnStopGame -= StopChecking;
    }
    
    private void ScoreChanged(int scores)
    {
        _tmpScores += _sectorPrice * scores;
        //var newScores = _scores + _sectorPrice * scores;
        //StartCoroutine(IncrementScores());
    }

    private void HighScoresChange(int scores)
    {
        if (scores > _highScores) _highScores = scores;
        _highScoresText.text = _highScores.ToString();
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
            _scores++;
            _tmpScores--;
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