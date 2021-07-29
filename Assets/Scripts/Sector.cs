using System.Collections;
using InputSwipe;
using UnityEngine;

public class Sector : MonoBehaviour
{
   [SerializeField, Range(0f, 100)] private float _rotateSpeed = 100;
    private SwipeDetection _swipeDetection;
    private Game _game;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    
    private float _drag;
    private float _sectorHeight;
    private int _colorIndex;
    private Quaternion _nextDegree;

    private bool _isLeft;
    private bool _isRight;
    private bool _collision = true;

    private const int Left = 1;
    private const int Right = -1;
    private const float RotateDegrees = 72;
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

    private void RotateSectors(float degrees, int direction)
    {
        var minPoint = _game.BucketHeight + _sectorHeight;
        if (!_rigidbody.isKinematic && 
            transform.position.y > minPoint &&
            transform.rotation != _nextDegree)
        {
            var originalRot = transform.rotation;    
            transform.rotation = Quaternion.Slerp(originalRot, 
                originalRot * Quaternion.AngleAxis(degrees * direction, Vector3.up),
                Time.deltaTime * _rotateSpeed);
        }
    }
   
   private void OnCollisionEnter(Collision other)
    {
        if(_collision)
        {
            if (_rigidbody.isKinematic) return;
            var contactPoint = other.GetContact(0);
            var center = _meshRenderer.bounds.center;

            if (!(contactPoint.point.y < center.y)) return;
            _rigidbody.isKinematic = true;
            _rigidbody.drag = _drag;
        }
    }

    private IEnumerator SwitchLeft(float delay)
    {
        _isLeft = true;
        yield return new WaitForSeconds(delay);
        _isLeft = false;
    }
    
    private IEnumerator SwitchRight(float delay)
    {
        _isRight = true;
        yield return new WaitForSeconds(delay);
        _isRight = false;
    }

    private void FixedUpdate()
    {
        CheckBottom();
        if (_isRight)
            RotateSectors(RotateDegrees, Right);

        if (_isLeft)
            RotateSectors(RotateDegrees, Left);
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
        _swipeDetection.OnSwipeRight += RotateRight;
        _swipeDetection.OnSwipeLeft += RotateLeft;
        _swipeDetection.OnSwipeDown += Fall;
        _game.OnGameOver += OffCollision;
        _collision = true;
    }

    private void OnDisable()
    {
        ResetSector();
        _swipeDetection.OnSwipeRight -= RotateRight;
        _swipeDetection.OnSwipeLeft -= RotateLeft;
        _swipeDetection.OnSwipeDown -= Fall;
        _game.OnGameOver -= OffCollision;
    }

    private void OffCollision()
    {
        _collision = false;
    }
    
   private void RotateLeft()
    {
        var originalRot = transform.rotation;
        _nextDegree = originalRot * Quaternion.AngleAxis(RotateDegrees * Left, Vector3.up);
        StartCoroutine(SwitchLeft(0.5f));
    }

    private void RotateRight()
    {
        var originalRot = transform.rotation;
        _nextDegree = originalRot * Quaternion.AngleAxis(RotateDegrees * Right, Vector3.up);
        StartCoroutine(SwitchRight(0.5f));
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