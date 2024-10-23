using System;
using DataCenter;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
	public void Refresh(StoreData _data)
	{
		Debug.Log($"REFRESH {_data.ID}");
		this.data = _data;
		this.Name.text = Singleton<GlobalData>.Instance.GetText(_data.Name);
		Singleton<FontChanger>.Instance.SetFont(Name);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(_data.Icon);
		this.Icon.SetNativeSize();
		this.DiscountObject.SetActive(this.data.Discount != 0);
		this.Discount.text = "-" + this.data.Discount.ToString() + "%";
		this.Description.text = Singleton<GlobalData>.Instance.GetText("SEE_DETAILS");
		Singleton<FontChanger>.Instance.SetFont(Description);
		this.Backlight.sprite = this.LightSprites[this.data.Type - 1];
		switch (this.data.Type)
		{
		case 1:
		case 2:
		case 3:
			this.Count.gameObject.SetActive(true);
			this.Count.text = "X " + this.data.ItemCount[0];
			this.InfoButton.gameObject.SetActive(false);
			this.BuyButton.interactable = true;
			// this.Price.text = "$" + StoreDataManager.GetChargePoint(this.data.ChargePoint).Price;
			this.Price.text = Singleton<InApps>.Instance.GetPrice(data.ChargePoint);
			break;
		case 4:
			this.Count.gameObject.SetActive(false);
			this.InfoButton.gameObject.SetActive(true);
			if (this.data.Tag == 1 && this.data.Purchased)
			{
				this.BuyButton.interactable = false;
				this.Price.text = Singleton<GlobalData>.Instance.GetText("ALREADY_BUY");
			}
			else
			{
				this.BuyButton.interactable = true;
				// this.Price.text = "$" + StoreDataManager.GetChargePoint(this.data.ChargePoint).Price;
				this.Price.text = Singleton<InApps>.Instance.GetPrice(data.ChargePoint);
			}
			Singleton<FontChanger>.Instance.SetFont(Price);
			break;
		}
	}

	public void ClickOnBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.DoCharge(this.data.ChargePoint, delegate
		{
			//Product(data);
		});
	}

	public void Product(StoreData data)
	{
		StoreDataManager.Buy(data.ID);
		if (data.ID == 9010)
		{
			UITick.SetVipTime();
			Singleton<GlobalData>.Instance.TodayVIPReward = 0;
		}
		else if (data.ID == 9013)
		{
			PlayerPrefs.SetInt("MONTHLY_CARD", 30);
			PlayerPrefs.SetInt("MONTHLY_CARD_DAILY_AWARD", 1);
		}
		this.Refresh(data);
		Singleton<UiManager>.Instance.ShowAward(data.ItemID, data.ItemCount, null);
	}

	public void ClickOnInstructions()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.ShopItemPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.ShopItemPage).GetComponent<ShopItemPage>().sd = this.data;
		});
	}

	public Text Name;

	public Text Discount;

	public Text Price;

	public Text Count;

	public Text Description;

	public Button BuyButton;

	public Button InfoButton;

	public Image Icon;

	public Image Backlight;

	public GameObject DiscountObject;

	public Sprite[] LightSprites;

	private StoreData data;
}
