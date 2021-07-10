using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Pillar pillar;
    void Start()
    {
        Time.timeScale = 0;
        if(buttonPanel.activeInHierarchy == false)
            buttonPanel.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        buttonPanel.SetActive(false);
        pillar.StartSpawn();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        pillar.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        pillar.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        if(gameOverPanel.activeInHierarchy == false)
            gameOverPanel.SetActive(true);
    }
}
