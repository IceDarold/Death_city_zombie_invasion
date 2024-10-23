using System;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradePopup : GamePage
{
	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 1f).OnComplete(delegate
		{
		});
		this.PopupName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
		this.GoldCount.text = this.Golds.ToString();
		this.DiamondCount.text = this.Diamonds.ToString();
		this.ConfirmName.text = Singleton<GlobalData>.Instance.GetText("CONFIRM");
		Singleton<FontChanger>.Instance.SetFont(PopupName);
		Singleton<FontChanger>.Instance.SetFont(GoldCount);
		Singleton<FontChanger>.Instance.SetFont(DiamondCount);
		Singleton<FontChanger>.Instance.SetFont(ConfirmName);
	}

	public void OnConfirmClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		ItemDataManager.SetCurrency(CommonDataType.GOLD, this.Golds);
		ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.Diamonds);
		Singleton<UiManager>.Instance.ShowEarnMoneyEffect(this.Golds, this.Diamonds, 0);
		Singleton<UiManager>.Instance.TopBar.Refresh();
		Singleton<UiManager>.Instance.ClosePage(PageType.Popup, null);
	}

	public Text PopupName;

	public Text GoldCount;

	public Text DiamondCount;

	public Text ConfirmName;

	public int Golds;

	public int Diamonds;

	public CanvasGroup canvasGroup;
}
