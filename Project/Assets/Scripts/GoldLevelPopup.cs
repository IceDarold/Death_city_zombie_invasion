using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class GoldLevelPopup : GamePage
{
	public new void OnEnable()
	{
		this.data = CheckpointDataManager.GetGoldCheckpoint();
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.RefreshPage();
	}

	public void RefreshPage()
	{
		this.TitleTxt.text = Singleton<GlobalData>.Instance.GetText("MORE_MONEY");
		this.DescribeTxt.text = Singleton<GlobalData>.Instance.GetText("GOLD_LEVEL_DES");
		this.BtnNameTxt.text = Singleton<GlobalData>.Instance.GetText("CHALLENGE");
		Singleton<FontChanger>.Instance.SetFont(TitleTxt);
		Singleton<FontChanger>.Instance.SetFont(DescribeTxt);
		Singleton<FontChanger>.Instance.SetFont(BtnNameTxt);
		if (PlayerPrefs.GetInt("GOLD_CHECKPOINT", 2) == 2)
		{
			this.ChallengeButton.interactable = true;
			this.Timer.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("GOLD_CHECKPOINT", 2) == 1)
		{
			if (3600.0 - GameTimeManager.CalculateTimeToNow("GOLD_CHECKPOINT") > 0.0)
			{
				this.ChallengeButton.interactable = false;
				this.dt = 3600f - (float)GameTimeManager.CalculateTimeToNow("GOLD_CHECKPOINT");
				this.Timer.gameObject.SetActive(true);
			}
			else
			{
				this.ChallengeButton.interactable = true;
				this.Timer.gameObject.SetActive(false);
			}
		}
		else
		{
			this.ChallengeButton.interactable = false;
			this.dt = (float)GameTimeManager.GetLeftSecondsToday();
			this.Timer.gameObject.SetActive(true);
		}
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public void OnclickChallenge()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		if (Singleton<GlobalData>.Instance.GetEnergy() <= 0)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.EnergyShortagePage, null);
			return;
		}
		Singleton<GlobalData>.Instance.SetEnergy(-1);
		CheckpointDataManager.SelectCheckpoint = this.data;
		Singleton<GlobalData>.Instance.AdvertisementReviveTimes = 0;
		MapsPage.instance.isScence = true;
		MapsPage.instance.Close();
		Singleton<UiManager>.Instance.ShowTopBar(false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(this.data.SceneID), PageName.InGamePage);
	}

	private void Update()
	{
		if (this.Timer.gameObject.activeSelf)
		{
			if (this.dt > 0f)
			{
				this.dt -= Time.deltaTime;
				this.Timer.text = GameTimeManager.ConvertToString((int)this.dt);
			}
			else
			{
				this.ChallengeButton.interactable = true;
				this.Timer.gameObject.SetActive(false);
			}
		}
	}

	public CanvasGroup canvasGroup;

	public Text TitleTxt;

	public Text DescribeTxt;

	public Text BtnNameTxt;

	public Text Timer;

	public Button ChallengeButton;

	public CheckpointData data;

	private float dt;
}
