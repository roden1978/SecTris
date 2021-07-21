using UnityEngine;
using UnityEngine.UI;

namespace InputSwipe.Pause
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Game game;
        [SerializeField] private GameObject pauseButton;
        private PauseActions _action;
        private bool _isPaused;
        private Button _button;

        private void Awake()
        {
            _action = new PauseActions();
        }

        private void Start()
        {
            _action.PauseGame.Pause.performed += _ => DeterminePause();
            _button = pauseButton.GetComponent<Button>();
        }

        private void DeterminePause()
        {
            if (_button.interactable)
            {
                if(_isPaused)
                    Resume();
                else
                    Pause();
            }
            
        }

        private void OnEnable()
        {
            _action.Enable();
            game.OnGameStart += PauseButtonOn;
            game.OnStopGame += PauseButtonOff;
        }

        private void OnDisable()
        {
            _action.Disable();
            game.OnGameStart -= PauseButtonOn;
            game.OnStopGame -= PauseButtonOff;
        }

        public void Pause()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            _isPaused = true;
        }

        public void Resume()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void PauseBack()
        {
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        private void PauseButtonOn()
        {
            _button.interactable = true;
        }

        private void PauseButtonOff()
        {
            _button.interactable = false;
        }
      
    }
}
