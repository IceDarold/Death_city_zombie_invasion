using DataCenter;
using DG.Tweening;
using RacingMode;
using ui;
using UnityEngine;
using UnityEngine.UI;

public class InRacingPage : GamePage
{
	public override void Show()
	{
		this.BloodSlider.value = (this.SliderValue = 1f);
		this.coolDown = 0f;
		Singleton<GlobalData>.Instance.GameDNA = 0;
		Singleton<GlobalData>.Instance.GameGoldCoins = 0;
		base.Show();
		Singleton<GameAudioManager>.Instance.PlayMusic(Singleton<GameAudioManager>.Instance.RacingBGM);
		GameLogManager.StartGameLog();
		this.BloodImage.gameObject.SetActive(false);
		this.SkillButtonMask.gameObject.SetActive(false);
		this.SkillButton.interactable = true;
		LeftImage.gameObject.SetActive(Singleton<UiControllers>.Instance.IsMobile);
		RightImage.gameObject.SetActive(Singleton<UiControllers>.Instance.IsMobile);
	}

	public override void OnBack()
	{
		RacingSceneManager.Instance.SetPause(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.PausePage, null);
	}

	public void SetBlood(float value)
	{
		this.SliderValue = value;
	}

	public void OnPauseClick()
	{
		RacingSceneManager.Instance.SetPause(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.PausePage, null);
	}

	public void ClickOnSkill()
	{
		RacingSceneManager.Instance.Car.ShockWave();
		this.coolDown = this.SkillCoolDown;
		this.SkillButtonMask.gameObject.SetActive(true);
		this.SkillButton.interactable = false;
	}

	public void OnLeftDown()
	{
		this.direction = -1;
		this.LeftImage.localScale = Vector3.one * 0.8f;
	}

	public void OnLeftUp()
	{
		if (this.direction == -1)
		{
			this.direction = 0;
		}
		this.LeftImage.localScale = Vector3.one;
	}

	public void OnRightDown()
	{
		this.direction = 1;
		this.RightImage.localScale = Vector3.one * 0.8f;
	}

	public void OnRightUp()
	{
		if (this.direction == 1)
		{
			this.direction = 0;
		}
		this.RightImage.localScale = Vector3.one;
	}

	public void PlayHurtParticle()
	{
		this.BloodImage.gameObject.SetActive(true);
		this.BloodImage.color = Color.white;
		this.BloodImage.DOColor(new Color(1f, 1f, 1f, 0f), 1f).OnComplete(delegate
		{
			this.BloodImage.gameObject.SetActive(false);
		});
	}

	private void Update()
	{
		this.BloodSlider.value = Mathf.Lerp(this.BloodSlider.value, this.SliderValue, Time.deltaTime * 8f);
		if (RacingSceneManager.Instance.GameState != RacingState.PAUSE)
		{
			if (this.coolDown > 0f)
			{
				this.coolDown -= Time.deltaTime;
				this.SkillButtonMask.fillAmount = this.coolDown / this.SkillCoolDown;
			}
			else
			{
				this.SkillButtonMask.gameObject.SetActive(false);
				this.SkillButton.interactable = true;
			}
		}
	}

	private void DesktopInput()
	{
		if (Singleton<UiControllers>.Instance.IsMobile) return;
		this.direction = Input.GetAxis("Horizontal");
	}

	private void FixedUpdate()
	{
		DesktopInput();
		if (RacingSceneManager.Instance.GameState == RacingState.RACING)
		{
			if (this.direction < 0)
			{
				RacingSceneManager.Instance.Car.TurningLeft();
			}
			else if (this.direction > 0)
			{
				RacingSceneManager.Instance.Car.TurningRight();
			}
			else
			{
				RacingSceneManager.Instance.Car.StopTurning();
			}
		}
	}

	public Slider BloodSlider;

	private float SliderValue;

	public Button SkillButton;

	public Image SkillButtonMask;

	public float SkillCoolDown = 20f;

	public Transform LeftImage;

	public Transform RightImage;

	public Image BloodImage;

	private float coolDown;

	private float direction;
}
