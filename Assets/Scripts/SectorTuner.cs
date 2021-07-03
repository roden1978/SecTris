using UnityEngine;

public sealed class SectorTuner : MonoBehaviour
{
    [SerializeField] private Pool sectorsPool;

    private GameObject _tunedSector;

    public GameObject TuneSector(Vector3 position, Material material, int angel, int index)
    {
        _tunedSector = sectorsPool.GetPooledObject();
        _tunedSector.transform.position = position;
        _tunedSector.transform.Rotate(0, angel, 0);
        
        var meshRenderer = _tunedSector.GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        
        var script = _tunedSector.GetComponent<Sector>();
        script.SetColorIndex(index);

        return _tunedSector;
    }

   
}
