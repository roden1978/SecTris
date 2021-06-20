using System;
using UnityEngine;

public class Sector : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;
    
    private Material _material;
    private float _angel;
    private int _colorIndex;
    //private Vector3 _center;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public void SetColorIndex(int index)
    {
        _colorIndex = index;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_center.position,  _left.position - _center.position);
        Gizmos.DrawRay(_center.position,  _right.position - _center.position);
        //Gizmos.DrawRay(_center.position, _right.position);
        //Gizmos.DrawRay(_center.position, _meshRenderer.);
        //Gizmos.DrawRay(_meshRenderer.bounds.center, transform.position);
    }
}
