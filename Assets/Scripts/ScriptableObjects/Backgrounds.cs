using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Backgrounds List", menuName = "Backgrounds List", order = 51)]
    public sealed class Backgrounds : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites;

        public Sprite[] GetSprites()
        {
            return sprites;
        }

        public Sprite GetSprite()
        {
            var index = Random.Range(0, sprites.Length);
            return sprites[index];
        }
    }
}

