using UnityEngine;
using UnityEngine.UI;

namespace ui.imageTranslator
{
    [RequireComponent(typeof(Text))]
    public class TextTranslator : MonoBehaviour
    {
        [SerializeField] private string[] _texts;
        private void Start()
        {
            SetSprite();
        }

        private void SetSprite()
        {
            var text = GetComponent<Text>();
            text.text = _texts[(int)Singleton<GlobalData>.Instance.GetCurrentLanguage()];
        }
    }
}
