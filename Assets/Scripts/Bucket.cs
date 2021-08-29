using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private Pool _pool;
    [SerializeField] private Neighbor _neighbor;
    [SerializeField] private Game _game;
    [SerializeField][Range(0.1f, 0.5f)] private float _updateBucketDelay = 0.1f;
    [SerializeField][Range(0.1f, 1f)] private float _removeNotActiveDelay = 0.5f;
    
    private const float StopPoint = 5f;
    public event Action OnOverflowBucket;
    public event Action<float> OnChangeBucketHeight;

    private IBucketHeight _bucketHeight;
    private List<GameObject> _bucket;
        
    private Coroutine _updateBucket;
    private Coroutine _removeNotActive;
    
    private int _prevFixedCount;
    
    private float _prevBucketHeight;
    private float _currentBucketHeight;
    private void Awake()
    {
        _bucket = new List<GameObject>();
        _bucketHeight = new BucketHeight(_bucket);
    }

    private void OnEnable()
    {
        _game.OnGameStart += StartUpdateBucket;
        _game.OnGameOver += StopUpdateBucket;
    }

    private void OnDisable()
    {
        _game.OnGameStart -= StartUpdateBucket;
        _game.OnGameOver -= StopUpdateBucket;
    }
    private void StartUpdateBucket()
    {
        _currentBucketHeight = 0;
        _updateBucket = StartCoroutine(UpdateBucket(_updateBucketDelay));
        _removeNotActive = StartCoroutine(RemoveNotActive(_removeNotActiveDelay));
    }

    private void StopUpdateBucket()
    {
        StopCoroutine(_updateBucket);
        StopCoroutine(_removeNotActive);
        _currentBucketHeight = 0;
        _prevFixedCount = 0;
        _bucket.Clear();
    }
    private IEnumerator UpdateBucket(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);

            FillBucket();

            _currentBucketHeight = _bucketHeight.Value();
            
            if (Math.Abs(_prevBucketHeight - _currentBucketHeight) > 0)
            {
                _prevBucketHeight = _currentBucketHeight;
                OnChangeBucketHeight?.Invoke(_currentBucketHeight);
            }
            
            if (_currentBucketHeight > StopPoint)
            {
                OnChangeBucketHeight?.Invoke(0f);
                OnOverflowBucket?.Invoke();
            }
            
            if (_prevFixedCount != _bucket.Count)
            {
                _prevFixedCount = _bucket.Count;
                _neighbor.FindNeighborSectors(_bucket);
            }
            
            _prevFixedCount = _bucket.Count;
            _bucket.Clear();
        }
    }

    private void FillBucket()
    {
        var active = _pool.GetAllActive();
        foreach (var sector in active)
        {
            if (sector.transform.TryGetComponent(out Rigidbody sectorRigidbody) && 
                sectorRigidbody.isKinematic)
                _bucket.Add(sector);
        }
    }

    private bool NotActive(GameObject sector)
    {
        return !sector.activeInHierarchy;
    }
    private IEnumerator RemoveNotActive(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            _bucket.RemoveAll(NotActive);
        }
    }

}
