using UnityEngine;

namespace InputSwipe
{
    public class FallDownSound : MonoBehaviour
    {
        [SerializeField] private AudioSource fallDownSound;
        [SerializeField] private SwipeDetection swipeDetection;

        private void OnEnable()
        {
            swipeDetection.OnSwipeDown += Play;
        }

        private void OnDisable()
        {
            swipeDetection.OnSwipeDown -= Play;
        }

        private void Play()
        {
            fallDownSound.Play();
        }
    }
}
