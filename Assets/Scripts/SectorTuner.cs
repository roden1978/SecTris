using Scripts;
using UnityEngine;

public sealed class SectorTuner : MonoBehaviour
{
    [SerializeField] private Pool _sectorsPool;

    private MeshRenderer _meshRenderer;
    private GameObject _tunedSector;

    public GameObject TuneSector(Transform tr, Material material, int angel)
    {
        _tunedSector = _sectorsPool.GetPooledObject();
        _meshRenderer = _tunedSector.GetComponent<MeshRenderer>();
        _meshRenderer.material = material;
        _tunedSector.transform.position = tr.position;
        _tunedSector.transform.Rotate(0, angel, 0);
        return _tunedSector;
    }
   
}
