using UnityEngine;

namespace InputSwipe
{
    public class SwipeSound : MonoBehaviour
    {
        [SerializeField] private AudioSource swipeSound;
        [SerializeField] private SwipeDetection swipeDetection;

        private void OnEnable()
        {
            swipeDetection.OnSwipeLeft += Play;
            swipeDetection.OnSwipeRight += Play;
        
        }

        private void OnDisable()
        {
            swipeDetection.OnSwipeLeft -= Play;
            swipeDetection.OnSwipeRight -= Play;
        }

        private void Play()
        {
            swipeSound.Play();
        }
    }
}
