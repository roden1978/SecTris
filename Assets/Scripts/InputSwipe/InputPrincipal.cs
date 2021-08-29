using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSwipe
{
    [DefaultExecutionOrder(-1)]
    public class InputPrincipal : Singleton<InputPrincipal>
    {
        [SerializeField] private Game _game;
        [SerializeField] private Bucket _bucket;
        
        private SectorControls _sectorControls;
        private Camera _mainCamera;
        private bool _isControl;

        public event Action<Vector2, float> OnStartTouch;
        public event Action<Vector2, float> OnEndTouch;
        private void Awake()
        {
            _sectorControls = new SectorControls();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _sectorControls.Enable();
            _game.OnGameStart += StartControl;
            _bucket.OnOverflowBucket += EndControl;
        }

        private void OnDisable() 
        {
            _sectorControls.Disable();
            _game.OnGameStart -= StartControl;
            _bucket.OnOverflowBucket -= EndControl;
        }

        private void Start()
        {
            _sectorControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
            _sectorControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        }

        private void StartControl()
        {
            _isControl = true;
        }

        private void EndControl()
        {
            _isControl = false;
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            if (!_isControl) return;
            var position = ScreenToWorld(_mainCamera,
                _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            OnStartTouch?.Invoke(position, (float)context.startTime);

        }
        
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            if (!_isControl) return;
            var position = ScreenToWorld(_mainCamera,
                _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>());
               
            OnEndTouch?.Invoke(position, (float)context.time);

        }

        public Vector2 PrimaryPosition()
        {
            return ScreenToWorld(_mainCamera,
                _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
        
        private Vector3 ScreenToWorld(Camera mainCamera, Vector3 position)
        {
            position.z = mainCamera.nearClipPlane - mainCamera.transform.position.z;
            return mainCamera.ScreenToWorldPoint(position);
        }
    }
}
