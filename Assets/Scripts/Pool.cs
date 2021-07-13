using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour 
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int capacity;
        
    private List<GameObject> _pool;
    private List<GameObject> _activeObjects;
    private int _index;
        
    private void Start ()
    {
        _pool = new List<GameObject>();
        _activeObjects = new List<GameObject>();
        Initialize(prefab, capacity);
    }

    private void Initialize(GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            var pooledObject = Instantiate(prefab, transform); 
            pooledObject.name = "Sector(" + _index + ")";
            pooledObject.SetActive(false);
            _pool.Add(pooledObject);
            _index++;
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
            
        var pooledObject = Instantiate(prefab, transform);
        pooledObject.name = "Sector(" + _index + ")";
        _index++;
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