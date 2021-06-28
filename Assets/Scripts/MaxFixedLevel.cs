using System.Collections.Generic;
using UnityEngine;

public class MaxFixedLevel : IMaxFixedLevel
{
    private readonly List<GameObject> _list;
    
    public MaxFixedLevel(List<GameObject> list)
    {
        _list = list;
    }
    
    public int value()
    {
        if (_list.Count == 0)
        {
            return 0;
        }
        var maxPosition = int.MinValue;
      
        foreach (var item in _list)
        {
            var sector = item.transform.GetComponent<Sector>();
            if (sector.GetLevel() > maxPosition)
            {
                maxPosition = sector.GetLevel();
            }
        }
        return maxPosition;
    }
    
}
