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
    
    
    private Neighbour _neighbour;
    
    
    private bool _spawn;
    
    private List<GameObject> _moved;
    private List<GameObject> _fixed;
    private List<GameObject> _active;

    private void Start()
    {
        _moved = new List<GameObject>();
        _fixed = new List<GameObject>();
        _active = new List<GameObject>();
        _neighbour = new Neighbour(_fixed);
        
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
        StartCoroutine(RemoveNotActive(.5f)); 
    }
    private IEnumerator RemoveNotActive(float delay)
    {
        while (_spawn)
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
        while (_spawn)
        {
            yield return new WaitForSeconds(delay);

            _active = pool.GetAllActive();
            foreach (var sector in _active)
            {
                var sectorRigidbody = sector.transform.GetComponent<Rigidbody>();
                if(!sectorRigidbody.isKinematic)
                    _moved.Add(sector);                
            }
            
            _neighbour.Find();
            
            if(_moved.Count == 0)
                torusSectors.Assembly();
            
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