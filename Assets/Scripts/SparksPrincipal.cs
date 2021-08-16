using ScriptableObjects;
using UnityEngine;

public class SparksPrincipal : MonoBehaviour
{
    [SerializeField] private Pool _sparksPool;
    [SerializeField] private SectorsSwitch _sectorsSwitch;
    [SerializeField] private Materials _materials;
    private void OnEnable()
    {
        _sectorsSwitch.OnCollect += StartSpark;
    }

    private void OnDisable()
    {
        _sectorsSwitch.OnCollect -= StartSpark;
    }

    private void StartSpark(Sector sector)
    {
        var spark = _sparksPool.GetPooledObject();
        var sparkParticleSystem = spark.GetComponent<ParticleSystem>();
        var sparkRenderer = sparkParticleSystem.GetComponent<Renderer>();
        var index = sector.GetColorIndex();
        sparkRenderer.material = _materials.GetMaterial(index);

        var center = sector.GetComponent<MeshRenderer>().bounds.center;
        spark.transform.position = center;
        Debug.Log(center);
        spark.SetActive(true);
    }
    
    
}
