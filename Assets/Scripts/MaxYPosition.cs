using System.Collections.Generic;
using System.Linq;
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
      if (_list.Count == 0) return 0;
      var value = _list.Select(item => 
         item.transform.position.y).Prepend(float.MinValue).Max();
      return value;
   }
}
