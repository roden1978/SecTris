using UnityEngine;

public class Blow : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float radius = 5;
    [SerializeField, Range(0, 100)] private float force = 35;
    
    private const int LayerMaskSector = 1 << 9;

    public void ExplodeSectors()
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
}
