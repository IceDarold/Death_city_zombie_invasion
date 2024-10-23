using System;
using DataCenter;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public void Init()
	{
		this.NameTxt.text = Singleton<GlobalData>.Instance.GetText(this.Data.Name);
		this.DesShowTxt.text = Singleton<GlobalData>.Instance.GetText("LOOK_GOODS");
		this.ShowBtn.SetActive(true);
		this.DesTxt.gameObject.SetActive(true);
		this.DesTxt.text = Singleton<GlobalData>.Instance.GetText(this.Data.Describe).Replace("\\n", "\n");
		this.DiscountTxt.text = "-" + this.Data.Discount.ToString() + "%";
		this.DiscountGo.SetActive(this.Data.Discount != 0);
		this.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(this.Data.Icon);
		this.Icon.SetNativeSize();
		Singleton<FontChanger>.Instance.SetFont(NameTxt);
		Singleton<FontChanger>.Instance.SetFont(DesShowTxt);
		Singleton<FontChanger>.Instance.SetFont(DesTxt);
		Singleton<FontChanger>.Instance.SetFont(DiscountTxt);
		int type = this.Data.Type;
		if (type != 1)
		{
			if (type != 2)
			{
				if (type == 3)
				{
					Singleton<FontChanger>.Instance.SetFont(AddNumTxt);
					this.ShowBtn.SetActive(false);
					if (this.Data.Tag == 2)
					{
						this.IconType.sprite = Singleton<UiManager>.Instance.CommonSprites[1];
						if (ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_DIAMOND) > 0)
						{
							this.DesTxt.gameObject.SetActive(true);
							this.AddNumTxt.text = "+30%" + Singleton<GlobalData>.Instance.GetText("ADDITIONAL_DIAMOND");
							this.AddNumTxt.gameObject.SetActive(true);
						}
						else
						{
							this.ShowBtn.SetActive(false);
							this.DesTxt.gameObject.SetActive(false);
							this.AddNumTxt.gameObject.SetActive(false);
						}
					}
					else if (this.Data.Tag == 3)
					{
						this.IconType.sprite = Singleton<UiManager>.Instance.CommonSprites[5];
						if (ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_DNA) > 0)
						{
							this.DesTxt.gameObject.SetActive(true);
							this.AddNumTxt.text = "+30%" + Singleton<GlobalData>.Instance.GetText("ADDITIONAL_DNA");
							this.AddNumTxt.gameObject.SetActive(true);
						}
						else
						{
							this.ShowBtn.SetActive(false);
							this.DesTxt.gameObject.SetActive(false);
							this.AddNumTxt.gameObject.SetActive(false);
						}
					}
					else if (this.Data.Tag == 1)
					{
						this.IconType.sprite = Singleton<UiManager>.Instance.CommonSprites[0];
						if (ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_GOLD) > 0)
						{
							this.DesTxt.gameObject.SetActive(true);
							this.AddNumTxt.text = "+30%" + Singleton<GlobalData>.Instance.GetText("ADDITIONAL_GOLD");
							this.AddNumTxt.gameObject.SetActive(true);
						}
						else
						{
							this.DesTxt.gameObject.SetActive(false);
							this.AddNumTxt.gameObject.SetActive(false);
						}
					}
					this.IconType.sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(this.Data.ItemID[0]).Icon);
					this.NumCountTxt.text = this.Data.ItemCount[0].ToString();
					this.NumGo.SetActive(true);
					this.TimeGift.SetActive(false);
					this.NumGift.SetActive(false);
					this.BtnTimeTxt.gameObject.SetActive(false);
					// this.BtnTxt.text = "$" + StoreDataManager.GetChargePoint(this.Data.ChargePoint).Price;
					this.BtnTxt.text = Singleton<InApps>.Instance.GetPrice(Data.ChargePoint);
				}
			}
			else
			{
				this.NumGo.SetActive(false);
				if (this.Data.Tag == 1)
				{
					this.NumGift.SetActive(false);
					this.TimeGift.SetActive(true);
					if (this.Data.Count > this.Data.LimitCount)
					{
						this.BuyBtn.interactable = false;
						this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[1];
						this.BtnTxt.color = this.TxtColors[1];
						this.BtnTimeTxt.gameObject.SetActive(false);
						this.BtnTxt.gameObject.SetActive(true);
						this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("ALREADY_BUY");
					}
					else
					{
						this.BuyBtn.interactable = true;
						this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[0];
						this.BtnTxt.color = this.TxtColors[0];
						this.BtnTimeTxt.gameObject.SetActive(false);
						this.BtnTxt.gameObject.SetActive(true);
						// this.BtnTxt.text = "$" + StoreDataManager.GetChargePoint(this.Data.ChargePoint).Price;
						this.BtnTxt.text = Singleton<InApps>.Instance.GetPrice(Data.ChargePoint);
					}
				}
				else if (this.Data.Tag == 2)
				{
					this.TimeGift.SetActive(false);
					this.NumGift.SetActive(true);
					if (this.Data.LimitCount > this.Data.Count)
					{
						this.NumDesTxt.gameObject.SetActive(true);
						this.NumTxt.gameObject.SetActive(true);
						this.NumTxt.text = (this.Data.LimitCount - this.Data.Count).ToString();
						this.NumDesTxt.text = Singleton<GlobalData>.Instance.GetText("NUMDESGIFT");
						this.BtnTimeTxt.gameObject.SetActive(false);
						this.BtnTxt.gameObject.SetActive(true);
						// this.BtnTxt.text = "$" + StoreDataManager.GetChargePoint(this.Data.ChargePoint).Price;
						this.BtnTxt.text = Singleton<InApps>.Instance.GetPrice(Data.ChargePoint);
						this.BuyBtn.interactable = true;
						this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[0];
						this.BtnTxt.color = this.TxtColors[0];
					}
					else
					{
						this.NumDesTxt.gameObject.SetActive(false);
						this.NumTxt.gameObject.SetActive(false);
						this.BtnTimeTxt.gameObject.SetActive(true);
						this.BuyBtn.interactable = false;
						this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[1];
						this.BtnTxt.color = this.TxtColors[1];
						this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("NEXT_REFRESH");
					}
				}
				else
				{
					this.NumGift.SetActive(false);
					this.TimeGift.SetActive(false);
					if (this.Data.Purchased)
					{
						base.gameObject.SetActive(false);
					}
					else
					{
						this.BtnTimeTxt.gameObject.SetActive(false);
						this.BtnTxt.gameObject.SetActive(true);
						// this.BtnTxt.text = "$" + StoreDataManager.GetChargePoint(this.Data.ChargePoint).Price;
						this.BtnTxt.text = Singleton<InApps>.Instance.GetPrice(Data.ChargePoint);
					}
				}
			}
		}
		else
		{
			this.TimeGift.SetActive(false);
			this.NumGift.SetActive(false);
			this.NumGo.SetActive(false);
			if (this.Data.Purchased)
			{
				this.BuyBtn.interactable = false;
				this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[1];
				this.BtnTxt.color = this.TxtColors[1];
				this.BtnTimeTxt.gameObject.SetActive(false);
				this.BtnTxt.gameObject.SetActive(true);
				this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("ALREADY_BUY");
			}
			else
			{
				this.BuyBtn.interactable = true;
				this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[0];
				this.BtnTxt.color = this.TxtColors[0];
				this.BtnTimeTxt.gameObject.SetActive(false);
				this.BtnTxt.gameObject.SetActive(true);
				// this.BtnTxt.text = "$" + StoreDataManager.GetChargePoint(this.Data.ChargePoint).Price;
				this.BtnTxt.text = Singleton<InApps>.Instance.GetPrice(Data.ChargePoint);
			}
		}
		Singleton<FontChanger>.Instance.SetFont(BtnTxt);
	}

	public void Update()
	{
		if (this.Data.Type == 3 && UITick.getADDITIONALSec(this.Data.Tag - 1).Length != 0)
		{
			this.DesTxt.gameObject.SetActive(true);
			this.AddNumTxt.gameObject.SetActive(true);
		}
	}

	public void OnclickShow()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.ShopItemPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.ShopItemPage).GetComponent<ShopItemPage>().sd = this.Data;
		});
	}

	public void OnclickBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<GlobalData>.Instance.DoCharge(this.Data.ChargePoint, delegate
		{
			this.OnclickBuySucess();
		});
	}

	public void OnclickBuySucess()
	{
		StoreDataManager.Buy(this.Data.ID);
		int[] itemID = this.Data.ItemID;
		int[] array = (int[])this.Data.ItemCount.Clone();
		int type = this.Data.Type;
		if (type != 1)
		{
			if (type != 2)
			{
				if (type == 3)
				{
					int num = (int)((float)this.Data.ItemCount[0] * 0.3f);
					if (this.Data.Tag == 1 && ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_GOLD) > 0)
					{
						ItemDataManager.SetCurrency(CommonDataType.GOLD, num);
						ItemDataManager.SetCurrency(CommonDataType.ADDITIONAL_GOLD, -1);
						array[0] += num;
						UITick.setADDITIONALSec(0, 1);
					}
					else if (this.Data.Tag == 2 && ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_DIAMOND) > 0)
					{
						ItemDataManager.SetCurrency(CommonDataType.DIAMOND, num);
						ItemDataManager.SetCurrency(CommonDataType.ADDITIONAL_DIAMOND, -1);
						array[0] += num;
						UITick.setADDITIONALSec(1, 1);
					}
					else if (this.Data.Tag == 3 && ItemDataManager.GetCurrency(CommonDataType.ADDITIONAL_DNA) > 0)
					{
						ItemDataManager.SetCurrency(CommonDataType.DNA, num);
						ItemDataManager.SetCurrency(CommonDataType.ADDITIONAL_DNA, -1);
						array[0] += num;
						UITick.setADDITIONALSec(2, 1);
					}
				}
			}
		}
		else
		{
			this.BuyBtn.interactable = false;
			this.BuyBtn.GetComponent<Image>().sprite = this.BtnSprites[1];
			this.BtnTxt.color = this.TxtColors[1];
			this.BtnTimeTxt.gameObject.SetActive(false);
			this.BtnTxt.gameObject.SetActive(true);
			this.BtnTxt.text = Singleton<GlobalData>.Instance.GetText("ALREADY_BUY");
			if (this.Data.Tag == 1)
			{
				UITick.SetVipTime();
			}
		}
		Singleton<UiManager>.Instance.TopBar.Refresh();
		Singleton<UiManager>.Instance.ShowAward(itemID, array, null);
	}

	public Sprite[] BtnSprites;

	public Color[] TxtColors;

	public Text NameTxt;

	public UILabelTick TimeGiftTxt;

	public UILabelTick BtnTimeTxt;

	public Text DesTxt;

	public Text NumDesTxt;

	public Text NumTxt;

	public Text BtnTxt;

	public Text NumCountTxt;

	public Text AddNumTxt;

	public Text DiscountTxt;

	public Image IconType;

	public Image Icon;

	public Button BuyBtn;

	public GameObject TimeGift;

	public GameObject NumGift;

	public GameObject NumGo;

	public GameObject DiscountGo;

	public GameObject ShowBtn;

	public StoreData Data;

	public StorePage curStorePage;

	public Text DesShowTxt;
}
