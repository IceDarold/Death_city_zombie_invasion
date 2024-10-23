using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ui.debug
{
    public class DebugButton : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _buttonText;
        [SerializeField] private Button _button;

        public void Construct(string name, string buttonText, UnityAction action)
        {
            _name.text = name;
            _buttonText.text = buttonText;
            _button.onClick.AddListener(action);
        }
    }
}
