using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Random = UnityEngine.Random;

public class TorusSectors : MonoBehaviour
{
    [SerializeField] private SectorTuner _sectorsTuner;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Materials _materials;
    [SerializeField][Range(0, 100)] private int _createSectorChance = 20;
    
    private const int SectorAmount = 5;
    private const int DefaultAngel = 72;
    
    private List<GameObject> _tunedSectors;
    
    private void Start()
    {
        _tunedSectors = new List<GameObject>();
    }

    public List<GameObject> Assembly()
    {
        _tunedSectors.Clear();
        for (var i = 0; i < SectorAmount; i++)
        {
            var index = Random.Range(0, SectorAmount);
            var material = _materials.GetMaterial(index); 
            var chance = Random.Range(0, 101);

            if (chance <= _createSectorChance) continue;
            var angel = i * DefaultAngel;
            var position = _spawnPoint.position;
            _tunedSectors.Add(_sectorsTuner.TuneSector(position, material, angel, index));
        }

        return _tunedSectors;
    }

}
