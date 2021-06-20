using Scripts;
using UnityEngine;

public sealed class SectorTuner : MonoBehaviour
{
    [SerializeField] private Pool _sectorsPool;

    private GameObject _tunedSector;

    public GameObject TuneSector(Transform tr, Material material, int angel, int index)
    {
        _tunedSector = _sectorsPool.GetPooledObject();
        var meshRenderer = _tunedSector.GetComponent<MeshRenderer>();
        var script = _tunedSector.GetComponent<Sector>();
        meshRenderer.material = material;
        script.SetColorIndex(index);
        _tunedSector.transform.position = tr.position;
        _tunedSector.transform.Rotate(0, angel, 0);
        return _tunedSector;
    }

   
}
