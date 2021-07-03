using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    [SerializeField] private int level;
    private Pillar pillar;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private RaycastHit _hitLeft;
    private RaycastHit[] _hitDown;
    
    private float _size;
    private int _colorIndex;
    
    //private Vector3 _center;

    private const float Angel = -36;
    private const float HitLeftDistance = 1f;
    private const int LayerMaskSector = 1 << 9;
    private const int LayerMaskPlatform = 1 << 8;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _size = _meshRenderer.bounds.size.y;
        _rigidbody = GetComponent<Rigidbody>();
        _hitDown = new RaycastHit[2];
        pillar = FindObjectOfType<Pillar>();
    }

    /*private void Update()
    {
        if(_rigidbody.isKinematic)
            level = Mathf.RoundToInt(transform.position.y / _size);
    }*/

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
        if (_rigidbody.isKinematic) return;
        var contactPoint = other.GetContact(0);
        var center = _meshRenderer.bounds.center;
        if (contactPoint.point.y < center.y)
            _rigidbody.isKinematic = true;
    }

    /*private IEnumerator CheckBottom(float delay)
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(delay);
            var bounds = _meshRenderer.bounds;
            var center = bounds.center;
            var maxDistance = bounds.size.y;
            var result = Physics.Raycast(center, Vector3.down, maxDistance,
                                            LayerMaskSector | LayerMaskPlatform);
            if (result == false)
                _rigidbody.isKinematic = false;
        }
    }*/

    private void FixedUpdate()
    {
        var bounds = _meshRenderer.bounds;
        var center = bounds.center;
        var maxDistance = bounds.size.y;
        var result = Physics.Raycast(center, Vector3.down, maxDistance,
            LayerMaskSector | LayerMaskPlatform);
        if (result == false)
            _rigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        ResetSector();
    }

    private void ResetSector()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        level = 0;
    }

    public void SetColorIndex(int index)
    {
        _colorIndex = index;
    }

    public int GetColorIndex() => _colorIndex;

    //public int GetLevel() => level;

    public RaycastHit GetHitLeft() => _hitLeft;

    public RaycastHit[] GetHitDown()
    {
        return _hitDown;
    }
    
    
}