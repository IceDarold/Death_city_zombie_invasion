using System;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class SnipeUI : MonoBehaviour, IMessageHandler
{
	public bool IsScopeShow
	{
		get
		{
			return this.snipeScopeUI.IsScopeShow;
		}
	}

	private void Awake()
	{
		this.LType = Singleton<GlobalData>.Instance.GetCurrentLanguage();
		Singleton<GlobalMessage>.Instance.RegisterMessageHandler(this);
		this.snipeShoot.interactable = false;
		this.snipeScopeUI.SetShootBtnEnable(delegate(bool _enable)
		{
			this.snipeShoot.interactable = _enable;
		});
		this.gameScene = GameApp.GetInstance().GetGameScene();
		this.snipeScopeUI.snipeUI = this;
		this.introPageBtn.text = Singleton<GlobalData>.Instance.GetText("SNIPE_INTRO_PAGE_BTN");
		this.introPageTarget.text = Singleton<GlobalData>.Instance.GetText("SNIPE_INTRO_PAGE_TARGET");
		this.guideDesc.text = Singleton<GlobalData>.Instance.GetText("GUIDE_AIM");
		this.snipeModeGuide = (PlayerPrefs.GetInt("SNIPE_GUIDE_RECORDER", 0) != 0);
		Singleton<FontChanger>.Instance.SetFont(introPageBtn);
		Singleton<FontChanger>.Instance.SetFont(introPageTarget);
		Singleton<FontChanger>.Instance.SetFont(guideDesc);
		if (this.snipeModeGuide)
		{
			this.SnipeGuideObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		this.LType = Singleton<GlobalData>.Instance.GetCurrentLanguage();
		this.RefreshMissionDesc();
		this.RefreshSliderHandleDesc();
	}

	public void OnDestroy()
	{
		Singleton<GlobalMessage>.Instance.RemoveMessageHandler(this);
	}

	public void SetWeaponData(Weapon wp)
	{
		this.scope.text = wp.SnipeScope + "X";
		this.bulletNum.text = wp.BulletCount + "/" + wp.MaxCapacity;
		Singleton<FontChanger>.Instance.SetFont(scope);
		Singleton<FontChanger>.Instance.SetFont(bulletNum);
	}

	public void SetBulletCount(int curNum, int maxGunLoad)
	{
		this.bulletNum.text = curNum + "/" + maxGunLoad;
		Singleton<FontChanger>.Instance.SetFont(bulletNum);
	}

	public void SetMission(string descCH, string descEN, int curAmout, int maxAmount)
	{
		this.curMissionDesc_RU = descCH;
		this.curMissionDesc_EN = descEN;
		this.RefreshMission(curAmout, maxAmount);
	}

	public void RefreshMission(int curAmout, int maxAmount)
	{
		this.curMissionAmount = curAmout;
		this.maxMissionAmount = maxAmount;
		this.RefreshMissionDesc();
	}

	public void SetLevelDescription(string intro, string description)
	{
		this.levelIntro.text = intro;
		this.levelDescription.text = description;
		Singleton<FontChanger>.Instance.SetFont(levelIntro);
		Singleton<FontChanger>.Instance.SetFont(levelDescription);
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePause;
		this.introPage.SetActive(true);
	}

	public void OnLevelDescriptionPressed()
	{
		this.introPage.SetActive(false);
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePlaying;
		Time.timeScale = 1f;
		if (!this.snipeModeGuide)
		{
			this.SnipeGuideObject.SetActive(true);
		}
	}

	private void RefreshMissionDesc()
	{
		this.missionDescription.text = string.Format((this.LType != LanguageEnum.Russian) ? this.curMissionDesc_EN : this.curMissionDesc_RU, this.curMissionAmount, this.maxMissionAmount);
		Singleton<FontChanger>.Instance.SetFont(missionDescription);
	}

	public void OnCameraFOVChanged(Slider _slider)
	{
		if (!this.snipeModeGuide && this.SnipeGuideObject.activeSelf)
		{
			this.SnipeGuideObject.SetActive(false);
			PlayerPrefs.SetInt("SNIPE_GUIDE_RECORDER", 1);
			PlayerPrefs.Save();
		}
		this.OnCameraFOVChanged(_slider.value);
	}

	public void OnCameraFOVChanged(float _percent)
	{
		if (_percent == 0f)
		{
			this.targetFOV = 60f;
			this.snipeScopeUI.StartBlurAndHideScope();
		}
		else
		{
			this.targetFOV = this.gameScene.GetPlayerWeaponFOV(_percent);
			if (!this.snipeScopeUI.IsScopeShow)
			{
				this.snipeScopeUI.StartBlurAndShowScope();
			}
		}
	}

	private void RefreshSliderHandleDesc()
	{
		this.scopeCtrlHandleDesc.text = Singleton<GlobalData>.Instance.GetText("SCOPE_SLIDER_DESC");
		Singleton<FontChanger>.Instance.SetFont(scopeCtrlHandleDesc);
	}

	public void HandleMessage(MessageType type)
	{
		if (type != MessageType.LanguageChanged)
		{
			if (type != MessageType.SniperHeadShot)
			{
				if (type == MessageType.SniperKillMissionTarget)
				{
					this.uiEffectAnimator.Play("SpecialKill", 0, 0f);
					Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.SniperKill);
				}
			}
			else
			{
				this.uiEffectAnimator.Play("HeadShoot", 0, 0f);
				Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.SniperKill);
			}
		}
		else
		{
			this.OnEnable();
		}
	}

	public void DoShakeScope()
	{
		this.snipeScopeUI.DoShakeScope();
	}

	public bool CanShoot()
	{
		return this.snipeShoot.interactable;
	}

	public void Update()
	{
		fovSlider.value += Input.GetAxis("Mouse ScrollWheel");
		float num = this.gameScene.GetCameraFOV();
		if (num == this.targetFOV)
		{
			return;
		}
		float num2 = Mathf.Sign(this.targetFOV - num);
		num += num2 * 80f * Time.deltaTime;
		if ((num2 > 0f && num > this.targetFOV) || (num2 < 0f && num < this.targetFOV))
		{
			num = this.targetFOV;
		}
		this.gameScene.SetCameraFOV(num);
	}

	public void HideSnipeUI()
	{
		base.gameObject.SetActive(false);
	}

	public void SetMissionTimeLimited(bool timeLimited)
	{
		this.missionTime.gameObject.SetActive(timeLimited);
	}

	public void SetMissionTime(int delay)
	{
		int num = delay / 60;
		int num2 = delay - num * 60;
		this.missionTime.text = $"{num:D2}：{num2:D2}";
	}

	public Text scope;

	public Text scopeCtrlHandleDesc;

	public Text bulletNum;

	public Text missionDescription;

	public Text levelIntro;

	public Text levelDescription;

	public Text missionTime;

	public Text introPageBtn;

	public Text introPageTarget;

	public Text guideDesc;

	public Button snipeShoot;

	public SnipeScopeUI snipeScopeUI;

	public GameObject introPage;

	public GameObject SnipeGuideObject;

	public Animator uiEffectAnimator;

	public Slider fovSlider;

	protected string curMissionDesc_RU = string.Empty;

	protected string curMissionDesc_EN = string.Empty;

	protected int curMissionAmount;

	protected int maxMissionAmount;

	protected LanguageEnum LType;

	private float targetFOV = 60f;

	private const string SCOPE_SLIDER_DESC = "SCOPE_SLIDER_DESC";

	private const string SNIPE_INTRO_PAGE_BTN = "SNIPE_INTRO_PAGE_BTN";

	private const string SNIPE_INTRO_PAGE_TARGET = "SNIPE_INTRO_PAGE_TARGET";

	private const string SNIPE_GUIDE_DEDSC = "GUIDE_AIM";

	private GameScene gameScene;

	protected bool snipeModeGuide;
}
