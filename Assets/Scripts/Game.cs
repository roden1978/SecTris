using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    
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
    }

    public void Exit()
    {
        Application.Quit();
    }
}
