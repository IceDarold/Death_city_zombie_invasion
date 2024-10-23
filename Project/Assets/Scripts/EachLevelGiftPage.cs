using System;
using System.Collections.Generic;
using DataCenter;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EachLevelGiftPage : GamePage
{
	private new void Awake()
	{
		this.gifts = EachLevelGiftSystem.GetEachLevelGiftDatas();
		this.CreateChildren();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	private void CreateChildren()
	{
		this.ChildrenRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(180 * this.gifts.Count + 20), 410f);
		for (int i = 0; i < this.gifts.Count; i++)
		{
			EachLevelGiftItem eachLevelGiftItem = UnityEngine.Object.Instantiate<EachLevelGiftItem>(this.ItemPrefab);
			eachLevelGiftItem.transform.SetParent(this.ChildrenRoot);
			eachLevelGiftItem.transform.localPosition = Vector3.zero;
			eachLevelGiftItem.transform.localScale = Vector3.one;
			this.EachLevelGifts.Add(eachLevelGiftItem);
			eachLevelGiftItem.gameObject.SetActive(true);
		}
	}

	public override void Refresh()
	{
		this.Title.text = Singleton<GlobalData>.Instance.GetText("STORE_NAME_43");
		Singleton<FontChanger>.Instance.SetFont(Title);
		for (int i = 0; i < this.EachLevelGifts.Count; i++)
		{
			this.EachLevelGifts[i].Refresh(this.gifts[i]);
		}
		bool flag = ItemDataManager.GetCurrency(CommonDataType.EACH_LEVEL_AWARD_GIFT) != 0;
		this.BuyButton.gameObject.SetActive(!flag);
		this.BuyDescribe.gameObject.SetActive(!flag);
		this.Progress.gameObject.SetActive(flag);
		if (flag)
		{
			int reachGifts = EachLevelGiftSystem.GetReachGifts();
			int totalGifts = EachLevelGiftSystem.GetTotalGifts();
			this.ProgressText.text = string.Concat(new object[]
			{
				Singleton<GlobalData>.Instance.GetText("PROGRESS"),
				" ",
				reachGifts,
				"/",
				totalGifts
			});
			Singleton<FontChanger>.Instance.SetFont(ProgressText);
		}
		else
		{
			//this.BuyPrice.text = "$ " + StoreDataManager.GetChargePoint(43).Price;
			this.BuyPrice.text = Singleton<InApps>.Instance.GetPrice(43);
			this.BuyDescribe.text = Singleton<GlobalData>.Instance.GetText("LEVEL_AWARD_BUY_TIP");
			Singleton<FontChanger>.Instance.SetFont(BuyPrice);
			Singleton<FontChanger>.Instance.SetFont(BuyDescribe);
		}
	}

	public void BuyGift()
	{
		Singleton<GlobalData>.Instance.DoCharge(43, delegate
		{
			//Product();
		});
	}

	public void Product()
	{
		ItemDataManager.SetCurrency(CommonDataType.EACH_LEVEL_AWARD_GIFT, 1);
		this.Refresh();
		if (PlayerPrefs.GetString("FirstPayShow", "true") == "true" && PlayerPrefs.GetString("FirstPay", "true") == "false")
		{
			PlayerPrefs.SetString("FirstPayShow", "false");
		// if (MirraSDK.Prefs.GetBool("FirstPayShow", true) && !MirraSDK.Prefs.GetBool("FirstPay", true))
		// {
		// 	MirraSDK.Prefs.SetBool("FirstPayShow", false);
			Singleton<UiManager>.Instance.ShowAward(new int[]
			{
				403
			}, new int[]
			{
				3
			}, Singleton<GlobalData>.Instance.GetText("FIRSTPAY"));
		}
	}

	public Transform ChildrenRoot;

	public EachLevelGiftItem ItemPrefab;

	public Text Title;

	public Button BuyButton;

	public Text BuyPrice;

	public Text BuyDescribe;

	public Transform Progress;

	public Text ProgressText;

	public List<EachLevelGiftItem> EachLevelGifts = new List<EachLevelGiftItem>();

	private List<EachLevelGiftData> gifts = new List<EachLevelGiftData>();
}
