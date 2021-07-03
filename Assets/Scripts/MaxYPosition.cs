using System.Collections.Generic;
using UnityEngine;

public class MaxYPosition: IMaxYPosition
{
   private readonly List<GameObject> _list;
   public MaxYPosition(List<GameObject> list)
   {
      _list = list;
   }

   public float Value()
   {
      if (_list.Count == 0)
      {
         return 0;
      }
      var maxPosition = float.MinValue;
      
      foreach (var item in _list)
      {
         if (item.transform.position.y > maxPosition)
         {
            maxPosition = item.transform.position.y;
         }
      }
      return maxPosition;
   }
}
