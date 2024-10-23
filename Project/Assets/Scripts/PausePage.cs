using System;
using ads;
using DataCenter;
using DG.Tweening;
using RacingMode;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class PausePage : GamePage
{
	public new void OnEnable()
	{
		Ads.ShowInter();
        //Advertisements.Instance.ShowInterstitial();
		//Singleton<GlobalData>.Instance.ShowAdvertisement(14, null, null);
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 1f);
		this.exitTxt.text = Singleton<GlobalData>.Instance.GetText("EXIT");
		this.opertionTxt.text = Singleton<GlobalData>.Instance.GetText("OPTION");
		this.continueTxt.text = Singleton<GlobalData>.Instance.GetText("CONTINUE");
		this.titleTxt.text = Singleton<GlobalData>.Instance.GetText("CURRENT_MISSION");
		Singleton<FontChanger>.Instance.SetFont(exitTxt);
		Singleton<FontChanger>.Instance.SetFont(opertionTxt);
		Singleton<FontChanger>.Instance.SetFont(continueTxt);
		Singleton<FontChanger>.Instance.SetFont(titleTxt);
		Singleton<FontChanger>.Instance.SetFont(contentTxt);
		if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.RACING)
		{
			Mission curMission = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().CurMission;
			if (curMission == null)
			{
				return;
			}
			string format = (Singleton<GlobalData>.Instance.GetCurrentLanguage() != LanguageEnum.Russian) ? curMission.description_en : curMission.description_ru;
			if (curMission != null)
			{
				this.contentTxt.text = string.Format(format, curMission.curAmount, curMission.targetAmount);
			}
		}
		else
		{
			this.contentTxt.text = string.Empty;
		}
	}

	public void OnOptionClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.OptionPage, null);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.OptionPage.ToString());
	}

	public override void Close()
	{
		base.Close();
	}

	public override void OnBack()
	{
		this.Close();
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			RacingSceneManager.Instance.SetPause(false);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
		}
		else
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.InGamePage, null);
			this.SetGameState(PlayingState.GamePlaying);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		}
	}

	public void OnContinueClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			RacingSceneManager.Instance.SetPause(false);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
		}
		else
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.InGamePage, null);
			this.SetGameState(PlayingState.GamePlaying);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		}
	}

	public void SetGameState(PlayingState gameState)
	{
		GameApp.GetInstance().GetGameScene().PlayingState = gameState;
	}

	public void OnExitClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.GameGoldCoins = 0;
		Singleton<GlobalData>.Instance.GameDNA = 0;
		Singleton<GameAudioManager>.Instance.ClearAudio();
		this.Close();
		Singleton<UiManager>.Instance.ShowTopBar(true);
		if (Singleton<UiManager>.Instance.GetPage(PageName.DialoguePage) != null)
		{
			Singleton<UiManager>.Instance.GetPage(PageName.DialoguePage).Close();
		}
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			Singleton<UiManager>.Instance.CurrentPage.Close();
		}
		else
		{
			Singleton<UiManager>.Instance.RemovePage(PageName.InGamePage);
		}
		Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.MainPage);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.MainPage.ToString());
		Singleton<DropItemManager>.Instance.Recycle();
		Singleton<GamePropManager>.Instance.Recycle();
	}

	public CanvasGroup canvasGroup;

	public Text exitTxt;

	public Text opertionTxt;

	public Text continueTxt;

	public Text titleTxt;

	public Text contentTxt;

	private IPlayerControl player;
}
