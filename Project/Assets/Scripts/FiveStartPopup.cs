using System;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class FiveStartPopup : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.DOFade(1f, 0.4f).OnComplete(delegate
		{
		});
		this.DesTxt.text = Singleton<GlobalData>.Instance.GetText("FIVESTAR");
		this.AdviceTxt.text = Singleton<GlobalData>.Instance.GetText("ADVICE");
		this.BuyTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		this.TitleTxt.text = Singleton<GlobalData>.Instance.GetText("PARTICIPATION");
		Singleton<FontChanger>.Instance.SetFont(DesTxt);
		Singleton<FontChanger>.Instance.SetFont(AdviceTxt);
		Singleton<FontChanger>.Instance.SetFont(BuyTxt);
		Singleton<FontChanger>.Instance.SetFont(TitleTxt);
	}

	public void OnclickAdvice()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		//GMGSDK.showComplaintDialog();
	}

	public void OnclickYes()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		//GMGSDK.jumpToGP();
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public Text TitleTxt;

	public Text DesTxt;

	public Text AdviceTxt;

	public Text BuyTxt;

	public CanvasGroup canvasGroup;
}
