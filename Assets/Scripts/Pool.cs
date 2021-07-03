using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour 
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _capacity;
        
    private List<GameObject> _pool;
    private List<GameObject> _activeObjects;
        
    private void Start ()
    {
        _pool = new List<GameObject>();
        _activeObjects = new List<GameObject>();
        Initialize(_prefab, _capacity);
    }

    private void Initialize(GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            var pooledObject = Instantiate(prefab, transform);
            pooledObject.SetActive(false);
            _pool.Add(pooledObject);
        }
    }
       
    public GameObject GetPooledObject()
    {
        foreach (var item in _pool)
        {
            if (item.activeInHierarchy == false)
            {
                item.SetActive(true);
                return item;
            }               
        }
            
        var pooledObject = Instantiate(_prefab, transform);
        _pool.Add(pooledObject);

        return pooledObject;
    }

    public List<GameObject> GetAllActive()
    {
        _activeObjects.Clear();
        foreach (var item in _pool)
        {
            if(item.activeInHierarchy)
                _activeObjects.Add(item);    
        }

        return _activeObjects;
    }
       
}