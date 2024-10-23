using UnityEngine;
using UnityEngine.UI;

namespace ui.imageTranslator
{
    public class FontChanger : Singleton<FontChanger>
    {
        [SerializeField] private Font _ruFontCapturIt;
        [SerializeField] private Font _enFontOnkelz;

        public void SetFont(Text text)
        {
            if (text == null) return;
            if (Singleton<GlobalData>.Instance.GetCurrentLanguage() == LanguageEnum.English)
            {
                if (text.font.name.Equals(_ruFontCapturIt.name))
                {
                    text.font = _enFontOnkelz;
                }
            }
            else
            {
                if (text.font.name.Equals(_enFontOnkelz.name))
                {
                    text.font = _ruFontCapturIt;
                }
            }

        }
    }
}
