using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Backgrounds _backgrounds;
        [SerializeField] private Image _image;   
        public void ChangeBackground()
        {
            _image.sprite = _backgrounds.GetSprite();
        }
    }
}
