using UnityEngine;
using UnityEngine.UI;

namespace InputSwipe.Pause
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Game _game;
        [SerializeField] private Bucket _bucket;
        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private GameObject _settingsPanel;
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
            _button = _pauseButton.GetComponent<Button>();
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
            _game.OnGameStart += PauseButtonOn;
            _bucket.OnOverflowBucket += PauseButtonOff;
        }

        private void OnDisable()
        {
            _action.Disable();
            _game.OnGameStart -= PauseButtonOn;
            _bucket.OnOverflowBucket -= PauseButtonOff;
        }

        public void Pause()
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
            _isPaused = true;
        }

        public void Resume()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void PauseBack()
        {
            _pausePanel.SetActive(false);
            _gameOverPanel.SetActive(true);
        }

        private void PauseButtonOn()
        {
            _button.interactable = true;
        }

        private void PauseButtonOff()
        {
            _button.interactable = false;
        }

        public void SettingsPanelOn()
        {
            _settingsPanel.SetActive(true);
        }
        public void SettingsPanelOff()
        {
            _settingsPanel.SetActive(false);
        }

    }
}
