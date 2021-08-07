using System;
using UI;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Background _background;
    [SerializeField] private Bucket _bucket;
    [SerializeField] private Scores _scores;
    [SerializeField] private Settings _settings;
    [SerializeField] private AudioSource _backgroundMusic;

    private IStorage _storage;

    private GameData _gameData;
    private SettingsData _settingsData;

    private const string SettingsFileName = "settings.dat";
    private const string GameDataFileName = "gamedata.dat";
    public event Action OnGameOver;
    public event Action OnGameStart;
    public event Action<SettingsData> OnNewSettingsData; 
    public event Action<GameData> OnNewGameData;

    private void Awake()
    {
        _gameData = new GameData();
        _settingsData = new SettingsData();
        _storage = new Storage();
    }

    private void Start()
    {
        _settingsData = (SettingsData) LoadSettingsData(_settingsData);
        UpdateMixerSettings(_settingsData);

        _gameData = (GameData) LoadGameData(_gameData);
        OnNewGameData?.Invoke(_gameData);

        Time.timeScale = 0;
        _mainPanel.SetActive(true);
    }

    public void StartGame()
    {
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
        _bucket.OnOverflowBucket += StopGame;
        _scores.OnHighScoreChange += SaveGameData;
        _settings.OnSettingsDataChange += SaveSettingsData;
        _settings.OnSettingsPanelOpen += UpdateSettingsData;
    }

    private void OnDisable()
    {
        _bucket.OnOverflowBucket -= StopGame;
        _scores.OnHighScoreChange -= SaveGameData;
        _settings.OnSettingsDataChange -= SaveSettingsData;
        _settings.OnSettingsPanelOpen -= UpdateSettingsData;

    }

    public void GameOver()
    {
        Time.timeScale = 1;
        OnGameOver?.Invoke();
        _gameOverPanel.SetActive(false);
    }

    private void StopGame()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void ChangeBackground()
    {
        _background.ChangeBackground();
    }

    private void SaveGameData(GameData gameData)
    {
        _storage.Save(gameData, GameDataFileName);
    }

    private void SaveSettingsData(SettingsData settingsData)
    {
        _storage.Save(settingsData, SettingsFileName);
    }

    private object LoadGameData(object data)
    {
        return (GameData) _storage.Load(data, GameDataFileName);
    }

    private object LoadSettingsData(object data)
    {
        return (SettingsData) _storage.Load(data, SettingsFileName);
    }

    private void UpdateSettingsData()
    {
        OnNewSettingsData?.Invoke(_settingsData);        
    }

    private void UpdateMixerSettings(SettingsData settingsData)
    {
        var mute = settingsData._mute != 0;
        _settings.Mute(mute);
        _settings.ChangeVolume(settingsData._volume);
        StartBackgroundMusic();
    }

    private void StartBackgroundMusic()
    {
        _backgroundMusic.Play();
    }
}
