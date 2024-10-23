using System;
using ads;
using DataCenter;
using RacingMode;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class FailedNoticePage : GamePage
{
	public override void Show()
	{
		base.Show();
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePause;
	}

	public override void Refresh()
	{
		this.TitleNameTxt.text = Singleton<GlobalData>.Instance.GetText("FAILED");
		this.EndTxt.text = Singleton<GlobalData>.Instance.GetText("END");
		Singleton<FontChanger>.Instance.SetFont(TitleNameTxt);
		Singleton<FontChanger>.Instance.SetFont(EndTxt);
		Singleton<FontChanger>.Instance.SetFont(RebirthNumTxt);
		Singleton<FontChanger>.Instance.SetFont(RebirthTxt);
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.MAINLINE_SNIPE || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.SNIPE)
		{
			this.ReviveCoinCount.gameObject.SetActive(false);
			this.RebirthTxt.text = Singleton<GlobalData>.Instance.GetText("RESTART");
			this.RebirthNumTxt.gameObject.SetActive(false);
			this.ReviveCostIcon.gameObject.SetActive(false);
			this.AdReviveButton.gameObject.SetActive(false);
		}
		else
		{
			this.ReviveCoinCount.gameObject.SetActive(true);
			this.ReviveCostIcon.gameObject.SetActive(true);
			this.AllAmoutTxt.text = Singleton<GlobalData>.Instance.GetText("HAVEGOT") + ":";
			this.AllAmoutNumTxt.text = ItemDataManager.GetCurrency(CommonDataType.REVIVE_COIN).ToString();
			this.RebirthTxt.text = Singleton<GlobalData>.Instance.GetText("REVIVE");
			this.RebirthNumTxt.gameObject.SetActive(true);
			this.RebirthNumTxt.text = this.cost.ToString();
			if (Singleton<GlobalData>.Instance.AdvertisementReviveTimes > 0)
			{
				this.AdReviveButton.gameObject.SetActive(true);
				this.AdReviveName.text = Singleton<GlobalData>.Instance.GetText("REVIVE");
				Singleton<FontChanger>.Instance.SetFont(AdReviveName);
			}
			else
			{
				this.AdReviveButton.gameObject.SetActive(false);
			}
		}
	}

	public override void OnBack()
	{
		base.OnBack();
		Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Close();
		Singleton<UiManager>.Instance.ShowPage(PageName.CommonFinishPage, null);
	}

	public void OnclickEnd()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		Singleton<UiManager>.Instance.GameSuccess = -1;
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.GameOverPage.ToString());
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			Singleton<UiManager>.Instance.GetPage(PageName.InRacingPage).Close();
		}
		else
		{
			Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).Close();
		}
		Singleton<UiManager>.Instance.ShowPage(PageName.GameOverPage, null);
	}

	public void OnclickReBirth()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.MAINLINE_SNIPE || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.SNIPE)
		{
			this.Close();
			Singleton<UiManager>.Instance.RemovePage(PageName.InGamePage);
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
			Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
		}
		else if (ItemDataManager.GetCurrency(CommonDataType.REVIVE_COIN) >= this.cost)
		{
			this.Close();
			ItemDataManager.SetCurrency(CommonDataType.REVIVE_COIN, -this.cost);
			if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
			{
				RacingSceneManager.Instance.SetRevive();
				GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
			}
			else
			{
				GameApp.GetInstance().GetGameScene().reviveCount++;
				Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().Recevie();
				GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
			}
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.REVIVE_COIN, 1);
		}
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            this.Close();
            Singleton<GlobalData>.Instance.AdvertisementReviveTimes--;
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
            {
                RacingSceneManager.Instance.SetRevive();
                GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
            }
            else
            {
                GameApp.GetInstance().GetGameScene().reviveCount++;
                Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().Recevie();
                GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
            }
        }

    }
    public void ClickOnAdvertisementRevive()
    {
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
        Ads.ShowReward(() =>
        {
		    this.Close();
		    Singleton<GlobalData>.Instance.AdvertisementReviveTimes--;
		    if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		    {
		        RacingSceneManager.Instance.SetRevive();
		        GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
		    }
		    else
		    {
		        GameApp.GetInstance().GetGameScene().reviveCount++;
		        Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().Recevie();
		        GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		    }
        });
  //      Singleton<GlobalData>.Instance.ShowAdvertisement(0, delegate
		//{
		//	this.Close();
		//	Singleton<GlobalData>.Instance.AdvertisementReviveTimes--;
		//	if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		//	{
		//		RacingSceneManager.Instance.SetRevive();
		//		GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
		//	}
		//	else
		//	{
		//		GameApp.GetInstance().GetGameScene().reviveCount++;
		//		Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().Recevie();
		//		GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		//	}
		//}, null);
	}

	public void UseReBirth()
	{
		Singleton<UiManager>.Instance.ClosePage(PageType.Popup, null);
		this.SetGameState(PlayingState.GamePlaying);
	}

	public void SetGameState(PlayingState gameState)
	{
		GameApp.GetInstance().GetGameScene().PlayingState = gameState;
	}

	public Text TitleNameTxt;

	public Text EndTxt;

	public Text RebirthTxt;

	public Text RebirthNumTxt;

	public Text AllAmoutTxt;

	public Text AllAmoutNumTxt;

	public Button AdReviveButton;

	public Text AdReviveName;

	public Transform ReviveCoinCount;

	public Image ReviveCostIcon;

	private IPlayerControl player;

	private IGameSceneControl gameScene;

	private int cost = 1;
}
