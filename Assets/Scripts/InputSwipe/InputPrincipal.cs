using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSwipe
{
    [DefaultExecutionOrder(-1)]
    public class InputPrincipal : Singleton<InputPrincipal>
    {
        private SectorControls _sectorControls;
        private Camera _mainCamera;
        public delegate void StartTouch(Vector2 position, float time);
        public delegate void EndTouch(Vector2 position, float time);

        public event StartTouch OnStartTouch;
        public event EndTouch OnEndTouch;
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
            if (OnStartTouch != null)
                OnStartTouch(Utils.ScreenToWorld(_mainCamera, 
                    _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>()), 
                    (float)context.startTime);
        }
        
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnEndTouch != null)
                OnEndTouch(Utils.ScreenToWorld(_mainCamera, 
                    _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>()),
                    (float)context.time);
        }

        public Vector2 PrimaryPosition()
        {
            return Utils.ScreenToWorld(_mainCamera, _sectorControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        }

    }
}
