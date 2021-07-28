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
    private void OnEnable()
    {
        _neighbor.OnScoreChanged += ScoreChanged;
        _game.OnGameStart += ScoresReset;
    }

    private void OnDisable()
    {
        _neighbor.OnScoreChanged -= ScoreChanged;
        _game.OnGameStart -= ScoresReset;
    }
    
    private void ScoreChanged(int scores)
    {
        var newScores = _scores + _sectorPrice * scores;
        _scores = newScores;
        _scoresText.text = newScores.ToString();
        HighScoresChange(_scores);
    }

    private void HighScoresChange(int scores)
    {
        if (scores > _highScores) _highScores = scores;
        _highScoresText.text = _highScores.ToString();
    }
    
    private void ScoresReset()
    {
        _scores = 0;
        ScoreChanged(0); 
    }
}