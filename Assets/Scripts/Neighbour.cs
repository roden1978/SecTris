using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Neighbour
{
    private readonly List<GameObject> _list;
    private readonly IMaxFixedLevel _maxFixedLevel;
    private readonly List<GameObject> _levelList;
    private readonly List<GameObject> _findSectors;
    private readonly SectorComparer _sectorComparer;
    private List<GameObject> _sectors;
    private const int StartLevel = 1;

    public Neighbour(List<GameObject> list)
    {
        _list = list;
        _maxFixedLevel = new MaxFixedLevel(_list);
        _levelList = new List<GameObject>();
        _findSectors = new List<GameObject>();
        _sectors = new List<GameObject>();
        _sectorComparer = new SectorComparer();
    }

    public void Find()
    {
        var levelsAmount = _maxFixedLevel.value();
        for (var i = 0; i <= levelsAmount; i++)
        {
            foreach (var item in _list)
            {
                var sector = item.transform.GetComponent<Sector>();
                var level = sector.GetLevel();
                if (i == level)
                {
                    _levelList.Add(item);
                }
            }

            foreach (var item in _levelList)
            {
                var sector = item.transform.GetComponent<Sector>();
                var result = sector.CastLeft();
                if (result)
                {
                    var hit = sector.GetHitLeft();
                    var hitSector = hit.collider.transform.GetComponent<Sector>();
                    if (sector.GetColorIndex() != hitSector.GetColorIndex()) continue;
                    _findSectors.Add(item);
                    _findSectors.Add(hitSector.gameObject);
                }
            }


            _sectors = _findSectors.Distinct(_sectorComparer).ToList();
            _findSectors.Clear();

            if (_sectors.Count < 3) _sectors.Clear();

            if (i > StartLevel)
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

            _levelList.Clear();
            _sectors.Clear();
        }
    }
}