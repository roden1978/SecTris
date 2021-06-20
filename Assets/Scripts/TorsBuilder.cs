using System;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Random = UnityEngine.Random;

public class TorsBuilder : MonoBehaviour
{
    [SerializeField] private SectorTuner _sectorsTuner;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private Materials _materials;
    
    private List<GameObject> _tunedSectors;
    
    private const int SECTOR_AMOUNT = 5;
    private const int DEFAULT_ANGEL = 72;
    private const int BORDER = 20;

    private void Start()
    {
        _tunedSectors = new List<GameObject>();
    }

    public List<GameObject> BuildingTor()
    {
        _tunedSectors.Clear();
        for (int i = 0; i < SECTOR_AMOUNT; i++)
        {
            var index = Random.Range(0, SECTOR_AMOUNT);
            var material = _materials.GetMaterial(index);
            var chance = Random.Range(0, 101);
            
            if(chance > BORDER)
            {
                var angel = i * DEFAULT_ANGEL;
                var position = _spawnPoint.transform;
                _tunedSectors.Add(_sectorsTuner.TuneSector(position, material, angel, i));
            }
        }

        return _tunedSectors;
    }

}
