using System.Collections;
using System.Linq;
using UnityEngine;

public class Blow : MonoBehaviour
{
    [SerializeField] private Pool pool;
    [SerializeField] private GameObject mainPanel;
    
    [SerializeField, Range(0, 10)] private float radius = 5;
    [SerializeField, Range(0, 1000)] private float force = 200;
    [SerializeField, Range(0, 10)] private float delay = 2;
    
    private const int LayerMaskSector = 1 << 9;

    public void Action()
    {
        StartCoroutine(Restart(delay));
        PrepareSectors();
        ExplodeSectors();
    }

    private void ExplodeSectors()
    {
        var results = new Collider[100];
        var size = Physics.OverlapSphereNonAlloc(transform.position, radius, results, LayerMaskSector);
        if(size == 0) return;
        foreach (var item in results)
        {
            if(item && item.TryGetComponent(out Rigidbody rb))
                rb.AddExplosionForce(force, transform.position, radius, 0, ForceMode.Impulse);
        }    
    }

    private void PrepareSectors()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors)
        {
            var rb = sector.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private IEnumerator Restart(float time)
    {
        yield return new WaitForSeconds(time);
        RestoreSectorsConstraints();
        DeactivateSectors();
        Time.timeScale = 0;
        mainPanel.SetActive(true);
    }
    
    private void DeactivateSectors()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors
            .Where(sector => sector.activeInHierarchy))
        {
            sector.SetActive(false);
        }
    }

    private void RestoreSectorsConstraints()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors)
        {
            var rb = sector.GetComponent<Rigidbody>();
            
            rb.constraints = RigidbodyConstraints.FreezeRotation | 
                             RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionZ;
        }
    }
}
