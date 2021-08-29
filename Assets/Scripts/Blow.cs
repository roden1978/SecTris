using UnityEngine;

public class Blow : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float _radius = 5;
    [SerializeField, Range(0, 100)] private float _force = 35;
    
    private const int LayerMaskSector = 1 << 9;

    public void ExplodeSectors()
    {
        var sectorsColliders = new Collider[100];
        
        if(Physics.OverlapSphereNonAlloc(
            transform.position, 
            _radius,
            sectorsColliders,
            LayerMaskSector) == 0)
            return;
        
        foreach (var sectorCollider in sectorsColliders)
        {
            if(sectorCollider && sectorCollider.TryGetComponent(out Rigidbody sectorRigidbody))
                sectorRigidbody.AddExplosionForce(
                    _force,
                    transform.position,
                    _radius,
                    0,
                    ForceMode.Impulse
                    );
        }    
    }
}
