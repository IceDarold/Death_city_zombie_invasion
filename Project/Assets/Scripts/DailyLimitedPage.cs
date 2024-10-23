using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class DailyLimitedPage : GamePage
{
	public override void Show()
	{
		this.CountLimitGifts = StoreDataManager.GetCountLimitGifts();
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		this.TitleText.text = Singleton<GlobalData>.Instance.GetText("DAILY_LIMITED_TITLE");
		this.TipText.text = Singleton<GlobalData>.Instance.GetText("DAILY_LIMITED_TIP");
		Singleton<FontChanger>.Instance.SetFont(TitleText);
		Singleton<FontChanger>.Instance.SetFont(TipText);
		for (int i = 0; i < this.GiftObjects.Count; i++)
		{
			if (i < this.CountLimitGifts.Count)
			{
				this.GiftObjects[i].gameObject.SetActive(true);
				this.GiftObjects[i].Refresh(this.CountLimitGifts[i]);
			}
			else
			{
				this.GiftObjects[i].gameObject.SetActive(false);
			}
		}
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	public void ClickOnBuy(int index)
	{
		StoreData data = this.CountLimitGifts[index];
		Singleton<GlobalData>.Instance.DoCharge(data.ChargePoint, delegate
		{
			//Product(index);
		});
	}

	public void Product(int index)
	{
		var data = CountLimitGifts[index];
		StoreDataManager.Buy(data.ID);
		this.Refresh();
		Singleton<UiManager>.Instance.ShowAward(data.ItemID, data.ItemCount, Singleton<GlobalData>.Instance.GetText(data.Name));
	}

	public CanvasGroup Content;

	public Text TitleText;

	public Text TipText;

	public List<DailyLimitedChild> GiftObjects = new List<DailyLimitedChild>();

	private List<StoreData> CountLimitGifts = new List<StoreData>();
}
