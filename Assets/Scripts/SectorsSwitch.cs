using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsSwitch : MonoBehaviour
{
   [SerializeField] private Neighbor _neighbor;
   [SerializeField] private AudioSource _collectSectorEffect;
   [SerializeField] [Range(0.01f, 0.1f)] private float _delay = 0.05f;

   public event Action<Sector> OnCollect; 

   private void OnEnable()
   {
      _neighbor.OnCollectSectors += CollectSectors;
   }

   private void OnDisable()
   {
      _neighbor.OnCollectSectors -= CollectSectors;
   }

   private void CollectSectors(Sector[] sectors)
   {
      StartCoroutine(Collect(sectors));
   }

   private IEnumerator Collect(IReadOnlyList<Sector> sectors)
   {
      var count = 0;
      while (count < sectors.Count)
      {
         OnCollect?.Invoke(sectors[count]);
         _collectSectorEffect.Play();
         sectors[count].gameObject.SetActive(false);
         count++;
         yield return new WaitForSeconds(_delay);
      }
   }
}
