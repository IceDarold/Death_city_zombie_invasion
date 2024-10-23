using DataCenter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ui.debug
{
    public class DebugPanel : Singleton<DebugPanel>
    {
        private const string DebugButtonPath = "ui/Debug/DebugButton";
        
        [SerializeField] private Button _visibleButton;
        private DebugButton _buttonPrefab;

        private void Start()
        {

        }

        public void Init()
        {
            _visibleButton.onClick.AddListener(ChangeVisible);
            _buttonPrefab = Resources.Load<DebugButton>(DebugButtonPath);
            AddButtons();
        }

        public void AddButton(string name, string buttonText, UnityAction action)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.name = name;
            button.Construct(name, buttonText, action);
        }

        private void ChangeVisible()
        {
            transform.parent.parent.gameObject.SetActive(!transform.parent.parent.gameObject.activeSelf);
        }

        private void AddButtons()
        {
            //AddButton("Device", "SWITCH", Singleton<UiControllers>.Instance.SwitchDevice);
            AddButton("Energy", "Refresh", Singleton<GlobalData>.Instance.EnergyBackToFull);
            AddButton("Gold", "+1000", () =>
            {
                ItemDataManager.SetCurrency(CommonDataType.GOLD, 1000);
                Singleton<UiManager>.Instance.TopBar.Refresh();
            });
            AddButton("Diamonds", "+1000", () =>
            {
                ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 1000);
                Singleton<UiManager>.Instance.TopBar.Refresh();
            });
            AddButton("DNA", "+1000", () =>
            {
                ItemDataManager.SetCurrency(CommonDataType.DNA, 1000);
                Singleton<UiManager>.Instance.TopBar.Refresh();
            });
            AddButton("Reset Save", "Reset", () =>
            {
                PlayerPrefs.DeleteAll();
                Application.Quit();
            });
            
            // foreach (var kvp in _productsTemplate.Actions)
            // {
            //     AddButton(kvp.Key, "buy", () => kvp.Value?.Invoke());
            // }
        }
    }
}
