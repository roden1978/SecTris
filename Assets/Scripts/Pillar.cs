using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pillar : MonoBehaviour
{
    [SerializeField] private TorusSectors _torusSectors;
    [SerializeField] private Pool _pool;
    [SerializeField] private Game _game;
    [SerializeField] private Bucket _bucket;

    public event Action<List<GameObject>> OnNewAssembly; 

    private List<GameObject> _moved;
    private List<GameObject> _active;

    private Coroutine _spawn;

    private float _bucketHeight;

    private void Start()
    {
        _moved = new List<GameObject>();
        _active = new List<GameObject>();
    }

    private void OnEnable()
    {
        _game.OnGameOver += StopGame;
        _game.OnGameStart += GameStartSpawn;
        _bucket.OnChangeBucketHeight += ChangeBucketHeight;
    }

    private void OnDisable()
    {
        _game.OnGameOver -= StopGame;
        _game.OnGameStart -= GameStartSpawn;
        _bucket.OnChangeBucketHeight -= ChangeBucketHeight;
    }

    private void GameStartSpawn()
    {
        _spawn = StartCoroutine(SpawnSectors(.1f)); 
    }
    private IEnumerator SpawnSectors(float delay)
    {
        while (true)
        {
            _active = _pool.GetAllActive();

            foreach (var sector in _active)
            {
                var positionY = sector.transform.position.y;
                if (!sector.TryGetComponent(out Rigidbody sectorsRigidbody)) continue;
                if (positionY > _bucketHeight && sectorsRigidbody.isKinematic == false)
                    _moved.Add(sector);
            }

            if (_moved.Count == 0)
            {
                var sectors = _torusSectors.Assembly();
                OnNewAssembly?.Invoke(sectors);
            }

            _moved.Clear();

            yield return new WaitForSeconds(delay);
        }
    }

    private void StopGame()
    {
        StopCoroutine(_spawn);
    }

    private void ChangeBucketHeight(float height)
    {
        _bucketHeight = height;
    }
}

