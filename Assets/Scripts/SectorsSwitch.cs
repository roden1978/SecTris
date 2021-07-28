using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsSwitch : MonoBehaviour
{
   [SerializeField] private Neighbor _neighbor;
   private int _count;
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
         yield return new WaitForSeconds(.05f);
         sectors[_count].gameObject.SetActive(false);
         _count++;
      }
   }
}
