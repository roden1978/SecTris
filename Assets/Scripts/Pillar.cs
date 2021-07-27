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

    private bool _spawn;
    
    private List<GameObject> _moved;
    private List<GameObject> _active;

    private void Start()
    {
        _moved = new List<GameObject>();
        _active = new List<GameObject>();
    }

    private void OnEnable()
    {
        swipeDetection.OnSwipeDown += Fall;
        game.OnGameOver += StopGame;
        game.OnGameStart += GameStartSpawn;
    }

    private void OnDisable()
    {
        swipeDetection.OnSwipeDown -= Fall;
        game.OnGameOver -= StopGame;
        game.OnGameStart -= GameStartSpawn;
    }

    private void GameStartSpawn()
    {
        _spawn = true;
        StartCoroutine(SpawnSectors(.1f)); 
    }
   

   

    private IEnumerator SpawnSectors(float delay)
    {
        while (_spawn)
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
        _moved.Clear();
    }

    private void Fall()
    {
        var fallingSectors = pool.GetAllActive();
        
        foreach (var sector in fallingSectors)
        {
            var sectorRigidbody = sector.transform.GetComponent<Rigidbody>();
            if(!sectorRigidbody.isKinematic)
                sectorRigidbody.drag = 0;               
        }
      
    }

    private void StopGame()
    {
        _spawn = false;
    }
}