using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class VipDayRewardPopup : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.DesTxt.text = Singleton<GlobalData>.Instance.GetText("Vip_Day_Reward");
		this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		this.NumTxt.text = "X" + 80;
		Singleton<FontChanger>.Instance.SetFont(DesTxt);
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 80);
		Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 80, 0);
		Singleton<GlobalData>.Instance.TodayVIPReward = 0;
		Singleton<UiManager>.Instance.TopBar.Refresh();
		this.Close();
	}

	public Text DesTxt;

	public Text BtnTxt;

	public Text NumTxt;

	public CanvasGroup canvasGroup;
}
