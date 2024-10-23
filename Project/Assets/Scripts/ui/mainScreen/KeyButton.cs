using System;
using UnityEngine;

namespace ui.mainScreen
{
    public class KeyButton : MonoBehaviour
    {
        private void Awake()
        {
            ChangeVisible();
            Singleton<UiControllers>.Instance.OnSwitchDevice += ChangeVisible;
        }

        private void ChangeVisible()
        {
            gameObject.SetActive(!Singleton<UiControllers>.Instance.IsMobile);
        }

        private void OnDestroy()
        {
            Singleton<UiControllers>.Instance.OnSwitchDevice -= ChangeVisible;
        }
    }
}
