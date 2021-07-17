using System.Collections;
using System.Linq;
using UnityEngine;

public class Engineer: MonoBehaviour
{
    [SerializeField] private Pool pool;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Blow blow;
    [SerializeField] private float time;

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

    public void DeactivateSectors()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors
            .Where(sector => sector.activeInHierarchy))
        {
            sector.SetActive(false);
        }
    }

    public void RestoreSectorsConstraints()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors)
        {
            var rb = sector.GetComponent<Rigidbody>();

            rb.constraints = RigidbodyConstraints.FreezeRotation |
                             RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionZ;
            rb.drag = 5;
        }
    }

    public void Detonate()
    {
        StartCoroutine(ActivateMainPanel(time));
        PrepareSectors();
        blow.ExplodeSectors();
    }
    
    private IEnumerator ActivateMainPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestoreSectorsConstraints();
        DeactivateSectors();
        Time.timeScale = 0;
        mainPanel.SetActive(true);
    }
    
}