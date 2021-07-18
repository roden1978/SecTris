using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSwipe
{
    [DefaultExecutionOrder(-1)]
    public class InputPrincipal : Singleton<InputPrincipal>
    {
        private SectorControls _sectorControls;
        private Camera _mainCamera;

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
        }

        private void OnDisable() 
        {
            _sectorControls.Disable();
        }

        private void Start()
        {
            _sectorControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
            _sectorControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            OnStartTouch?.Invoke(ScreenToWorld(_mainCamera, 
                    _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>()), 
                (float)context.startTime);
        }
        
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            OnEndTouch?.Invoke(ScreenToWorld(_mainCamera, 
                    _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>()),
                (float)context.time);
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
