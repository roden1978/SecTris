using System.Collections;
using InputSwipe;
using UnityEngine;

public class Sector : MonoBehaviour
{
   [SerializeField, Range(0f, 1000f)] private float rotateSpeed = 500;
    private SwipeDetection _swipeDetection;
    private Pillar _pillar;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private RaycastHit _hitLeft;
    private RaycastHit[] _hitDown;
    
    private float _size;
    private int _colorIndex;
    private Quaternion _nextDegree;

    private bool _isLeft;
    private bool _isRight;
    private bool _collision = true;

    private const int Left = 1;
    private const int Right = -1;
    //private int _level;
    private const float Angel = -36;
    private const float RotateDegrees = 72;
    private const float HitLeftDistance = 1f;
    private float _drag;
    private const int LayerMaskSector = 1 << 9;
    private const int LayerMaskPlatform = 1 << 8;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _pillar = FindObjectOfType<Pillar>();
        _swipeDetection = FindObjectOfType<SwipeDetection>();
        _size = _meshRenderer.bounds.size.y;
        _rigidbody = GetComponent<Rigidbody>();
        _hitDown = new RaycastHit[2];
        _drag = _rigidbody.drag;
    }

    private void RotateSectors(float degrees, int direction)
    {
        
        var minPoint = _pillar.BucketHeight;
        if (!_rigidbody.isKinematic && transform.position.y > minPoint && transform.rotation != _nextDegree)
        {
            var originalRot = transform.rotation;    
            transform.rotation = Quaternion.Slerp(originalRot, 
                originalRot * Quaternion.AngleAxis(degrees * direction, Vector3.up),
                Time.deltaTime * rotateSpeed);
        }
    }

    public bool CastLeft()
    {
        var center = _meshRenderer.bounds.center;
        var position = transform.position;
        var originPoint = new Vector3(position.x, center.y, position.z);
        var directionOrigin = originPoint - center;
        var direction = Quaternion.AngleAxis(Angel, Vector3.up) * directionOrigin;

        var result = Physics.Raycast(center, direction, out _hitLeft, HitLeftDistance, LayerMaskSector);
        return result;
    }

    public int CastDown()
    {
        var center = _meshRenderer.bounds.center;
        var hitDownDistance = _size * 2f;
        var size = Physics.RaycastNonAlloc(center, Vector3.down, _hitDown, hitDownDistance, LayerMaskSector);
        Debug.DrawRay(center, Vector3.down, Color.red);
        Debug.Log($"Size {size}");
        return size;
    }

   private void OnCollisionEnter(Collision other)
    {
        if(_collision)
        {
            if (_rigidbody.isKinematic) return;
            var contactPoint = other.GetContact(0);
            var center = _meshRenderer.bounds.center;
            
            if (contactPoint.point.y < center.y)
            {
                _rigidbody.isKinematic = true;
                _rigidbody.drag = _drag;
            }
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

    private void CheckBottom()
    {
        var bounds = _meshRenderer.bounds;
        var center = bounds.center;
        var maxDistance = bounds.size.y;
        var result = Physics.Raycast(center, Vector3.down, maxDistance,
            LayerMaskSector | LayerMaskPlatform);
        if (result == false)
            _rigidbody.isKinematic = false;
    }

    private void OnEnable()
    {
        _swipeDetection.OnSwipeRight += RotateRight;
        _swipeDetection.OnSwipeLeft += RotateLeft;
        _pillar.OnGameOver += OffCollision;
        _collision = true;
    }

    private void OnDisable()
    {
        ResetSector();
        _swipeDetection.OnSwipeRight -= RotateRight;
        _swipeDetection.OnSwipeLeft -= RotateLeft;
        _pillar.OnGameOver -= OffCollision;
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

    

    private void ResetSector()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.drag = _drag;
        //_level = 0;
    }

    public void SetColorIndex(int index)
    {
        _colorIndex = index;
    }
    public int GetColorIndex() => _colorIndex;
    public RaycastHit GetHitLeft() => _hitLeft;
    public RaycastHit[] GetHitDown()
    {
        return _hitDown;
    }
    
    
}