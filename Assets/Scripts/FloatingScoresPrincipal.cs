using ScriptableObjects;
using TMPro;
using UnityEngine;

public class FloatingScoresPrincipal : MonoBehaviour
{
    [SerializeField] private Pool _scoresPool;
    [SerializeField] private SectorsSwitch _sectorsSwitch;
    [SerializeField] private FontAssets _materials;
    [SerializeField] private Scores _scores;
    private void OnEnable()
    {
        _sectorsSwitch.OnCollect += StartScores;
    }

    private void OnDisable()
    {
        _sectorsSwitch.OnCollect -= StartScores;
    }

    private void StartScores(Sector sector)
    {
        var score = _scoresPool.GetPooledObject();
        if(score.TryGetComponent(out TMP_Text scoreTextMeshPro))
        {
            var index = sector.GetColorIndex();
            scoreTextMeshPro.font = _materials.GetTMPAsset(index);
            scoreTextMeshPro.text = "+" + _scores.GetSectorPrice();
        }


        if (sector.TryGetComponent(out MeshRenderer meshRenderer))
        {
            score.transform.position = meshRenderer.bounds.center;
            score.SetActive(true);
        }
    }
    
    
}
