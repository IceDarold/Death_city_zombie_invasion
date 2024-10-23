using System;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class HallowmasGiftPage : GamePage
{
	public override void Show()
	{
		this.gift = StoreDataManager.GetStoreData(9041);
		if (this.gift == null)
		{
			return;
		}
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		base.Refresh();
		this.Title.text = Singleton<GlobalData>.Instance.GetText(this.gift.Name);
		this.Describe.text = Singleton<GlobalData>.Instance.GetText(this.gift.Describe).Replace("\\n", "\n");
		//this.BuyName.text = "$ " + StoreDataManager.GetChargePoint(this.gift.ChargePoint).Price;
		this.BuyName.text = Singleton<InApps>.Instance.GetPrice(gift.ChargePoint);
		Singleton<FontChanger>.Instance.SetFont(Title);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(BuyName);
	}

	public void ClickOnBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.DoCharge(this.gift.ChargePoint, delegate
		{
			// Product();
		});
	}

	public void Product()
	{
		StoreDataManager.Buy(9041);
		this.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
		Singleton<UiManager>.Instance.ShowAward(this.gift.ItemID, this.gift.ItemCount, null);
	}

	public void ClociOnClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
	}

	public CanvasGroup Content;

	public Text Title;

	public Text Describe;

	public Text BuyName;

	private StoreData gift;
}
