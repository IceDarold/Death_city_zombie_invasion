using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PayPromptPopup : GamePage
{
	private new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		this.Sure.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		this.Cancel.text = Singleton<GlobalData>.Instance.GetText("CANCEL");
		this.Describe.text = string.Concat(new object[]
		{
			Singleton<GlobalData>.Instance.GetText("WILL_COST"),
			this.Count,
			" ",
			Singleton<GlobalData>.Instance.GetText(this.currencyType.ToString()),
			this.SomeThing,
			"?"
		});
		Singleton<FontChanger>.Instance.SetFont(Sure);
		Singleton<FontChanger>.Instance.SetFont(Cancel);
		Singleton<FontChanger>.Instance.SetFont(Describe);
	}

	public void OnclickSure()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (ItemDataManager.GetCurrency(this.currencyType) >= this.Count)
		{
			ItemDataManager.SetCurrency(this.currencyType, -this.Count);
			this.action();
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(this.currencyType, this.Count - ItemDataManager.GetCurrency(this.currencyType));
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.LackOfMoneyPopup.ToString());
		}
		this.Close();
	}

	public void OnCloseClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public CanvasGroup canvasGroup;

	public Text Describe;

	public Text Cancel;

	public Text Sure;

	public CommonDataType currencyType;

	public int Count;

	public string SomeThing;

	public UnityAction action;
}
