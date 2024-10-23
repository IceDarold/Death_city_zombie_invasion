using System;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

namespace ui
{
    public class UiControllers : Singleton<UiControllers>
    {
        [SerializeField] private Joystick _leftJoystick;
        [SerializeField] private Joystick _rightJoystick;
        [SerializeField] private Joystick _snipeJoystick;
        [SerializeField] private Button _switchDevice;
        [SerializeField] private Text _text;
        public Joystick LeftJoystick => _leftJoystick;
        public Joystick RightJoystick => _rightJoystick;
        public Joystick SnipeJoystick => _snipeJoystick;

        public bool IsMobile => false;
        
        public event Action OnSwitchDevice;
        

        public void EnableJoysticks()
        {
            var isNormalMode = GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.Normal;
            _leftJoystick.gameObject.SetActive(isNormalMode);
            _rightJoystick.gameObject.SetActive(isNormalMode);
            _snipeJoystick.gameObject.SetActive(!isNormalMode);
        }
	
        public void DisableJoysticks()
        {
            _leftJoystick.gameObject.SetActive(false);
            _rightJoystick.gameObject.SetActive(false);
            _snipeJoystick.gameObject.SetActive(false);
        }
        
        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        public void NormalMode()
        {
            if (!IsMobile) return;
            _leftJoystick.gameObject.SetActive(true);
            _rightJoystick.gameObject.SetActive(true);
        }

        public void SnipeMode()
        {
            if (!IsMobile) return;
            _leftJoystick.gameObject.SetActive(false);
            _rightJoystick.gameObject.SetActive(true);
        }

        public void Shot()
        {
            if (IsMobile) return;
            if (GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.SnipeMode)
            {
                var inGamePage = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) as InGamePage;
                inGamePage.OnSnipeFirePressed();
            }
        }

    }
}
