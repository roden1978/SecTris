using System;
using UnityEngine;

public class Sector : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _angel;
    private Transform _defaultParent;
    
    private MeshRenderer _renderer;
    private Vector3 _center;
    

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _defaultParent = transform.parent;
        Debug.Log("Awake");
    }

    public Transform GetDefaultParent()
    {
       return _defaultParent;
    }

    public void SetDefaultParent(Transform parent)
    {
        _defaultParent = parent;
    }
}
