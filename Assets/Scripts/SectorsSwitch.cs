using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsSwitch : MonoBehaviour
{
   [SerializeField] private Neighbor _neighbor;
   private int _count;

   public event Action<Sector> OnDeactivateSector;
   private void OnEnable()
   {
      _neighbor.OnBurningSectors += SwitchSectors;
   }

   private void OnDisable()
   {
      _neighbor.OnBurningSectors -= SwitchSectors;
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
         sectors[_count].gameObject.SetActive(false);
         OnDeactivateSector?.Invoke(sectors[_count]);
         _count++;
         yield return new WaitForSeconds(0.05f);
      }
   }
}
