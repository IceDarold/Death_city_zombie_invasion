using System;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LackOfMoneyPopup : GamePage
{
	private new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		ChargePoint chargePoint = StoreDataManager.GetChargePoint(this.ChargePointID);
		StoreData storeData = StoreDataManager.GetStoreData(chargePoint);
		// this.BuyPrice.text = "$" + chargePoint.Price.ToString();
		this.BuyPrice.text = Singleton<InApps>.Instance.GetPrice(ChargePointID);
		ShopName.text = Singleton<GlobalData>.Instance.GetText("SHOP");
		Singleton<FontChanger>.Instance.SetFont(ShopName);
		if (this.currencyType == CommonDataType.GOLD)
		{
			if (ItemDataManager.GetCurrency(CommonDataType.DOUBLE) == 0)
			{
				this.ShopGo.SetActive(true);
				this.DoubleGo.SetActive(true);
				this.DoubleTxt.text = Singleton<GlobalData>.Instance.GetText("DOUBLE");
				Singleton<FontChanger>.Instance.SetFont(DoubleTxt);
			}
			else
			{
				this.ShopGo.SetActive(true);
				this.DoubleGo.SetActive(false);
			}
		}
		else if (this.currencyType == CommonDataType.DIAMOND)
		{
			this.ShopGo.SetActive(true);
			this.DoubleGo.SetActive(false);
		}
		else if (this.currencyType == CommonDataType.DNA)
		{
			this.ShopGo.SetActive(true);
			this.DoubleGo.SetActive(false);
		}
		else if (this.currencyType == CommonDataType.REVIVE_COIN)
		{
			this.ShopGo.SetActive(false);
			this.DoubleGo.SetActive(false);
		}
		this.Describe.text = string.Concat(new object[]
		{
			Singleton<GlobalData>.Instance.GetText("NO_ENOUGH"),
			Singleton<GlobalData>.Instance.GetText(this.currencyType.ToString()),
			"\n",
			Singleton<GlobalData>.Instance.GetText("WILL_BUY"),
			storeData.ItemCount[0],
			Singleton<GlobalData>.Instance.GetText(this.currencyType.ToString())
		});
		Singleton<FontChanger>.Instance.SetFont(Describe);
	}

	public void OnShopClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowTopBar(true);
		Singleton<UiManager>.Instance.PageStack.Peek().Hide();
		this.Close();
		switch (this.currencyType)
		{
		case CommonDataType.GOLD:
			Singleton<UiManager>.Instance.ShowStorePage(1);
			break;
		case CommonDataType.DIAMOND:
			Singleton<UiManager>.Instance.ShowStorePage(2);
			break;
		case CommonDataType.DNA:
			Singleton<UiManager>.Instance.ShowStorePage(3);
			break;
		default:
			Singleton<UiManager>.Instance.ShowStorePage(0);
			break;
		}
	}

	public void OnBuyClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.DoCharge(this.ChargePointID, delegate
		{
			StoreDataManager.BuyChargePoint(this.ChargePointID);
			StoreData storeData = StoreDataManager.GetStoreData(StoreDataManager.GetChargePoint(this.ChargePointID));
			Singleton<UiManager>.Instance.ShowAward(storeData.ItemID, storeData.ItemCount, null);
		});
		Singleton<UiManager>.Instance.TopBar.Refresh();
		this.Close();
	}

	public void OnclickDouble()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		StoreData data = StoreDataManager.GetStoreData(9011);
		Singleton<GlobalData>.Instance.DoCharge(data.ChargePoint, delegate
		{
			StoreDataManager.Buy(data.ID);
			Singleton<UiManager>.Instance.ShowAward(data.ItemID, data.ItemCount, null);
			this.Close();
		});
	}

	public override void Close()
	{
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage) && Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage).gameObject.activeSelf)
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FailedNoticePage).Refresh();
		}
		base.Close();
	}

	public void OnCloseClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
	}

	public CanvasGroup canvasGroup;

	public GameObject DoubleGo;

	public GameObject ShopGo;

	public Text Describe;

	public Text ShopName;

	public Text BuyPrice;

	public Text DoubleTxt;

	[HideInInspector]
	public CommonDataType currencyType;

	[HideInInspector]
	public int ChargePointID;

	public int Count;
}
