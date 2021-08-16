using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Neighbor: MonoBehaviour
{
    private List<Sector> _foundVerticalSectors;
    private List<Sector> _foundHorizontalSectors;
    private List<Sector> _allFoundSectors;

    public event Action<int> OnScoreChanged; 
    public event Action<Sector[]> OnCollectSectors;
    
    private const int Row = 3;
    private const int Column = 5;
    private const int MinimumSectors = 3;

  private void Awake()
    {
        _foundVerticalSectors = new List<Sector>();
        _foundHorizontalSectors = new List<Sector>();
        _allFoundSectors = new List<Sector>();
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
       var verticalArray = new Sector[Row, Column];
       var horizontalArray = new Sector[Column];
       
       for (var z = levelsAmount; z >= 0 ; z--)
       {
           if (z > 1)
           {
               for (var i = 0; i < Row; i++)
               {
                   for (var j = 0; j < Column; j++)
                   {
                       verticalArray[i, j] = bucket[z - i, j];
                   }
               }

               VerticalSearch(verticalArray);
           }
           
           var oneColorList = new List<Sector>();
           var indexElementWithEqualsColor = 0;

           for (var j = 0; j < Column; j++)
           {
               horizontalArray[j] = bucket[z, j];
           }

           for (var i = 0; i < Column; i++)
           {
               oneColorList = AssemblyOneColorArray(horizontalArray, horizontalArray[i], i);

               if (oneColorList.Count < MinimumSectors - 1) continue;
               indexElementWithEqualsColor = i;
               break;
           }
           if (oneColorList.Count >= MinimumSectors - 1)
                HorizontalSearch(oneColorList.ToArray(), horizontalArray[indexElementWithEqualsColor]);
           
           var noDupesCollectedSectors = _allFoundSectors.Distinct().ToList();
           OnScoreChanged?.Invoke(noDupesCollectedSectors.Count);
           var copy = new Sector[noDupesCollectedSectors.Count];
           noDupesCollectedSectors.CopyTo(copy);
           OnCollectSectors?.Invoke(copy);
           _allFoundSectors.Clear();
       }
   }

   private List<Sector> HorizontalSearch(Sector[] sectors, Sector current)
   {
      // for (var i = 0; i < sectors.Length; i++)
      // {
      //     var current = sectors[0];
      //     if (!current) continue;
           
           var arrayForNegative = PositiveHorizontalSearchNext(sectors, current);
           if (arrayForNegative != null)
                NegativeHorizontalSearchNext(arrayForNegative.ToArray(), current);
           
           if(_foundHorizontalSectors.Count >= MinimumSectors - 1)
               _foundHorizontalSectors.Add(current);
           else
               _foundHorizontalSectors.Clear();
         
           foreach (var sector in _foundHorizontalSectors)
           {
               _allFoundSectors.Add(sector);
           }
           _foundHorizontalSectors.Clear();
       //}
       return _allFoundSectors;
   }

   private List<Sector> AssemblyOneColorArray(IReadOnlyList<Sector> sectors, Sector current, int i)
   {
       var oneColorArray = new List<Sector>();
       for (var j = 0; j < Column; j++)
       {
           if (!sectors[j] || !current || j == i) continue;
           
           //if (!sectors[j] ) continue;
           
           if(current.GetColorIndex() == sectors[j].GetColorIndex())
               oneColorArray.Add(sectors[j]);
       }

       return oneColorArray;
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
           
           SearchNextVertical(array, current, 0);
           
           if(_foundVerticalSectors.Count < MinimumSectors)
           {
               _foundVerticalSectors.Clear();
               continue;
           }
           
           foreach (var sector in _foundVerticalSectors)
           {
               _allFoundSectors.Add(sector);
           }
           _foundVerticalSectors.Clear();
       }
       return _allFoundSectors; 
   }
   private List<Sector> RemainingVerticalSectors(IReadOnlyList<Sector> sectors, Sector current, int i)

   {
       if(i + 1 > sectors.Count) return null;
       var verticalSectorsNew = new List<Sector>();

       if (!sectors[i + 1]  || !current) return null;
       if (current.GetColorIndex() == sectors[i + 1].GetColorIndex())
           for (var j = i + 1; j < sectors.Count; j++)
           {
               verticalSectorsNew.Add(sectors[j]);
           }
       else
        return null;
       
        AddVerticalEquals(current);

       return verticalSectorsNew;

   }

   private List<Sector> PositiveHorizontalSearchNext(Sector[] array, Component current)
   {
       var nextArray = new List<Sector>();
       int currentAngel;
       if (array.Length == 0) return nextArray;

       if (current.transform.eulerAngles.y < 1)
           currentAngel = 0;
       else
           currentAngel = Mathf.RoundToInt(current.transform.eulerAngles.y);
       
       for (var i = currentAngel + 72; i < 360; i += 72)
       {
           nextArray = RemainingHorizontalSectors(array, array[0], i);
           if (nextArray.Count == 0) break;
           array = nextArray.ToArray();
       }

       return array.ToList();
   }
   
   private void NegativeHorizontalSearchNext(Sector[] array, Component current)
   {
       
       if (array.Length <= 0) return;

       var reversArray = array.Reverse().ToArray();
       
       for (var i = 360 - 72; i > 0; i -= 72)
       {
           if (reversArray.Length == 0) break;
           var nextArray = RemainingHorizontalSectors(reversArray, reversArray[0], i);
           reversArray = nextArray.ToArray();
       }
   }

   private List<Sector> RemainingHorizontalSectors(Sector[] sectors, Sector current, int angel)
   {
       var sectorsNew = new List<Sector>();
       if (sectors.Length == 0) return sectorsNew;
       
           var currentAngel = Mathf.RoundToInt(current.transform.eulerAngles.y);
           if(currentAngel == angel)
           {
               for (var i = 1; i < sectors.Length; i++)
               {
                   sectorsNew.Add(sectors[i]);
               }

               _foundHorizontalSectors.Add(current);
           }
           else
           {
               return new List<Sector>();
           }
       return sectorsNew;
   }
   private void SearchNextVertical(Sector[] array, Sector current, int i)
   {
       while (true)
       {
           var nextArray = RemainingVerticalSectors(array, current, i);
           if (nextArray == null) return;
           if (i + 1 == nextArray.Count)
               AddVerticalEquals(nextArray[0]);
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

   private void AddVerticalEquals(Sector sector)
   {
       _foundVerticalSectors.Add(sector);
   }
}
