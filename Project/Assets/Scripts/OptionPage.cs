using System;
using System.Collections;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class OptionPage : GamePage
{
	private new void Awake()
	{
		this.init();
	}

	private new void OnEnable()
	{
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.4f);
		this.RefreshPage();
	}

	private void init()
	{
		this.MusicToggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.OnMusicClick(isOn);
		});
		this.SoundToggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.OnSoundClick(isOn);
		});
		this.LanguageRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(330f, (float)(this.GameLanguages.Length * 45));
		for (int i = 0; i < this.GameLanguages.Length; i++)
		{
			int id = i;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/LanguageChild")) as GameObject;
			gameObject.transform.SetParent(this.LanguageLayer);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.Find("Label").GetComponent<Text>().text = this.GameLanguages[id];
			Toggle component = gameObject.GetComponent<Toggle>();
			component.group = this.LanguageLayer.GetComponent<ToggleGroup>();
			component.onValueChanged.AddListener(delegate(bool isOn)
			{
				this.SetGameLanguage(isOn, id);
			});
			if (i == (int)Singleton<GlobalData>.Instance.GetCurrentLanguage())
			{
				component.isOn = true;
			}
			else
			{
				component.isOn = false;
			}
			gameObject.SetActive(true);
		}
	}

	public override void Close()
	{
		Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.SensitivityChanged);
		base.Close();
	}

	public void OnLanguageClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (this.LanguageRoot.gameObject.activeSelf)
		{
			this.LanguageRoot.gameObject.SetActive(false);
		}
		else
		{
			this.LanguageRoot.gameObject.SetActive(true);
		}
	}

	public void RefreshPage()
	{
		this.TitleText.text = Singleton<GlobalData>.Instance.GetText("OPTION");
		this.MusicButtonText.text = Singleton<GlobalData>.Instance.GetText("MUSIC");
		this.SoundButtonText.text = Singleton<GlobalData>.Instance.GetText("SOUND");
		this.SensitivityText.text = Singleton<GlobalData>.Instance.GetText("SENSITIVITY");
		this.LanguageTitleText.text = Singleton<GlobalData>.Instance.GetText("LANGUAGE");
		this.musicTxtOpen.text = Singleton<GlobalData>.Instance.GetText("ON");
		this.SoundTxtOpen.text = Singleton<GlobalData>.Instance.GetText("ON");
		this.musicTxtClose.text = Singleton<GlobalData>.Instance.GetText("OFF");
		this.SoundTxtClose.text = Singleton<GlobalData>.Instance.GetText("OFF");
		this.LanguageButtonText.text = this.GameLanguages[(int)Singleton<GlobalData>.Instance.GetCurrentLanguage()];
		this.ConfirmButtonText.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(TitleText);
		Singleton<FontChanger>.Instance.SetFont(MusicButtonText);
		Singleton<FontChanger>.Instance.SetFont(SoundButtonText);
		Singleton<FontChanger>.Instance.SetFont(SensitivityText);
		Singleton<FontChanger>.Instance.SetFont(LanguageTitleText);
		Singleton<FontChanger>.Instance.SetFont(musicTxtOpen);
		Singleton<FontChanger>.Instance.SetFont(SoundTxtOpen);
		Singleton<FontChanger>.Instance.SetFont(musicTxtClose);
		Singleton<FontChanger>.Instance.SetFont(SoundTxtClose);
		Singleton<FontChanger>.Instance.SetFont(LanguageButtonText);
		Singleton<FontChanger>.Instance.SetFont(ConfirmButtonText);
		this.RefalshPageData();
		this.ChangeSensitivity();
		this.RefreshShootingMode();
		this.LanguageRoot.gameObject.SetActive(false);
	}

	private void SetGameLanguage(bool isOn, int id)
	{
		if (isOn)
		{
			base.StartCoroutine(this.SelectLanguage((LanguageEnum)id));
		}
	}

	private IEnumerator SelectLanguage(LanguageEnum lan)
	{
		Singleton<UiManager>.Instance.ShowPage(PageName.WaitingPopup, null);
		Singleton<GlobalData>.Instance.SetGameLanguage(lan);
		yield return new WaitForSeconds(1f);
		Singleton<UiManager>.Instance.GetPage(PageName.WaitingPopup).Hide();
		this.LanguageRoot.gameObject.SetActive(false);
		this.RefreshPage();
		Singleton<UiManager>.Instance.PageStack.Peek().Refresh();
		yield break;
	}

	public void OnMusicClick(bool isOn)
	{
		Singleton<GameAudioManager>.Instance.GameMusic = this.MusicToggle.isOn;
		if (this.MusicToggle.isOn)
		{
			Singleton<GameAudioManager>.Instance.MusicSource.Play();
		}
		else
		{
			Singleton<GameAudioManager>.Instance.StopMusic();
		}
		this.MusicOff.SetActive(!this.MusicToggle.isOn);
		this.MusicOn.SetActive(this.MusicToggle.isOn);
	}

	public void OnSoundClick(bool isOn)
	{
		Singleton<GameAudioManager>.Instance.GameSound = this.SoundToggle.isOn;
		this.SoundOff.SetActive(!this.SoundToggle.isOn);
		this.SoundOn.SetActive(this.SoundToggle.isOn);
		if (!isOn)
		{
			Singleton<GameAudioManager>.Instance.StopAllSound();
		}
	}

	public void ChangeSensitivity()
	{
		Singleton<GlobalData>.Instance.Sensitivity = (int)this.SensitivitySlider.value;
		this.SliderValueText.text = ((int)((this.SensitivitySlider.value - this.SensitivitySlider.minValue) / (this.SensitivitySlider.maxValue - this.SensitivitySlider.minValue) * 100f)).ToString();
	}

	private void RefalshPageData()
	{
		this.SensitivitySlider.value = (float)Singleton<GlobalData>.Instance.Sensitivity;
		this.MusicToggle.isOn = Singleton<GameAudioManager>.Instance.GameMusic;
		this.SoundToggle.isOn = Singleton<GameAudioManager>.Instance.GameSound;
	}

	public void RefreshShootingMode()
	{
		this.ShootModeText.text = Singleton<GlobalData>.Instance.GetText("SHOOTING_MODE");
		this.AutoTag.gameObject.SetActive(Singleton<GlobalData>.Instance.ShootingMode == 0);
		this.AutoText.text = Singleton<GlobalData>.Instance.GetText("SHOOTING_MODE_AUTO");
		this.ManualTag.gameObject.SetActive(Singleton<GlobalData>.Instance.ShootingMode == 1);
		this.ManualText.text = Singleton<GlobalData>.Instance.GetText("SHOOTING_MODE_MANUAL");
		Singleton<FontChanger>.Instance.SetFont(ShootModeText);
		Singleton<FontChanger>.Instance.SetFont(AutoText);
		Singleton<FontChanger>.Instance.SetFont(ManualText);
	}

	public void SelectShootingMode(int i)
	{
		Singleton<GlobalData>.Instance.ShootingMode = i;
		if (GameApp.GetInstance().GetGameScene() != null)
		{
			GameApp.GetInstance().GetGameScene().ControlMode = (GameControlMode)i;
		}
		this.RefreshPage();
	}

	public void OnClickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.OnBack();
	}

	public CanvasGroup Content;

	public Text TitleText;

	public Text SensitivityText;

	public Text MusicButtonText;

	public Text SoundButtonText;

	public Text LanguageTitleText;

	public Text LanguageButtonText;

	public Text ConfirmButtonText;

	public Text SliderValueText;

	public Transform LanguageRoot;

	public Transform LanguageLayer;

	public Slider SensitivitySlider;

	public Toggle MusicToggle;

	public Toggle SoundToggle;

	public Text ShootModeText;

	public Image AutoTag;

	public Text AutoText;

	public Image ManualTag;

	public Text ManualText;

	public Text musicTxtOpen;

	public Text musicTxtClose;

	public Text SoundTxtOpen;

	public Text SoundTxtClose;

	public GameObject MusicOn;

	public GameObject MusicOff;

	public GameObject SoundOn;

	public GameObject SoundOff;

	private string[] GameLanguages = new string[]
	{
		"English",
		"Русский"
	};
}
