using System.Collections.Generic;
using UnityEngine;

public class SectorComparer : IEqualityComparer<GameObject>
{
    public bool Equals(GameObject x, GameObject y)
    {
        if (ReferenceEquals(x, y)) return true;
        
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;
        
        return (int)x.transform.rotation.y == (int)y.transform.rotation.y;
    }

    public int GetHashCode(GameObject obj)
    {
        var hashCode = obj.GetHashCode();
        return hashCode;
    }
}
