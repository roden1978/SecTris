using System;
using System.Collections;
using System.Linq;
using InputSwipe.Pause;
using UnityEngine;

public class Engineer: MonoBehaviour
{
    [SerializeField] private Menu menu;
    [SerializeField] private Pool pool;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Blow blow;
    [SerializeField] private float time;

    private void OnEnable()
    {
        menu.OnBack += Detonate;
    }

    private void OnDisable()
    {
        menu.OnBack -= Detonate;
    }

    private void PrepareSectors()
    {
        var sectors = pool.GetAllActive();
        foreach (var sector in sectors)
        {
            var sectorRigidbody = sector.GetComponent<Rigidbody>();
            sectorRigidbody.isKinematic = false;
            sectorRigidbody.constraints = RigidbodyConstraints.None;
            sectorRigidbody.drag = 0;
        }
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
            var sectorRigidbody = sector.GetComponent<Rigidbody>();
            
            sectorRigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                          RigidbodyConstraints.FreezePositionX |
                                          RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void Detonate()
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