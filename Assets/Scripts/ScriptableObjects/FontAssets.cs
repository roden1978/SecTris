using System;
using TMPro;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Font Assets List", menuName = "Font Assets List", order = 1)]
    public sealed class FontAssets : ScriptableObject
    {
        [SerializeField] private TMP_Asset[] _assets;

        public TMP_Asset[] GetAssets()
        {
            return _assets;
        }

        public TMP_Asset GetTMPAsset(int index)
        {
            if (index < _assets.Length)
            {
                return _assets[index];
            }

            throw new ArgumentOutOfRangeException(index.ToString());
        }
    }
}

