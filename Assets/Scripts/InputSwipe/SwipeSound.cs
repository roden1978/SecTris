using UnityEngine;

namespace InputSwipe
{
    public class SwipeSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _swipeSound;
        [SerializeField] private SwipeDetection _swipeDetection;

        private void OnEnable()
        {
            _swipeDetection.OnSwipeLeft += Play;
            _swipeDetection.OnSwipeRight += Play;
        
        }

        private void OnDisable()
        {
            _swipeDetection.OnSwipeLeft -= Play;
            _swipeDetection.OnSwipeRight -= Play;
        }

        private void Play()
        {
            _swipeSound.Play();
        }
    }
}
