using System.Collections;
using System.Collections.Generic;
using InputSwipe;
using UnityEngine;

public sealed class Pillar : MonoBehaviour
{
    [SerializeField] private TorusSectors torusSectors;
    [SerializeField] private Pool pool;
    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private Game game;

    private List<GameObject> _moved;
    private List<GameObject> _active;

    private Coroutine _spawn;

    private void Start()
    {
        _moved = new List<GameObject>();
        _active = new List<GameObject>();
    }

    private void OnEnable()
    {
        game.OnGameOver += StopGame;
        game.OnGameStart += GameStartSpawn;
    }

    private void OnDisable()
    {
        game.OnGameOver -= StopGame;
        game.OnGameStart -= GameStartSpawn;
    }

    private void GameStartSpawn()
    {
        _spawn = StartCoroutine(SpawnSectors(.1f)); 
    }
    private IEnumerator SpawnSectors(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            _active = pool.GetAllActive();
            foreach (var sector in _active)
            {
                var sectorRigidbody = sector.transform.GetComponent<Rigidbody>();
                if(sectorRigidbody.isKinematic == false)
                    _moved.Add(sector);                
            }
            
            if (_moved.Count == 0)
            {
                torusSectors.Assembly();
            }
            
            _moved.Clear();
            
        }
        //_moved.Clear();
    }

    private void StopGame()
    {
        StopCoroutine(_spawn);
    }
}