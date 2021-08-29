using UnityEngine;

namespace InputSwipe
{
    public class FallDownSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _fallDownSound;
        [SerializeField] private SwipeDetection _swipeDetection;

        private void OnEnable()
        {
            _swipeDetection.OnSwipeDown += Play;
        }

        private void OnDisable()
        {
            _swipeDetection.OnSwipeDown -= Play;
        }

        private void Play()
        {
            _fallDownSound.Play();
        }
    }
}
