using UnityEngine;
using UnityEngine.UI;

namespace ui.imageTranslator
{
    [RequireComponent(typeof(Image))]
    public class ImageTranslator : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        private void Start()
        {
            SetSprite();
        }

        private void SetSprite()
        {
            var image = GetComponent<Image>();
            image.sprite = _sprites[(int)Singleton<GlobalData>.Instance.GetCurrentLanguage()];
        }
    }
}
