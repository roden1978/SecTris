using System;
using System.Collections;
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

      [SerializeField] private GameObject trail;
      [SerializeField, Range(0f, 1f)] private float trailOffsetY = 0.5f;
      private const float TrailOffsetZ = - 1f;
      

      private Coroutine _coroutine;   
      private InputPrincipal _inputPrincipal;
      private Vector2 _startPosition;
      private Vector2 _endPosition;
      private float _startTime;
      private float _endTime;

      public event Action OnSwipeRight;
      public event Action OnSwipeLeft;
      public event Action OnSwipeDown;
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
         trail.SetActive(true);
         trail.transform.position = position;
         _coroutine = StartCoroutine(Trail());
      }

      private IEnumerator Trail()
      {
         while (true)
         {
            var trailPosition = new Vector3(_inputPrincipal.PrimaryPosition().x, 
               _inputPrincipal.PrimaryPosition().y + trailOffsetY, TrailOffsetZ);
            trail.transform.position = trailPosition;
            yield return null;
         }
      }
      private void SwipeEnd(Vector2 position, float time)
      {
         trail.SetActive(false);
         StopCoroutine(_coroutine);
         _endPosition = position;
         _endTime = time;
         DetectSwipe();
      }

      private void DetectSwipe()
      {
         if (Vector3.Distance(_startPosition, _endPosition) >= minimumDistance &&
             (_endTime - _startTime) <= maxTime)
         {
            var direction = _endPosition - _startPosition;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
         }
      }

      private void SwipeDirection(Vector2 direction)
      {
        if(Vector2.Dot(Vector2.left, direction) > directionThreshold)
            OnSwipeLeft?.Invoke();
        else if(Vector2.Dot(Vector2.right, direction) > directionThreshold)
            OnSwipeRight?.Invoke();
        else if(Vector2.Dot(Vector2.down, direction) > directionThreshold)
            OnSwipeDown?.Invoke();
      }
   }
}

