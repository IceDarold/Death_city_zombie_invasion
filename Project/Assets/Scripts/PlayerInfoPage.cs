using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPage : GamePage
{
	private new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.RefreshPage();
	}

	public void RefreshPage()
	{
		this.TitleNameTxt.text = Singleton<GlobalData>.Instance.GetText("CAREER");
		this.CloseBtnTxt.text = Singleton<GlobalData>.Instance.GetText("EXIT");
		this.PropertyTxt[0].text = Singleton<GlobalData>.Instance.GetText("KILL_ZOMBIES");
		this.PropertyTxt[1].text = Singleton<GlobalData>.Instance.GetText("PASS_CHECKPOINT");
		this.PropertyTxt[2].text = Singleton<GlobalData>.Instance.GetText("GAME_TIME");
		this.PropertyTxt[3].text = Singleton<GlobalData>.Instance.GetText("EARN_GOLDS");
		this.PropertyTxt[4].text = Singleton<GlobalData>.Instance.GetText("EARN_DIAMONDS");
		this.PropertyTxt[5].text = Singleton<GlobalData>.Instance.GetText("HIT_RATE");
		this.PropertyTxt[6].text = Singleton<GlobalData>.Instance.GetText("HEADSHOT_RATE");
		this.PropertyNumTxt[0].text = PlayerDataManager.Player.KillZombieCounts + string.Empty;
		this.PropertyNumTxt[1].text = PlayerDataManager.Player.CheckpointPassedTimes + string.Empty;
		this.PropertyNumTxt[2].text = PlayerDataManager.Player.GameDuration / 60 + string.Empty;
		this.PropertyNumTxt[3].text = PlayerDataManager.Player.EarnGoldCoins + string.Empty;
		this.PropertyNumTxt[4].text = PlayerDataManager.Player.EarnDiamonds + string.Empty;
		if (PlayerDataManager.Player.TotalShootTimes == 0)
		{
			this.PropertyNumTxt[5].text = "0";
			this.PropertyNumTxt[6].text = "0";
		}
		else
		{
			this.PropertyNumTxt[5].text = (int)((float)PlayerDataManager.Player.ShootHitTimes / (float)PlayerDataManager.Player.TotalShootTimes * 100f) + "%";
			this.PropertyNumTxt[6].text = (int)((float)PlayerDataManager.Player.ShootHeadTimes / (float)PlayerDataManager.Player.TotalShootTimes * 100f) + "%";
		}
		Singleton<FontChanger>.Instance.SetFont(TitleNameTxt);
		Singleton<FontChanger>.Instance.SetFont(CloseBtnTxt);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[0]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[1]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[2]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[3]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[4]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[5]);
		Singleton<FontChanger>.Instance.SetFont(PropertyTxt[6]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[0]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[1]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[2]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[3]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[4]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[5]);
		Singleton<FontChanger>.Instance.SetFont(PropertyNumTxt[6]);
	}

	public void OnclickExit()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public Text TitleNameTxt;

	public Text CloseBtnTxt;

	public Text[] PropertyTxt;

	public Text[] PropertyNumTxt;

	public CanvasGroup canvasGroup;
}
