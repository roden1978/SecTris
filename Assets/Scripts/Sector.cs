using InputSwipe;
using UnityEngine;

public class Sector : MonoBehaviour
{
    private SwipeDetection _swipeDetection;
    private Game _game;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    
    private float _drag;
    private float _sectorHeight;
    private int _colorIndex;
    
    private bool _collision = true;

    private const int LayerMaskSector = 1 << 9;
    private const int LayerMaskPlatform = 1 << 8;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _game = FindObjectOfType<Game>();
        _swipeDetection = FindObjectOfType<SwipeDetection>();
        _rigidbody = GetComponent<Rigidbody>();
        _drag = _rigidbody.drag;
        _sectorHeight = _meshRenderer.bounds.size.y;
    }

   private void OnCollisionEnter(Collision other)
   {
       if (!_collision) return;
       if (_rigidbody.isKinematic) return;
       var contactPoint = other.GetContact(0);
       var center = _meshRenderer.bounds.center;

       if (!(contactPoint.point.y < center.y)) return;
       _rigidbody.isKinematic = true;
       _rigidbody.drag = _drag;
   }
    private void FixedUpdate()
    {
        CheckBottom();
    }

   public int GetLevel()
    {
        return Mathf.RoundToInt(transform.position.y / _sectorHeight);
    }

    private void CheckBottom()
    {
        var bounds = _meshRenderer.bounds;
        var center = bounds.center;
        var result = Physics.Raycast(center, Vector3.down, _sectorHeight,
            LayerMaskSector | LayerMaskPlatform);
        if (result == false)
            _rigidbody.isKinematic = false;
    }

    private void OnEnable()
    {
        _swipeDetection.OnSwipeDown += Fall;
        _game.OnGameOver += OffCollision;
        _collision = true;
    }

    private void OnDisable()
    {
        ResetSector();
        _swipeDetection.OnSwipeDown -= Fall;
        _game.OnGameOver -= OffCollision;
    }

    private void OffCollision()
    {
        _collision = false;
    }
    
    private void Fall()
    {
        if(!_rigidbody.isKinematic)
            _rigidbody.drag = 0;               
    }

    private void ResetSector()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.drag = _drag;
        _collision = false;
    }

    public void SetColorIndex(int index)
    {
        _colorIndex = index;
    }
    public int GetColorIndex() => _colorIndex;
   
}