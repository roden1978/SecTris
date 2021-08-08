using UnityEngine;

namespace UI
{
    public class StartButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _startButtonSound;

        public void Play()
        {
            _startButtonSound.Play();
        }
    }
}
