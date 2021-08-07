using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private Game _game;
        [SerializeField] private Slider _volume;
        [SerializeField] private Toggle _mute;

        private SettingsData _settingsData;
        
        public event Action<SettingsData> OnSettingsDataChange;
        public event Action OnSettingsPanelOpen;
        
        private const string Master = "MasterVolume"; 
        private const string Music = "MusicVolume";

        private const float MaxValue = 0;
        private const float MinValue = -80;

        private void OnEnable()
        {
            _game.OnNewSettingsData += ChangeSettings;
            OnSettingsPanelOpen?.Invoke();
        }

        private void OnDisable()
        {
            _game.OnNewSettingsData -= ChangeSettings;
        }

        public void Mute(bool enable)
        {
            _mixer.audioMixer.SetFloat(Music, enable ? MinValue : MaxValue);
        }

        public void ChangeVolume(float value)
        {
            _mixer.audioMixer.SetFloat(Master, Mathf.Lerp(MinValue, MaxValue, value));
            
        }

        public void Save()
        {
            _settingsData._mute = _mute.isOn ? 1 : 0;
            _settingsData._volume = _volume.value;
            OnSettingsDataChange?.Invoke(_settingsData);
        }

        private void ChangeSettings(SettingsData settings)
        {
            _settingsData = settings;
            _volume.value = _settingsData._volume;
            _mute.isOn = _settingsData._mute != 0;
        }
    }
}
