using System;
using DataCenter;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EachLevelGiftItem : MonoBehaviour
{
	public void Refresh(EachLevelGiftData data)
	{
		this.gift = data;
		string text = this.SetName(data.RequireLevel);
		this.Name.text = text;
		this.AwardCount[0].text = data.AwardCount[0].ToString();
		this.AwardCount[1].text = data.AwardCount[1].ToString();
		this.ReceiveButton.gameObject.SetActive(data.State != 2);
		this.ReceiveButton.interactable = (data.State == 1);
		this.SelectImage.gameObject.SetActive(data.State == 1);
		this.ReceivedMask.gameObject.SetActive(data.State == 2);
		if (data.State == 0)
		{
			this.ButtonName.text = string.Concat(new string[]
			{
				Singleton<GlobalData>.Instance.GetText("COMPLETE"),
				" ",
				text,
				" ",
				Singleton<GlobalData>.Instance.GetText("RECEIVE")
			});
		}
		else if (data.State == 1)
		{
			this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVE");
		}
		else
		{
			this.ButtonName.text = Singleton<GlobalData>.Instance.GetText("RECEIVED");
		}
		Singleton<FontChanger>.Instance.SetFont(ButtonName);
		Singleton<FontChanger>.Instance.SetFont(AwardCount[0]);
		Singleton<FontChanger>.Instance.SetFont(AwardCount[1]);
	}

	public void ReceiveAward()
	{
		if (ItemDataManager.GetCurrency(CommonDataType.EACH_LEVEL_AWARD_GIFT) == 1)
		{
			EachLevelGiftSystem.SetState(this.gift, 2);
			this.Refresh(this.gift);
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.gift.AwardCount[0]);
			ItemDataManager.SetCurrency(CommonDataType.DNA, this.gift.AwardCount[1]);
			int[] id = new int[]
			{
				2,
				3
			};
			int[] count = new int[]
			{
				this.gift.AwardCount[0],
				this.gift.AwardCount[1]
			};
			Singleton<UiManager>.Instance.ShowAward(id, count, null);
		}
		else
		{
			string text = Singleton<GlobalData>.Instance.GetText("LEVEL_AWARD_BUY_DESCRIBE");
			//string name = "$ " + StoreDataManager.GetChargePoint(43).Price.ToString();
			string name = Singleton<InApps>.Instance.GetPrice(43);
			Singleton<UiManager>.Instance.ShowRemind(text, name, delegate
			{
				Singleton<GlobalData>.Instance.DoCharge(43, delegate
				{
					ItemDataManager.SetCurrency(CommonDataType.EACH_LEVEL_AWARD_GIFT, 1);
					Singleton<UiManager>.Instance.CurrentPage.Refresh();
					if (PlayerPrefs.GetString("FirstPayShow", "true") == "true" && PlayerPrefs.GetString("FirstPay", "true") == "false")
					{
						PlayerPrefs.SetString("FirstPayShow", "false");
						Singleton<UiManager>.Instance.ShowAward(new int[]
						{
							403
						}, new int[]
						{
							3
						}, Singleton<GlobalData>.Instance.GetText("FIRSTPAY"));
					}
				});
			}, null);
		}
	}

	private Sprite GetSprite(int id)
	{
		switch (id)
		{
		case 1:
			return Singleton<UiManager>.Instance.SpecialIcons[0];
		case 2:
			return Singleton<UiManager>.Instance.SpecialIcons[1];
		case 3:
			return Singleton<UiManager>.Instance.SpecialIcons[2];
		default:
			return Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(id).Icon);
		}
	}

	private string SetName(int key)
	{
		return Singleton<GlobalData>.Instance.GetText("LEVEL_TAG").Replace("#value#", key.ToString());
	}

	public Text Name;

	public Image SelectImage;

	public Image ReceivedMask;

	public Image[] AwardIcon;

	public Text[] AwardCount;

	public Button ReceiveButton;

	public Text ButtonName;

	private EachLevelGiftData gift;
}
