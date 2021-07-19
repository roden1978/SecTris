using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Backgrounds backgrounds;
        [SerializeField] private Image image;   
        public void ChangeBackground()
        {
            image.sprite = backgrounds.GetSprite();
        }
    }
}
