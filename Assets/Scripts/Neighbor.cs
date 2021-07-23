using System.Collections.Generic;
using UnityEngine;

public class Neighbor
{
    private readonly List<GameObject> _list;

    private readonly IMaxFixedLevel _maxFixedLevel;

    private readonly SectorComparer _sectorComparer;
    private List<List<Sector>> _levelsList;
    private const int StartLevel = 1;
    private const int SectorsInTorAmount = 5;
    private Sector[,] _bucket; 

    public Neighbor(List<GameObject> list)
    {
        _list = list;
        _maxFixedLevel = new MaxFixedLevel(_list);
        _sectorComparer = new SectorComparer();
    }

    public void Find()
    {
        var value = _maxFixedLevel.Value();
        var levelAmount = value + 1;
        _bucket = new Sector[levelAmount, SectorsInTorAmount];
        _bucket = FillBucket(levelAmount);
        Search(_bucket, value);
    }

   private Sector[,] FillBucket(int levelsAmount)
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
                
                    var sector = item.GetComponent<Sector>();
                    _bucket[levelIndex, index] = sector; 
                }
            }
        }
        return _bucket;
    }

   private void Search(Sector[,] bucket, int levelsAmount)
   {
       var investigatedArray = new Sector[3,SectorsInTorAmount];
       for (int z = levelsAmount; z >= 0; z--)
       {

           if (z < 2)
           {
               for (int j = 0; j < SectorsInTorAmount; j++)
               {
                   investigatedArray[0, j] = bucket[z, j];
               }
                
               var columnSectors = HorizontalSearch(investigatedArray);
               foreach (var sector in columnSectors)
               {
                   sector.gameObject.SetActive(false);
               }
           }
           else
           {
               for (int i = 0; i < 3; i++)
               {
                   for (int j = 0; j < SectorsInTorAmount; j++)
                   {
                       investigatedArray[i, j] = bucket[z - i, j];
                   }
               }

               var columnSectors = HorizontalSearch(investigatedArray);
               foreach (var sector in columnSectors)
               {
                   sector.gameObject.SetActive(false);
               }
           }

           /*var rowSectors = VerticalSearch(investigatedArray);
           foreach (var sector in rowSectors)
           {
               sector.gameObject.SetActive(false);
           }*/
       }
   }
   private List<Sector> HorizontalSearch(Sector[,] sectors)
   {
       var findSectors = new List<Sector>();
       for (int i = 0; i < SectorsInTorAmount; i++)
       {
           Sector current;
           if (sectors[0, i] != null) current = sectors[0, i];
           else continue;

           for (int j = 0; j < SectorsInTorAmount; j++)
           {
               var sector = SearchEqual(current, sectors[0, j]);
               
               if (sector == null) break;
               
               findSectors.Add(sector);
           }

           if (findSectors.Count < 3) findSectors.Clear();
           else break;
       }
      
       var last = SearchEqualLast(sectors[0, 0], sectors[0, SectorsInTorAmount - 1]);
       if(last != null && findSectors.Count < SectorsInTorAmount) findSectors.Add(last);

       return findSectors;
   }

   private Sector SearchEqual(Sector current, Sector second)
   {
       if (second == null) return null;
       return current.GetColorIndex() == second.GetColorIndex() ? second : null;
   }
   private Sector SearchEqualLast(Sector first, Sector last)
   {
       if (first == null || last == null) return null;
       return first.GetColorIndex() == last.GetColorIndex() ? last : null;
   }

   private List<Sector> VerticalSearch(Sector[,] sectors)
   {
       var findSectors = new List<Sector>();
       for (int i = 0; i < SectorsInTorAmount; i++)
       {
           if (sectors[0, i] == null) break;
           
           var current = sectors[0, i];
           
           for (int j = 1; j < 3; j++)
           {
               var result = SearchInRow(sectors, current, i, j);
               if(result != null) findSectors.Add(result);
           }
       }
        
       return findSectors;
   }

   private Sector SearchInRow(Sector[,] sectors, Sector current, int i, int j)
   {
       if (!sectors[j, i]) return null;
       return current.GetColorIndex() == sectors[j,i].GetColorIndex() ? sectors[i,j] : null;
   }
}
