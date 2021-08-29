using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BucketHeight: IBucketHeight
{
   private readonly List<GameObject> _list;
   private readonly float _floor;
   public BucketHeight(List<GameObject> list)
   {
      _list = list;
      _floor = .1f;
   }

   public float Value()
   {
      if (_list.Count == 0) return _floor;
      
      if (!_list[0].TryGetComponent(out MeshRenderer meshRenderer)) return 0;
      
      var delta = meshRenderer.bounds.size.y + _floor;
      var value = _list.Select(item => 
          item.transform.position.y).Prepend(float.MinValue).Max();
      
      return value + delta;
   }
}
