using UnityEngine;

public class GameOverSound : MonoBehaviour
{
   [SerializeField] private Bucket _bucket;
   [SerializeField] private AudioSource _audioSource;

   private void OnEnable()
   {
      _bucket.OnOverflowBucket += Play;
   }

   private void OnDisable()
   {
      _bucket.OnOverflowBucket -= Play;
   }

   private void Play()
   {
      _audioSource.Play();
   }
}
