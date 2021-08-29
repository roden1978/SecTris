using UnityEngine;
using UnityEngine.Audio;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;
        
        private const string Master = "MasterVolume"; 
        private const float MinValue = -80;
        
        private float _currentVolume;
        
        private void OnEnable()
        {
            _mixer.audioMixer.GetFloat(Master, out _currentVolume);
            _mixer.audioMixer.SetFloat(Master, MinValue);
        }

        private void OnDisable()
        {
            _mixer.audioMixer.SetFloat(Master, _currentVolume);
        }
    }
}
