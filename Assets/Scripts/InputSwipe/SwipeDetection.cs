using UnityEngine;

namespace InputSwipe
{
   public class SwipeDetection : MonoBehaviour
   {
      [SerializeField] 
      private float minimumDistance = .2f;

      [SerializeField]
      private float maxTime = 1f;

      [SerializeField, Range(0f, 1f)] 
      private float directionThreshold = .9f;
         
   
      private InputPrincipal _inputPrincipal;
      private Vector2 _startPosition;
      private Vector2 _endPosition;
      private float _startTime;
      private float _endTime;
      private void Awake()
      {
         _inputPrincipal = InputPrincipal.Instance;
      }

      private void OnEnable()
      {
         _inputPrincipal.OnStartTouch += SwipeStart;
         _inputPrincipal.OnEndTouch += SwipeEnd;
      }

      private void OnDisable()
      {
         _inputPrincipal.OnStartTouch -= SwipeStart;
         _inputPrincipal.OnEndTouch -= SwipeEnd;
      }

      private void SwipeStart(Vector2 position, float time)
      {
         _startPosition = position;
         _startTime = time;
      }
      private void SwipeEnd(Vector2 position, float time)
      {
         _endPosition = position;
         _endTime = time;
         DetectSwipe();
      }

      private void DetectSwipe()
      {
         if (Vector3.Distance(_startPosition, _endPosition) >= minimumDistance &&
             (_endTime - _startTime) <= maxTime)
         {
            //Debug.DrawLine(_startPosition, _endPosition, Color.magenta, 5f);
            Debug.Log("Swipe Detection");
            var direction = _endPosition - _startPosition;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
         }
      }

      private void SwipeDirection(Vector2 direction)
      {
         if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
            Debug.Log("Swipe up");
         else if(Vector2.Dot(Vector2.down, direction) > directionThreshold)
            Debug.Log("Swipe down");
         else if(Vector2.Dot(Vector2.left, direction) > directionThreshold)
            Debug.Log("Swipe left");
         else if(Vector2.Dot(Vector2.right, direction) > directionThreshold)
            Debug.Log("Swipe right");
      }
   }
}
