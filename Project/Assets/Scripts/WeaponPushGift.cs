using System;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPushGift : GamePage
{
	public override void Show()
	{
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		this.WeaponIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.Weapon.Icon);
		this.TitleName.text = Singleton<GlobalData>.Instance.GetText("ONLY_ONE_CHANCE");
		this.Describe.text = Singleton<GlobalData>.Instance.GetText(this.Weapon.Describe).Replace("\\n", "\n");
		this.DiscountText.text = Singleton<GlobalData>.Instance.GetText("PRICE") + " -20%";
		Singleton<FontChanger>.Instance.SetFont(TitleName);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(DiscountText);
		ItemUnlockType unlockType = this.Weapon.UnlockType;
		if (unlockType != ItemUnlockType.GOLD)
		{
			if (unlockType != ItemUnlockType.DIAMOND)
			{
				if (unlockType == ItemUnlockType.RMB)
				{
					this.OriginalPriceIcon.gameObject.SetActive(false);
					// this.OriginalPrice.text = "$" + StoreDataManager.GetChargePoint(this.Weapon.UnlockPrice).Price;
					this.OriginalPrice.text = Singleton<InApps>.Instance.GetPrice(Weapon.UnlockPrice);
					this.DiamondCount = (int)(StoreDataManager.GetChargePoint(this.Weapon.UnlockPrice).Price * 100f * 0.8f);
					this.CurrentPrice.text = this.DiamondCount.ToString();
				}
			}
			else
			{
				this.OriginalPriceIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetCommonItem(CommonDataType.DIAMOND).Icon);
				this.OriginalPriceIcon.gameObject.SetActive(true);
				this.OriginalPrice.text = this.Weapon.UnlockPrice.ToString();
				this.DiamondCount = (int)((float)this.Weapon.UnlockPrice * 0.8f);
				this.CurrentPrice.text = this.DiamondCount.ToString();
			}
		}
		else
		{
			this.OriginalPriceIcon.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetCommonItem(CommonDataType.GOLD).Icon);
			this.OriginalPriceIcon.gameObject.SetActive(true);
			this.OriginalPrice.text = this.Weapon.UnlockPrice.ToString();
			this.DiamondCount = (int)((float)this.Weapon.UnlockPrice / 30f * 0.8f);
			this.CurrentPrice.text = this.DiamondCount.ToString();
		}
	}

	public void ClickOnClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
	}

	public void ClickOnBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= this.DiamondCount)
		{
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, this.DiamondCount);
			WeaponDataManager.Unlock(this.Weapon.ID);
			this.Close();
		}
		else
		{
			this.Hide();
			Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, this.DiamondCount);
		}
	}

	public CanvasGroup Content;

	public Image WeaponIcon;

	public Text TitleName;

	public Text Describe;

	public Text DiscountText;

	public Text OriginalPrice;

	public Image OriginalPriceIcon;

	public Text CurrentPrice;

	[HideInInspector]
	public WeaponData Weapon = new WeaponData();

	private int DiamondCount;
}
