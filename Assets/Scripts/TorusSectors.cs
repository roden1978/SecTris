using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Random = UnityEngine.Random;

public class TorusSectors : MonoBehaviour
{
    [SerializeField] private SectorTuner sectorsTuner;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Materials materials;
    
    private List<GameObject> _tunedSectors;
    
    private const int SectorAmount = 5;
    private const int DefaultAngel = 72;
    private const int Border = 20;

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
            var material = materials.GetMaterial(index);
            var chance = Random.Range(0, 101);

            if (chance <= Border) continue;
            var angel = i * DefaultAngel;
            var position = spawnPoint.transform;
            _tunedSectors.Add(sectorsTuner.TuneSector(position, material, angel, index));
        }

        return _tunedSectors;
    }

}
