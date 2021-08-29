using UnityEngine;

public sealed class SectorTuner : MonoBehaviour
{
    [SerializeField] private Pool _sectorsPool;

    private GameObject _tunedSector;

    public GameObject TuneSector(Vector3 position, Material material, int angel, int index)
    {
        _tunedSector = _sectorsPool.GetPooledObject();
        _tunedSector.transform.position = position;
        _tunedSector.transform.Rotate(0, angel, 0);

        if (_tunedSector.TryGetComponent(out MeshRenderer meshRenderer))
            meshRenderer.material = material;

        if (_tunedSector.TryGetComponent(out Sector sector))
            sector.SetColorIndex(index);

        return _tunedSector;
    }
}