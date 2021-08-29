using System;
using System.Collections;
using UnityEngine;

namespace InputSwipe
{
   public class SwipeDetection : MonoBehaviour
   {
      [SerializeField] private float _minimumDistance = .2f;
      [SerializeField] private float _maxTime = 1f;
      [SerializeField, Range(0f, 1f)] private float _directionThreshold = .9f;
      [SerializeField] private GameObject _trail;
      [SerializeField, Range(0f, 1f)] private float _trailOffsetY = 0.5f;
      
      private const float TrailOffsetZ = - 1f;

      private Coroutine _trailCoroutine;   
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
         _trail.transform.position = new Vector3(position.x, position.y + _trailOffsetY, TrailOffsetZ);
         _trail.SetActive(true);
         _trailCoroutine = StartCoroutine(Trail());
      }

      private IEnumerator Trail()
      {
         while (true)
         {
            var trailPosition = new Vector3(_inputPrincipal.PrimaryPosition().x, 
               _inputPrincipal.PrimaryPosition().y + _trailOffsetY, TrailOffsetZ);
            _trail.transform.position = trailPosition;
            yield return null;
         }
      }
      private void SwipeEnd(Vector2 position, float time)
      {
         _trail.SetActive(false);
         StopCoroutine(_trailCoroutine);
         _endPosition = position;
         _endTime = time;
         DetectSwipe();
      }

      private void DetectSwipe()
      {
         if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance &&
             (_endTime - _startTime) <= _maxTime)
         {
            var direction = _endPosition - _startPosition;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
         }
      }

      private void SwipeDirection(Vector2 direction)
      {
        if(Vector2.Dot(Vector2.left, direction) > _directionThreshold)
            OnSwipeLeft?.Invoke();
        else if(Vector2.Dot(Vector2.right, direction) > _directionThreshold)
            OnSwipeRight?.Invoke();
        else if(Vector2.Dot(Vector2.down, direction) > _directionThreshold)
            OnSwipeDown?.Invoke();
      }
   }
}

