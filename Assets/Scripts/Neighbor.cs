using System;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor: MonoBehaviour
{
    private List<Sector> _findSectors;
    private List<Sector> _allFindSectors;

    public event Action<int> OnScoreChanged; 
    public event Action<Sector[]> OnBurningSectors;
    
    private const int Row = 3;
    private const int Column = 5;
    private const int MinimumSectors = 3;

  private void Awake()
    {
        _findSectors = new List<Sector>();
        _allFindSectors = new List<Sector>();
    }

    public void FindNeighborSectors(List<GameObject> sectors)
    {
        var value = new MaxFixedLevel(sectors).Value();
        var levelAmount = value + 1;
        var bucket = FillBucket(levelAmount, sectors);
        Search(bucket, value);
    }

   private Sector[,] FillBucket(int levelsAmount, List<GameObject> sectors)
   {
       var bucket = new Sector[levelsAmount, Column];
        for (var levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
        {
            foreach (var item in sectors)
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
       var investigatedArray = new Sector[Row,Column];
       for (var z = levelsAmount; z >= 0; z--)
       {
           if (z < Row - 1) continue;
           for (var i = 0; i < Row; i++)
           {
               for (var j = 0; j < Column; j++)
               {
                   investigatedArray[i, j] = bucket[z - i, j];
               }
           }

            var rowSectors = VerticalSearch(investigatedArray);
            if(rowSectors.Count != 0)
            {
                OnScoreChanged?.Invoke(rowSectors.Count);
                var copy = new Sector[rowSectors.Count];
                rowSectors.CopyTo(copy);
                OnBurningSectors?.Invoke(copy);
            }
            _allFindSectors.Clear();
       }
   }
   private List<Sector> VerticalSearch(Sector[,] sectors)
   {
       var array = new Sector[Row];
       for (var i = 0; i < Column; i++)
       {
           for (var j = 0; j < Row; j++)
           {
               array[j] = sectors[j, i];
           }

           var current = array[0];
           if (!current) continue;
           
           SearchNext(array, current, 0);
           
           if(_findSectors.Count < MinimumSectors)
           {
               _findSectors.Clear();
               continue;
           }
           foreach (var sector in _findSectors)
           {
               _allFindSectors.Add(sector);
           }
           _findSectors.Clear();
       }
       return _allFindSectors; 
   }
   private List<Sector> RemainingSectors(IReadOnlyList<Sector> sectors, Sector current, int i)

   {
       if(i + 1 > sectors.Count) return null;
       var sectorsNew = new List<Sector>();

       if (!sectors[i + 1]  || !current) return null;
       if (current.GetColorIndex() == sectors[i + 1].GetColorIndex())
           for (var j = i + 1; j < sectors.Count; j++)
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
