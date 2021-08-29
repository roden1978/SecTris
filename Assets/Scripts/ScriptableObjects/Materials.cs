using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Materials List", menuName = "Materials List", order = 1)]
    public sealed class Materials : ScriptableObject
    {
        [SerializeField] private Material[] _materials;

        public Material GetMaterial(int index)
        {
            if (index < _materials.Length)
            {
                return _materials[index];
            }

            throw new ArgumentOutOfRangeException(index.ToString());
        }
    }
}

