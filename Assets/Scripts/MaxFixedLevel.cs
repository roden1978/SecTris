using System.Collections.Generic;
using UnityEngine;

public class MaxFixedLevel : IMaxFixedLevel
{
    private readonly List<GameObject> _list;
    
    public MaxFixedLevel(List<GameObject> list)
    {
        _list = list;
    }
    
    public int Value()
    {
        if (_list.Count == 0)
        {
            return 0;
        }
        var maxPosition = int.MinValue;
      
        foreach (var item in _list)
        {
            var meshRenderer = item.GetComponent<MeshRenderer>();
            var sectorHeight = meshRenderer.bounds.size.y;
            var level = Mathf.RoundToInt(item.transform.position.y / sectorHeight);
            
            if (level > maxPosition)
            {
                maxPosition = level;
            }
        }
        return maxPosition;
    }
    
}
