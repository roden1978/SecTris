using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pillar : MonoBehaviour
{
    [SerializeField] private TorusSectors torusSectors;
    [SerializeField] private Pool pool;
    [SerializeField] private Game game;

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
        game.OnGameOver += StopGame;
        game.OnGameStart += GameStartSpawn;
        game.OnChangeBucketHeight += ChangeBucketHeight;
    }

    private void OnDisable()
    {
        game.OnGameOver -= StopGame;
        game.OnGameStart -= GameStartSpawn;
        game.OnChangeBucketHeight -= ChangeBucketHeight;
    }

    private void GameStartSpawn()
    {
        _spawn = StartCoroutine(SpawnSectors(.1f)); 
    }
    private IEnumerator SpawnSectors(float delay)
    {
        while (true)
        {
            _active = pool.GetAllActive();

            foreach (var sector in _active)
            {
                //var sectorRigidbody = sector.transform.GetComponent<Rigidbody>();
                var positionY = sector.transform.position.y;
                if(positionY > _bucketHeight) //sectorRigidbody.isKinematic == false && 
                    _moved.Add(sector);                
            }

            if (_moved.Count == 0)
            {
                var sectors = torusSectors.Assembly();
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

