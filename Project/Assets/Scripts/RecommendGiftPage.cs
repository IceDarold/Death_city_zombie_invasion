using System;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class RecommendGiftPage : GamePage
{
	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.2f);
		this.Content.transform.localPosition = Vector3.zero;
		this.Content.transform.DOLocalMoveY(90f, 0.2f, false).From<Tweener>();
	}

	private void RefreshPage()
	{
	}

	public override void OnBack()
	{
		this.Close();
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.InGamePage) && Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).gameObject.activeSelf && GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.GameLose)
		{
			GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePlaying;
		}
		else if (null != Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage) && Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage).gameObject.activeSelf)
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage).Refresh();
		}
	}

	public override void Refresh()
	{
		this.titleTxt.text = Singleton<GlobalData>.Instance.GetText(this.GiftData.Name);
		this.icon.sprite = Singleton<UiManager>.Instance.GetSprite(this.GiftData.PushIcon);
		this.desTxt.text = Singleton<GlobalData>.Instance.GetText(this.GiftData.Describe).Replace("\\n", "\n");
		this.DiscountText.gameObject.SetActive(this.GiftData.Discount != 0);
		this.DiscountText.text = string.Concat(new object[]
		{
			Singleton<GlobalData>.Instance.GetText("PRICE"),
			"-",
			this.GiftData.Discount,
			"%"
		});
		//this.buyTxt.text = "$" + StoreDataManager.GetChargePoint(this.GiftData.ChargePoint).Price;
		this.buyTxt.text = Singleton<InApps>.Instance.GetPrice(GiftData.ChargePoint);
		if (this.GiftData.Discount > 0)
		{
			this.OriginalPrice.gameObject.SetActive(true);
			//this.OriginalPrice.text = "$" + (int)(StoreDataManager.GetChargePoint(this.GiftData.ChargePoint).Price / (1f - (float)this.GiftData.Discount * 0.01f));
			//TODO:Price
			this.OriginalPrice.text = (int)(Singleton<InApps>.Instance.GetValuePrice(GiftData.ChargePoint) / (1f - (float)this.GiftData.Discount * 0.01f)) + Singleton<InApps>.Instance.GetCurrency();
		}
		else
		{
			this.OriginalPrice.gameObject.SetActive(false);
		}
		Singleton<FontChanger>.Instance.SetFont(titleTxt);
		Singleton<FontChanger>.Instance.SetFont(desTxt);
		Singleton<FontChanger>.Instance.SetFont(DiscountText);
		Singleton<FontChanger>.Instance.SetFont(buyTxt);
		Singleton<FontChanger>.Instance.SetFont(OriginalPrice);
	}

	public void OnclickBuy()
	{
		Singleton<GlobalData>.Instance.DoCharge(this.GiftData.ChargePoint, delegate
		{
			this.OnclickBuySucess();
		});
	}

	public void OnclickBuySucess()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		StoreDataManager.Buy(this.GiftData.ID);
		Singleton<UiManager>.Instance.ShowAward(this.GiftData.ItemID, this.GiftData.ItemCount, null);
		if (this.GiftData.Tag == 1 && this.GiftData.Type == 1)
		{
			UITick.setADDITIONALSec(1, 0);
			UITick.SetVipTime();
		}
		else if (UITick.getPushGiftType() != 0)
		{
			UITick.setPushGiftSec(0, 0);
		}
		this.Close();
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public StoreData GiftData;

	public CanvasGroup Content;

	public Image icon;

	public Text titleTxt;

	public Text buyTxt;

	public Text desTxt;

	public Text DiscountText;

	public Text OriginalPrice;
}
