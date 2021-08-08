using System.Collections;
using System.Linq;
using UnityEngine;

public class Engineer: MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Pool _pool;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Blow _blow;
    [SerializeField] private AudioSource _explosionEffect;
    [SerializeField] private float _time;

    private void OnEnable()
    {
        _game.OnGameOver += Detonate;
    }

    private void OnDisable()
    {
        _game.OnGameOver -= Detonate;
    }

    private void PrepareSectors()
    {
        var sectors = _pool.GetAllActive();
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
        var sectors = _pool.GetAllActive();
        foreach (var sector in sectors
            .Where(sector => sector.activeInHierarchy))
        {
            sector.SetActive(false);
        }
    }

    private void RestoreSectorsConstraints()
    {
        var sectors = _pool.GetAllActive();
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
        StartCoroutine(ActivateMainPanel(_time));
        PrepareSectors();
        _explosionEffect.Play();
        _blow.ExplodeSectors();
    }
    
    private IEnumerator ActivateMainPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestoreSectorsConstraints();
        DeactivateSectors();
        Time.timeScale = 0;
        _mainPanel.SetActive(true);
    }
    
}