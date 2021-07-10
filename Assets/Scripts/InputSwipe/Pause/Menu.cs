using UnityEngine;

namespace InputSwipe.Pause
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject buttonPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Pillar pillar;
        
        private PauseActions _action;
        private bool _isPaused;

        private void Awake()
        {
            _action = new PauseActions();
        }

        private void Start()
        {
            _action.PauseGame.Pause.performed += _ => DeterminePause();
        }

        private void DeterminePause()
        {
            if(_isPaused)
                Resume();
            else
                Pause();
        }

        private void OnEnable()
        {
            _action.Enable();
        }

        private void OnDisable()
        {
            _action.Disable();
        }

        public void Pause()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _isPaused = true;
        }

        public void Resume()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            AudioListener.pause = false;
            _isPaused = false;
        }

        public void Back()
        {
            if(pausePanel.activeInHierarchy)
                pausePanel.SetActive(false);
            if(gameOverPanel.activeInHierarchy)
                gameOverPanel.SetActive(false);
            buttonPanel.SetActive(true);
            pillar.ResetPools();
        }
    }
}
