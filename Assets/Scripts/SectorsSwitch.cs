using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsSwitch : MonoBehaviour
{
   [SerializeField] private Neighbor _neighbor;
   [SerializeField] private AudioSource _collectSectorEffect;
   [SerializeField] [Range(0.05f, 0.1f)] private float _delay = 0.05f;
   private int _count;

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
      _count = 0;
      StartCoroutine(Collect(sectors));
   }

   private IEnumerator Collect(IReadOnlyList<Sector> sectors)
   {
      while (_count < sectors.Count)
      {
         OnCollect?.Invoke(sectors[_count]);
         _collectSectorEffect.Play();
         sectors[_count].gameObject.SetActive(false);
         _count++;
         yield return new WaitForSeconds(_delay);
      }
   }
}
