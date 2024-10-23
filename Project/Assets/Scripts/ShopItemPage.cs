using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPage : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		this.DescTxt.text = Singleton<GlobalData>.Instance.GetText(this.sd.Describe).Replace("\\n", "\n");
		this.NameTxt.text = Singleton<GlobalData>.Instance.GetText(this.sd.Name);
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
		Singleton<FontChanger>.Instance.SetFont(DescTxt);
		Singleton<FontChanger>.Instance.SetFont(NameTxt);
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public Text NameTxt;

	public Text DescTxt;

	public Text BtnTxt;

	public StoreData sd;

	public CanvasGroup canvasGroup;
}
