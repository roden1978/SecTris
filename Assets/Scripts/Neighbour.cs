using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Neighbour
{
    private readonly List<GameObject> _list;

    private readonly IMaxFixedLevel _maxFixedLevel;

    //private List<GameObject> _levelList;
    //private readonly List<GameObject> _findSectors;
    private readonly SectorComparer _sectorComparer;
    private List<GameObject> _sectors;
    private const int StartLevel = 1;
    private const int SectorsInTorAmount = 5;
    private GameObject[,] _bucket; 

    public Neighbour(List<GameObject> list)
    {
        _list = list;
        _maxFixedLevel = new MaxFixedLevel(_list);
        //_levelList = new List<GameObject>();

        _sectors = new List<GameObject>();
        _sectorComparer = new SectorComparer();
    }

    public void Find()
    {
        var levelsAmount = _maxFixedLevel.Value() + 1;
        _bucket = new GameObject[levelsAmount,SectorsInTorAmount];
        _bucket = FillBucket(levelsAmount);
    }

    /*private List<GameObject> FindSectorsHorizontalLevel(GameObject[,] bucket)
    {
        var findSectors = new List<GameObject>();
        foreach (var item in list)
        {
            var sectorSource = item.GetComponent<Sector>();
            //var result = sectorSource.CastLeft();
            if (!sectorSource.CastLeft()) continue;
            var hit = sectorSource.GetHitLeft();
            var sectorTarget = hit.collider.gameObject.GetComponent<Sector>();
            if (sectorSource.GetColorIndex() != sectorTarget.GetColorIndex())
                findSectors.Clear();
            else
            {
                findSectors.Add(item);
                findSectors.Add(sectorTarget.gameObject);
            }
        }

        return findSectors;
    }*/

    private GameObject[,] FillBucket(int levelsAmount)
    {
        for (var levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
        {
            foreach (var item in _list)
            {
                var meshRenderer = item.GetComponent<MeshRenderer>();
                var sectorHeight = meshRenderer.bounds.size.y;
                var level = Mathf.RoundToInt(item.transform.position.y / sectorHeight);
            
                if (levelIndex == level)
                {
                    var angel = Mathf.RoundToInt(item.transform.eulerAngles.y);
                    var index = new SectorsIndex(angel).Value();
                
                    _bucket[levelIndex, index] = item;
                }
            }
        }
        return _bucket;
    }
    
}

/*
 * _sectors = FindSectorsHorizontalLevel(levelList)
                .Distinct(_sectorComparer)
                .ToList();
            if (_sectors.Count < 3) _sectors.Clear();

            /*if (i > StartLevel)
            {
                foreach (var item in _levelList)
                {
                    var sourceSector = item.GetComponent<Sector>();
                    var result = sourceSector.CastDown();
                    if (result == 0) continue;
                    var hits = sourceSector.GetHitDown();
                    if(hits.Length > 2)
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.collider.transform.TryGetComponent(out Sector targetSector))
                                if (targetSector.GetColorIndex() == sourceSector.GetColorIndex())
                                    _findSectors.Add(targetSector.gameObject);
                            //Debug.Log(targetSector.GetColorIndex());
                        }
                    }

                    if (_findSectors.Count > 1)
                        _findSectors.Add(item);
                }

                if (_findSectors.Count < 3)
                    _findSectors.Clear();
                else
                    foreach (var item in _findSectors)
                    {
                        _sectors.Add(item);
                    }
            }

            _findSectors.Clear();

foreach (var item in _sectors)
{
    item.SetActive(false);
}

//_levelList.Clear();
_sectors.Clear();
*/