using System.Collections.Generic;
using UnityEngine;

public class Neighbor
{
    private readonly List<GameObject> _list;

    private readonly IMaxFixedLevel _maxFixedLevel;

    private const int SectorsInTorAmount = 5;

    public Neighbor(List<GameObject> list)
    {
        _list = list;
        _maxFixedLevel = new MaxFixedLevel(_list);
    }

    public void Find()
    {
        var value = _maxFixedLevel.Value();
        var levelAmount = value + 1;
        var bucket = FillBucket(levelAmount);
        Search(bucket, value);
    }

   private Sector[,] FillBucket(int levelsAmount)
   {
       var bucket = new Sector[levelsAmount, SectorsInTorAmount];;
        for (var levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
        {
            foreach (var item in _list)
            {
                var sector = item.GetComponent<Sector>();
                var level = sector.GetLevel();

                if (levelIndex != level) continue;
                
                var angel = Mathf.RoundToInt(item.transform.eulerAngles.y);
                var index = new SectorsIndex(angel).Value();
                    
                bucket[levelIndex, index] = sector;
            }
        }
        return bucket;
    }

   private void Search(Sector[,] bucket, int levelsAmount)
   {
       var investigatedArray = new Sector[3,SectorsInTorAmount];
       for (int z = levelsAmount; z >= 0; z--)
       {
           if (z < 2) continue;
           for (int i = 0; i < 3; i++)
           {
               for (int j = 0; j < SectorsInTorAmount; j++)
               {
                   investigatedArray[i, j] = bucket[z - i, j];
               }
           }

            var rowSectors = VerticalSearch(investigatedArray);
            foreach (var sector in rowSectors)
            {
               sector.gameObject.SetActive(false);
            }
       }
   }
   private IEnumerable<Sector> VerticalSearch(Sector[,] sectors)
   {
       var findSectors = new List<Sector>();
       var array = new Sector[3];
       for (int i = 0; i < SectorsInTorAmount; i++)
       {
           for (int j = 0; j < 3; j++)
           {
               array[j] = sectors[j, i];
           }

           var current = array[0];
           if (!current) continue;
           findSectors.Add(current);
           
           var sector = SearchInRow(array, current, 0);
           if(!sector) continue; 
           findSectors.Add(sector);
           
           if(findSectors.Count < 3) findSectors.Clear();
           return findSectors;
       }
       return findSectors; 
   }

   private Sector SearchInRow(Sector[] sectors, Sector current, int i)
   {
       if(i + 1 > 2) return null;
       if (!current  || !sectors[i + 1]) return null;
       if (current.GetColorIndex() != sectors[i + 1].GetColorIndex()) return null;
       SearchInRow(sectors, sectors[i + 1], i + 1);
       return sectors[i + 1];

   }
}
