using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsSwitch : MonoBehaviour
{
   [SerializeField] private Neighbor _neighbor;
   [SerializeField] private AudioSource _collectSectorEffect;
   private int _count;

   private void OnEnable()
   {
      _neighbor.OnCollectSectors += SwitchSectors;
   }

   private void OnDisable()
   {
      _neighbor.OnCollectSectors -= SwitchSectors;
   }

   private void SwitchSectors(Sector[] sectors)
   {
      _count = 0;
      StartCoroutine(Burning(sectors));
   }

   private IEnumerator Burning(IReadOnlyList<Sector> sectors)
   {
      while (_count < sectors.Count)
      {
         _collectSectorEffect.Play();
         sectors[_count].gameObject.SetActive(false);
         _count++;
         yield return new WaitForSeconds(0.05f);
      }
   }
}
