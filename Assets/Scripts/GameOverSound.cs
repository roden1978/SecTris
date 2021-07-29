using UnityEngine;

public class GameOverSound : MonoBehaviour
{
   [SerializeField] private Game _game;
   [SerializeField] private AudioSource _audioSource;

   private void OnEnable()
   {
      _game.OnStopGame += Play;
   }

   private void OnDisable()
   {
      _game.OnStopGame -= Play;
   }

   private void Play()
   {
      _audioSource.Play();
   }
}
