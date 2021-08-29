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
        var maxPosition = int.MinValue;
      
        foreach (var item in _list)
        {
            if(item.TryGetComponent(out MeshRenderer meshRenderer))
            {
                var sectorHeight = meshRenderer.bounds.size.y;
                var level = Mathf.RoundToInt(item.transform.position.y / sectorHeight);

                if (level > maxPosition)
                {
                    maxPosition = level;
                }
            }
        }
        return maxPosition;
    }
    
}
