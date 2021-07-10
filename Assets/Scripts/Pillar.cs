using System;
using System.Collections;
using System.Collections.Generic;
using InputSwipe;
using UnityEngine;

public sealed class Pillar : MonoBehaviour
{
    [SerializeField] private TorusSectors torusSectors;
    [SerializeField] private Pool pool;
    [SerializeField] private SwipeDetection swipeDetection;
    
    private Neighbour _neighbour;
    private IMaxYPosition _maxYPosition;
    
    private bool _create;
    
    private List<GameObject> _moved;
    private List<GameObject> _fixed;
    private List<GameObject> _active;

    private const float StopPoint = 5f;

    private void Start()
    {
        _moved = new List<GameObject>();
        _fixed = new List<GameObject>();
        _active = new List<GameObject>();
        _neighbour = new Neighbour(_fixed);
        _maxYPosition = new MaxYPosition(_fixed);
        _create = true;
        StartCoroutine(SpawnSectors(.1f));
        StartCoroutine(RemoveNotActive(.5f));
    }

    private void OnEnable()
    {
        swipeDetection.OnSwipeRight += RotateRight;
        swipeDetection.OnSwipeLeft += RotateLeft;
    }

    private void OnDisable()
    {
        swipeDetection.OnSwipeRight -= RotateRight;
        swipeDetection.OnSwipeLeft -= RotateLeft;
    }

    private IEnumerator RemoveNotActive(float delay)
    {
        while (_create)
        {
            yield return new WaitForSeconds(delay);
            _fixed.RemoveAll(NotActive);
        }
    }

    private bool NotActive(GameObject sector)
    {
        return !sector.activeInHierarchy;
    }

    private IEnumerator SpawnSectors(float delay)
    {
        while (_create)
        {
            yield return new WaitForSeconds(delay);

            _active = pool.GetAllActive();
            foreach (var item in _active)
            {
                var rb = item.transform.GetComponent<Rigidbody>();
                if(rb.isKinematic)
                    _fixed.Add(item);
                else
                    _moved.Add(item);                
            }
            
            if (_maxYPosition.Value() > StopPoint)
                _create = false;
            
            _neighbour.Find();
            
            if(_moved.Count == 0)
                torusSectors.Assembly();
            
            _moved.Clear();
            _fixed.Clear();
            
        }
        _fixed.Clear();
        _moved.Clear();
        _active.Clear();
        Debug.Log("Game over");
    }

    public void ResetPools()
    {
        foreach (var item in _active)
        {
            if(item.activeInHierarchy)
                item.SetActive(false);
        }
        _fixed.Clear();
        _moved.Clear();
    }

    private void RotateLeft()
    {
        
    }

    private void RotateRight()
    {
        
    }

}