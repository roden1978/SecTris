using System.Collections.Generic;
using UnityEngine;

public class Neighbor
{
    private readonly List<GameObject> _list;

    private readonly IMaxFixedLevel _maxFixedLevel;
    private readonly List<Sector> _findSectors;

    private const int SectorsInTorAmount = 5;

    public Neighbor(List<GameObject> list)
    {
        _list = list;
        _maxFixedLevel = new MaxFixedLevel(_list);
        _findSectors = new List<Sector>();
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
       var bucket = new Sector[levelsAmount, SectorsInTorAmount];
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
            _findSectors.Clear();
       }
   }
   private IEnumerable<Sector> VerticalSearch(Sector[,] sectors)
   {
       var array = new Sector[3];
       for (int i = 0; i < SectorsInTorAmount; i++)
       {
           for (int j = 0; j < 3; j++)
           {
               array[j] = sectors[j, i];
           }

           var current = array[0];
           if (!current) continue;
           
           SearchNext(array, current, 0);
           
           if(_findSectors.Count < 3) _findSectors.Clear();
       }
       return _findSectors; 
   }
   private List<Sector> RemainingSectors(IReadOnlyList<Sector> sectors, Sector current, int i)

   {
       if(i + 1 > sectors.Count) return null;
       var sectorsNew = new List<Sector>();

       if (!sectors[i + 1]  || !current) return null;
       if (current.GetColorIndex() == sectors[i + 1].GetColorIndex())
           for (int j = i + 1; j < sectors.Count; j++)
           {
            sectorsNew.Add(sectors[j]);
           }
       else
        return null;
       
        AddEquals(current);

       return sectorsNew;

   }

   private void SearchNext(Sector[] array, Sector current, int i)
   {
       while (true)
       {
           var nextArray = RemainingSectors(array, current, i);
           if (nextArray == null) return;
           if (i + 1 == nextArray.Count)
               AddEquals(nextArray[0]);
           else
           {
               array = nextArray.ToArray();
               current = nextArray[0];
               i = 0;
               continue;
           }

           break;
       }
   }

   private void AddEquals(Sector sector)
   {
       _findSectors.Add(sector);
   }
}
