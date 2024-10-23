using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanel : MonoBehaviour
{
	private void Awake()
	{
		this.EquipmentDatas = EquipmentDataManager.Equipments;
		this.EquipmentRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(this.EquipmentDatas.Count * 195), 140f);
		this.RootWidth = (float)(this.EquipmentDatas.Count * 195);
		this.pos01 = this.CostRoot.transform.localPosition;
		this.pos02 = this.CenterTimeAndCost.localPosition;
		for (int i = 0; i < this.EquipmentDatas.Count; i++)
		{
			int index = i;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/EquipmentChild")) as GameObject;
			gameObject.transform.SetParent(this.EquipmentRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectEquipment(index);
			});
			EquipmentChild component = gameObject.GetComponent<EquipmentChild>();
			this.EquipmentChildren.Add(component);
		}
	}

	private void OnEnable()
	{
		this.CloseEffect();
		this.SelectIndex = 0;
		this.ChooseIndex = 1;
		this.SelectImage.position = this.EquipmentChildren[this.SelectIndex].transform.position;
		this.SelectImage.gameObject.SetActive(false);
		this.RefreshChildren();
		this.RefreshInfo();
		this.RefreshPlayerEquipment();
		this.InfoLayer.localPosition = new Vector3(800f, 40f, 0f);
		this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.EquipmentLayer.localPosition = new Vector3(0f, -450f, 0f);
		this.EquipmentLayer.DOLocalMove(this.DownAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo).OnComplete(delegate
		{
			if (Singleton<GlobalData>.Instance.FirstEquipMent > 0)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.BootomPos.gameObject.SetActive(false);
				this.curTeachPage.type = TeachUIType.Equipment;
				this.curTeachPage.RefreshPage();
				curTeachPage.Close();
				int firstEquipMent = Singleton<GlobalData>.Instance.FirstEquipMent;
				if (firstEquipMent != 1)
				{
					if (firstEquipMent != 2)
					{
						if (firstEquipMent == 3)
						{
							this.HeadButtonGO.SetActive(false);
							this.SelectIndex = 0;
							this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.EquipButton.gameObject);
							this.curTeachPage.BootomPos.gameObject.SetActive(true);
							this.curTeachPage.Button.SetActive(false);
							this.curTeachPage.EffectObj.gameObject.SetActive(false);
							this.curTeachPage.BootomPos.position = this.EquipmentChildren[this.SelectIndex].transform.position;
							this.curTeachPage.curEquipInfo.init(this.EquipmentDatas[this.SelectIndex], true);
							this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
							this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curEquipInfo.transform.position;
							this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
							this.curTeachPage.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
							this.HeadButtonGO.SetActive(true);
						}
					}
					else
					{
						this.SelectIndex = 1;
						this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.EquipButton.gameObject);
						this.curTeachPage.BootomPos.gameObject.SetActive(true);
						this.curTeachPage.Button.SetActive(false);
						this.curTeachPage.EffectObj.gameObject.SetActive(false);
						this.curTeachPage.BootomPos.position = this.EquipmentChildren[this.SelectIndex].transform.position;
						this.curTeachPage.curEquipInfo.init(this.EquipmentDatas[this.SelectIndex], true);
						this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
						this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curEquipInfo.transform.position;
						this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
						this.curTeachPage.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
					}
				}
				else
				{
					this.SelectIndex = 2;
					this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.EquipButton.gameObject);
					this.curTeachPage.BootomPos.gameObject.SetActive(true);
					this.curTeachPage.Button.SetActive(false);
					this.curTeachPage.EffectObj.gameObject.SetActive(false);
					this.curTeachPage.BootomPos.position = this.EquipmentChildren[this.SelectIndex].transform.position;
					this.curTeachPage.curEquipInfo.init(this.EquipmentDatas[this.SelectIndex], true);
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
					this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curEquipInfo.transform.position;
					this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
					this.curTeachPage.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
				}
				Singleton<FontChanger>.Instance.SetFont(curTeachPage.Guide02Txt);
				Singleton<FontChanger>.Instance.SetFont(curTeachPage.GuideTxt);
				this.RefreshChildren();
				this.RefreshInfo();
			}
			else
			{
				this.curTeachPage = null;
			}
		});
	}

	public void OnclickBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		EquipmentData data = this.EquipmentDatas[this.SelectIndex];
		switch (data.UnlockType)
		{
		case ItemUnlockType.RMB:
			Singleton<GlobalData>.Instance.DoCharge(data.UnlockPrice, delegate
			{
				EquipmentDataManager.Collect(data.ID);
			});
			break;
		case ItemUnlockType.GOLD:
			this.CostByType(CommonDataType.GOLD, data);
			break;
		case ItemUnlockType.DIAMOND:
			this.CostByType(CommonDataType.DIAMOND, data);
			break;
		case ItemUnlockType.DNA:
			this.CostByType(CommonDataType.DNA, data);
			break;
		}
		Singleton<UiManager>.Instance.TopBar.Refresh();
		this.RefreshInfo();
		this.RefreshChildren();
	}

	public void CostByType(CommonDataType type, EquipmentData data)
	{
		if (ItemDataManager.GetCurrency(type) >= data.UnlockPrice)
		{
			ItemDataManager.SetCurrency(type, -data.UnlockPrice);
			EquipmentDataManager.Collect(data.ID);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(type, data.UnlockPrice - ItemDataManager.GetCurrency(type));
		}
	}

	private void RefreshChildren()
	{
		this.count = 0;
		for (int i = 0; i < this.EquipmentDatas.Count; i++)
		{
			this.EquipmentChildren[i].init(this.EquipmentDatas[i], this.SelectIndex == i);
			if (this.EquipmentDatas[i].isNew)
			{
				this.count++;
			}
		}
		FunctionPage.Instance.FunctionButtons[1].Tip.gameObject.SetActive(this.count > 0);
	}

	private void RefreshSelectChild()
	{
		this.EquipmentChildren[this.SelectIndex].init(this.EquipmentDatas[this.SelectIndex], true);
	}

	public void RefreshInfo()
	{
		this.ChooseImage.transform.SetParent(this.ChooseGo[this.ChooseIndex - 1].transform);
		this.ChooseImage.transform.localPosition = Vector3.zero;
		this.ChooseImage.transform.SetAsFirstSibling();
		EquipmentData equipmentData = this.EquipmentDatas[this.SelectIndex];
		EquipmentSetData setData = EquipmentDataManager.GetSetData(equipmentData.SetID);
		this.ShuoMingTxt.text = Singleton<GlobalData>.Instance.GetText("ChOOSE_POS");
		this.EquipmentName.text = Singleton<GlobalData>.Instance.GetText(equipmentData.Name);
		this.ShowAllTxt.text = Singleton<GlobalData>.Instance.GetText("SHOWALL");
		Singleton<FontChanger>.Instance.SetFont(ShuoMingTxt);
		Singleton<FontChanger>.Instance.SetFont(EquipmentName);
		Singleton<FontChanger>.Instance.SetFont(ShowAllTxt);
		if (equipmentData.Part == 1)
		{
			this.EquipmentPart.text = Singleton<GlobalData>.Instance.GetText("HEAD");
		}
		else if (equipmentData.Part == 2)
		{
			this.EquipmentPart.text = Singleton<GlobalData>.Instance.GetText("CHEST");
		}
		else if (equipmentData.Part == 3)
		{
			this.EquipmentPart.text = Singleton<GlobalData>.Instance.GetText("FOOT");
		}
		Singleton<FontChanger>.Instance.SetFont(EquipmentPart);
		this.EquipmentIcon.sprite = Singleton<UiManager>.Instance.GetSprite(equipmentData.Icon);
		this.EquipmentIcon.SetNativeSize();
		this.EquipmentPower.text = string.Concat(new object[]
		{
			Singleton<GlobalData>.Instance.GetText("TENACITY"),
			" ",
			10 * EquipmentDataManager.GetFighting(equipmentData.ID, false),
			"/",
			10 * EquipmentDataManager.GetFighting(equipmentData.ID, true)
		});
		Singleton<FontChanger>.Instance.SetFont(EquipmentPower);
		this.BasicAttributeTitle.text = Singleton<GlobalData>.Instance.GetText("EQUIP_PROPERTY");
		this.BasicAttributeValue.text = Singleton<GlobalData>.Instance.GetText(equipmentData.AttributeType.ToString()) + " +" + equipmentData.AttributeValue[equipmentData.Level];
		this.SetAttributeTitle.text = Singleton<GlobalData>.Instance.GetText("BONUS_STATS");
		Singleton<FontChanger>.Instance.SetFont(BasicAttributeTitle);
		Singleton<FontChanger>.Instance.SetFont(BasicAttributeValue);
		Singleton<FontChanger>.Instance.SetFont(SetAttributeTitle);
		Singleton<FontChanger>.Instance.SetFont(SetAttributeValue);
		if (setData.Type == EquipmentAttribute.HEALTH)
		{
			this.SetAttributeValue.text = Singleton<GlobalData>.Instance.GetText(setData.Type.ToString()) + " +" + setData.Value;
		}
		else
		{
			this.SetAttributeValue.text = string.Concat(new object[]
			{
				Singleton<GlobalData>.Instance.GetText(setData.Type.ToString()),
				" +",
				setData.Value * 100f,
				"%"
			});
		}
		if (EquipmentDataManager.isSetsActivated(equipmentData.SetID) == 3)
		{
			this.SetAttributeValue.color = Color.white;
		}
		else
		{
			this.SetAttributeValue.color = Color.red;
		}
		this.SetTitle.text = Singleton<GlobalData>.Instance.GetText(setData.Name) + "(" + EquipmentDataManager.isSetsActivated(equipmentData.SetID).ToString() + "/3)";
		Singleton<FontChanger>.Instance.SetFont(SetTitle);
		List<EquipmentData> sets = EquipmentDataManager.GetSets(equipmentData.SetID);
		this.RefreshSetsInfo(ref this.SetCapName, sets[0]);
		this.RefreshSetsInfo(ref this.SetCoatName, sets[1]);
		this.RefreshSetsInfo(ref this.SetShoesName, sets[2]);
		this.SetButtonState(equipmentData);
	}

	private void RefreshPlayerEquipment()
	{
		if (PlayerDataManager.Player.Cap != 0)
		{
			ItemData itemData = ItemDataManager.GetItemData(PlayerDataManager.Player.Cap);
			this.PlayerHeadIcon.sprite = Singleton<UiManager>.Instance.GetSprite(itemData.Icon);
		}
		else
		{
			this.PlayerHeadIcon.sprite = this.AddSprite;
		}
		if (PlayerDataManager.Player.Coat != 0)
		{
			ItemData itemData2 = ItemDataManager.GetItemData(PlayerDataManager.Player.Coat);
			this.PlayerBagIcon.sprite = Singleton<UiManager>.Instance.GetSprite(itemData2.Icon);
		}
		else
		{
			this.PlayerBagIcon.sprite = this.AddSprite;
		}
		if (PlayerDataManager.Player.Shoes != 0)
		{
			ItemData itemData3 = ItemDataManager.GetItemData(PlayerDataManager.Player.Shoes);
			this.PlayerHandsIcon.sprite = Singleton<UiManager>.Instance.GetSprite(itemData3.Icon);
		}
		else
		{
			this.PlayerHandsIcon.sprite = this.AddSprite;
		}
		this.PlayerHeadPart.text = Singleton<GlobalData>.Instance.GetText("HEAD");
		this.PlayerBagPart.text = Singleton<GlobalData>.Instance.GetText("CHEST");
		this.PlayerHandsPart.text = Singleton<GlobalData>.Instance.GetText("FOOT");
		Singleton<FontChanger>.Instance.SetFont(PlayerHeadPart);
		Singleton<FontChanger>.Instance.SetFont(PlayerBagPart);
		Singleton<FontChanger>.Instance.SetFont(PlayerHandsPart);
	}

	private void RefreshSetsInfo(ref Text txt, EquipmentData data)
	{
		string text = Singleton<GlobalData>.Instance.GetText(data.Name);
		Singleton<FontChanger>.Instance.SetFont(txt);
		if (data.State >= EquipmentState.待升级 && EquipmentDataManager.isEquip(data.ID))
		{
			txt.text = text + "  " + Singleton<GlobalData>.Instance.GetText("HAVEEQUIPMENT");
			txt.color = Color.yellow;
		}
		else if (data.State >= EquipmentState.待升级 && !EquipmentDataManager.isEquip(data.ID))
		{
			txt.text = text + "  " + Singleton<GlobalData>.Instance.GetText("HAVEGOT");
			txt.color = Color.white;
		}
		else
		{
			txt.text = text + "  " + Singleton<GlobalData>.Instance.GetText("NOHAVE");
			txt.color = Color.black;
		}
	}

	private void SetButtonState(EquipmentData data)
	{
		Singleton<FontChanger>.Instance.SetFont(EquipButtonName);
		Singleton<FontChanger>.Instance.SetFont(UpgradeButtonName);
		Singleton<FontChanger>.Instance.SetFont(CostValue);
		switch (data.State)
		{
		case EquipmentState.未解锁:
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("GAIN");
			this.EquipButton.interactable = true;
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.UpgradeButton.gameObject.SetActive(false);
			this.Buybtn.SetActive(true);
			this.BuyBtnName.text = Singleton<GlobalData>.Instance.GetText("BUY");
			Singleton<FontChanger>.Instance.SetFont(BuyBtnName);
			switch (data.UnlockType)
			{
			case ItemUnlockType.NONE:
				this.Buybtn.SetActive(false);
				break;
			case ItemUnlockType.RMB:
				this.CostTypeImg.sprite = Singleton<UiManager>.Instance.CommonSprites[10];
				this.CostNumTxt.text = StoreDataManager.GetChargePoint(data.UnlockPrice).ToString();
				break;
			case ItemUnlockType.GOLD:
				this.CostTypeImg.sprite = Singleton<UiManager>.Instance.CommonSprites[2];
				this.CostNumTxt.text = data.UnlockPrice.ToString();
				break;
			case ItemUnlockType.DIAMOND:
				this.CostTypeImg.sprite = Singleton<UiManager>.Instance.CommonSprites[1];
				this.CostNumTxt.text = data.UnlockPrice.ToString();
				break;
			case ItemUnlockType.DNA:
				this.CostTypeImg.sprite = Singleton<UiManager>.Instance.CommonSprites[5];
				this.CostNumTxt.text = data.UnlockPrice.ToString();
				break;
			}
			this.CostRoot.SetActive(false);
			this.TimeRoot.SetActive(false);
			break;
		case EquipmentState.待制作:
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("PRODUCE");
			this.EquipButton.interactable = true;
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.Buybtn.SetActive(false);
			this.UpgradeButton.gameObject.SetActive(false);
			this.CostRoot.transform.localPosition = this.pos02;
			this.CostRoot.SetActive(true);
			this.CostValue.text = data.RequiredPrice[0].ToString();
			this.TimeRoot.SetActive(false);
			break;
		case EquipmentState.制作中:
			this.CountDown = (float)UITick.getItemRemainderSec(data.ID, data.RequiredTime[0]);
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
			this.EquipButton.interactable = true;
			this.Buybtn.SetActive(false);
			this.EquipButtonCost.gameObject.SetActive(true);
			this.EquipButton.gameObject.SetActive(true);
			this.UpgradeButton.gameObject.SetActive(false);
			this.TimeRoot.transform.localPosition = this.pos02;
			this.CostRoot.SetActive(false);
			this.TimeRoot.SetActive(true);
			break;
		case EquipmentState.待领取:
			data.State = EquipmentState.待升级;
			this.EquipButtonName.text = ((!EquipmentDataManager.isEquip(data.ID)) ? Singleton<GlobalData>.Instance.GetText("EQUIP") : Singleton<GlobalData>.Instance.GetText("HAVEEQUIP"));
			this.EquipButton.interactable = !EquipmentDataManager.isEquip(data.ID);
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.Buybtn.SetActive(false);
			this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
			this.UpgradeButton.interactable = true;
			this.UpgradeButton.gameObject.SetActive(true);
			this.UpgradeButtonCost.gameObject.SetActive(false);
			this.CostRoot.transform.localPosition = this.pos01;
			this.CostRoot.SetActive(true);
			this.CostValue.text = data.RequiredPrice[data.Level].ToString();
			this.TimeRoot.SetActive(false);
			break;
		case EquipmentState.待升级:
			this.EquipButtonName.text = ((!EquipmentDataManager.isEquip(data.ID)) ? Singleton<GlobalData>.Instance.GetText("EQUIP") : Singleton<GlobalData>.Instance.GetText("HAVEEQUIP"));
			this.EquipButton.interactable = !EquipmentDataManager.isEquip(data.ID);
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.Buybtn.SetActive(false);
			this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
			this.UpgradeButton.interactable = true;
			this.UpgradeButton.gameObject.SetActive(true);
			this.UpgradeButtonCost.gameObject.SetActive(false);
			this.CostRoot.transform.localPosition = this.pos01;
			this.CostRoot.SetActive(true);
			this.CostValue.text = data.RequiredPrice[data.Level].ToString();
			this.TimeRoot.SetActive(false);
			break;
		case EquipmentState.升级中:
			this.EquipButtonName.text = ((!EquipmentDataManager.isEquip(data.ID)) ? Singleton<GlobalData>.Instance.GetText("EQUIP") : Singleton<GlobalData>.Instance.GetText("HAVEEQUIP"));
			this.EquipButton.interactable = !EquipmentDataManager.isEquip(data.ID);
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.Buybtn.SetActive(false);
			this.CountDown = (float)UITick.getItemRemainderSec(data.ID, data.RequiredTime[data.Level]);
			this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
			this.UpgradeButton.interactable = true;
			this.UpgradeButton.gameObject.SetActive(true);
			this.UpgradeButtonCost.gameObject.SetActive(true);
			this.TimeRoot.transform.localPosition = this.pos01;
			this.CostRoot.SetActive(false);
			this.TimeRoot.SetActive(true);
			break;
		case EquipmentState.已满级:
			this.EquipButtonName.text = ((!EquipmentDataManager.isEquip(data.ID)) ? Singleton<GlobalData>.Instance.GetText("EQUIP") : Singleton<GlobalData>.Instance.GetText("HAVEEQUIP"));
			this.EquipButton.interactable = !EquipmentDataManager.isEquip(data.ID);
			this.EquipButtonCost.gameObject.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.Buybtn.SetActive(false);
			this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("LEVELMAX");
			this.UpgradeButton.interactable = false;
			this.UpgradeButton.gameObject.SetActive(true);
			this.UpgradeButtonCost.gameObject.SetActive(false);
			this.CostRoot.SetActive(false);
			this.TimeRoot.SetActive(false);
			break;
		}
	}

	public void SelectEquipment(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (index == this.SelectIndex || this.IsAction)
		{
			return;
		}
		this.IsAction = true;
		this.SelectImage.position = this.EquipmentChildren[this.SelectIndex].transform.position;
		this.EquipmentChildren[this.SelectIndex].SelectImage.gameObject.SetActive(false);
		this.SelectImage.gameObject.SetActive(true);
		this.SelectIndex = index;
		this.SelectImage.DOMoveX(this.EquipmentChildren[index].transform.position.x, 0.3f, false).OnComplete(delegate
		{
			this.IsAction = false;
			this.SelectImage.gameObject.SetActive(false);
			EquipmentData equipmentData = this.EquipmentDatas[this.SelectIndex];
			if (equipmentData.isNew)
			{
				EquipmentDataManager.RemoveNewTag(equipmentData.ID);
				this.count--;
				FunctionPage.Instance.FunctionButtons[1].Tip.gameObject.SetActive(this.count > 0);
			}
			this.RefreshSelectChild();
			this.RefreshInfo();
		});
	}

	public void ClickOnPlayerEquipment(int part)
	{
		if (part == 0)
		{
			this.ChooseIndex = 1;
		}
		else
		{
			this.ChooseIndex = part;
		}
		int num = 0;
		for (int i = 0; i < this.EquipmentChildren.Count; i++)
		{
			if (part == 0)
			{
				num++;
				this.EquipmentChildren[i].gameObject.SetActive(true);
			}
			else if (this.EquipmentDatas[i].Part == part)
			{
				num++;
				this.EquipmentChildren[i].gameObject.SetActive(true);
			}
			else
			{
				this.EquipmentChildren[i].gameObject.SetActive(false);
			}
		}
		this.RefreshInfo();
		this.EquipmentRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(num * 195), 140f);
	}

	public void CloseEffect()
	{
		this.curEffect.SetActive(false);
	}

	public void SetEffectOnclick(Transform trans)
	{
		this.curEffect.transform.SetParent(trans);
		this.curEffect.transform.localPosition = Vector3.zero;
		this.curEffect.SetActive(true);
		this.effectAni.DORestart(false);
		this.effectAni.DORestartById("2");
	}

	public void ClickOnEquipButton()
	{
		EquipmentData data = this.EquipmentDatas[this.SelectIndex];
		switch (data.State)
		{
		case EquipmentState.未解锁:
			Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				if (Singleton<GlobalData>.Instance.isDebug)
				{
					EquipmentDataManager.Unlock(data.ID);
				}
				else
				{
					Singleton<UiManager>.Instance.ShowPage(PageName.LackOfMeterialPopup, delegate()
					{
						Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Clear();
						for (int i = 0; i < data.DropID.Length; i++)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Add(data.DropID[i]);
						}
					});
				}
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				if (Singleton<GlobalData>.Instance.isDebug)
				{
					EquipmentDataManager.Unlock(data.ID);
				}
				else
				{
					Singleton<UiManager>.Instance.ShowPage(PageName.LackOfMeterialPopup, delegate()
					{
						Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Clear();
						for (int i = 0; i < data.DropID.Length; i++)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Add(data.DropID[i]);
						}
					});
				}
			}
			break;
		case EquipmentState.待制作:
			Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
			if (this.CheckGold(data))
			{
				this.SetEffectOnclick(this.EquipButtonName.transform);
				UITick.setItemSec(data.ID, 1);
				EquipmentDataManager.StartFabricate(data.ID);
				GameLogManager.SendCostLog(1, data.RequiredTime[data.Level], data.ID, 1);
				GameLogManager.SendPageLog(data.Name, "Produce");
			}
			break;
		case EquipmentState.制作中:
			Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
			if (this.CheckDiamond(data))
			{
				this.CloseEffect();
				UITick.setItemSec(data.ID, 0);
				EquipmentDataManager.FinishFabricate(data.ID);
				EquipmentDataManager.Collect(data.ID);
			}
			break;
		case EquipmentState.待领取:
			Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
			EquipmentDataManager.Collect(data.ID);
			break;
		default:
		{
			Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.EquipMentEquipClip, false);
			if (data.Part == 1)
			{
				PlayerDataManager.Equip(EquipmentPosition.Cap, data.ID);
				if (null != this.curTeachPage)
				{
					Singleton<GlobalData>.Instance.FirstEquipMent = 2;
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
				}
			}
			else if (data.Part == 2)
			{
				PlayerDataManager.Equip(EquipmentPosition.Coat, data.ID);
				if (null != this.curTeachPage)
				{
					Singleton<GlobalData>.Instance.FirstEquipMent = 1;
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
				}
			}
			else if (data.Part == 3)
			{
				PlayerDataManager.Equip(EquipmentPosition.Shoes, data.ID);
				if (null != this.curTeachPage)
				{
					Singleton<GlobalData>.Instance.FirstEquipMent = 0;
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
					Singleton<UiManager>.Instance.SetTopEnable(true, true);
					this.curTeachPage.Close();
				}
			}
			int firstEquipMent = Singleton<GlobalData>.Instance.FirstEquipMent;
			if (firstEquipMent != 1)
			{
				if (firstEquipMent == 2)
				{
					this.SelectIndex = 1;
					this.curTeachPage.BootomPos.gameObject.SetActive(true);
					this.curTeachPage.Button.gameObject.SetActive(false);
					this.curTeachPage.EffectObj.gameObject.SetActive(false);
					this.curTeachPage.BootomPos.position = this.EquipmentChildren[this.SelectIndex].transform.position;
					this.curTeachPage.curEquipInfo.init(this.EquipmentDatas[this.SelectIndex], true);
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
					this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curEquipInfo.transform.position;
					this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
					this.curTeachPage.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
				}
			}
			else
			{
				this.SelectIndex = 2;
				this.curTeachPage.BootomPos.gameObject.SetActive(true);
				this.curTeachPage.Button.gameObject.SetActive(false);
				this.curTeachPage.EffectObj.gameObject.SetActive(false);
				this.curTeachPage.BootomPos.position = this.EquipmentChildren[this.SelectIndex].transform.position;
				this.curTeachPage.curEquipInfo.init(this.EquipmentDatas[this.SelectIndex], true);
				this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
				this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curEquipInfo.transform.position;
				this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
				this.curTeachPage.GuideTxt.text = Singleton<GlobalData>.Instance.GetText(string.Empty);
			}
			Singleton<FontChanger>.Instance.SetFont(curTeachPage.Guide02Txt);
			Singleton<FontChanger>.Instance.SetFont(curTeachPage.GuideTxt);
			this.RefreshPlayerEquipment();
			break;
		}
		}
		this.RefreshInfo();
		this.RefreshChildren();
	}

	public void ClickOnUpgradeButton()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		EquipmentData equipmentData = this.EquipmentDatas[this.SelectIndex];
		EquipmentState state = equipmentData.State;
		if (state != EquipmentState.待升级)
		{
			if (state != EquipmentState.升级中)
			{
				return;
			}
			if (this.CheckDiamond(equipmentData))
			{
				this.CloseEffect();
				UITick.setItemSec(equipmentData.ID, 0);
				EquipmentDataManager.FinishUpgrade(equipmentData.ID);
				Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.WeaponUpClip, false);
				FunctionPage.Instance.ShowUpgradeEffect();
			}
		}
		else if (this.CheckGold(equipmentData))
		{
			this.SetEffectOnclick(this.UpgradeButtonName.transform);
			UITick.setItemSec(equipmentData.ID, 1);
			EquipmentDataManager.StartUpgrade(equipmentData.ID);
			GameLogManager.SendCostLog(1, equipmentData.RequiredTime[equipmentData.Level], equipmentData.ID, 2);
			GameLogManager.SendPageLog(equipmentData.Name, "Upgrade");
		}
		this.RefreshInfo();
		this.RefreshChildren();
	}

	public bool CheckGold(EquipmentData data)
	{
		if (ItemDataManager.GetCurrency(CommonDataType.GOLD) >= data.RequiredPrice[data.Level])
		{
			ItemDataManager.SetCurrency(CommonDataType.GOLD, -data.RequiredPrice[data.Level]);
			Singleton<UiManager>.Instance.TopBar.Refresh();
			return true;
		}
		int num = data.RequiredPrice[data.Level] - ItemDataManager.GetCurrency(CommonDataType.GOLD);
		Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.GOLD, num);
		GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
		return false;
	}

	public bool CheckDiamond(EquipmentData data)
	{
		int skipDiamond = Singleton<GlobalData>.Instance.GetSkipDiamond(UITick.getItemRemainderSec(data.ID, data.RequiredTime[data.Level]));
		if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= skipDiamond)
		{
			GameLogManager.SendCostLog(2, skipDiamond, data.ID, 3);
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -skipDiamond);
			Singleton<UiManager>.Instance.TopBar.Refresh();
			return true;
		}
		Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, skipDiamond);
		GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
		return false;
	}

	public void OnLayerMove()
	{
		if (this.EquipmentRoot.GetComponent<RectTransform>().sizeDelta.x < 1470f)
		{
			this.LeftMark.gameObject.SetActive(false);
			this.RightMark.gameObject.SetActive(false);
		}
		else
		{
			if (this.EquipmentRoot.localPosition.x < -830f)
			{
				this.LeftMark.gameObject.SetActive(true);
			}
			else
			{
				this.LeftMark.gameObject.SetActive(false);
			}
			if (this.RootWidth - 640f + this.EquipmentRoot.localPosition.x > 190f)
			{
				this.RightMark.gameObject.SetActive(true);
			}
			else
			{
				this.RightMark.gameObject.SetActive(false);
			}
		}
	}

	public void ShowTimeEffect(bool isOpen)
	{
		this.startEffect = isOpen;
		if (isOpen && this.TimeRoot.activeSelf)
		{
			this.TimeValue.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
			this.TimeValue.DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
		}
		else
		{
			this.TimeValue.transform.DOScale(Vector3.one, 0.1f);
			this.TimeValue.DOFade(1f, 0.1f);
		}
	}

	private void Update()
	{
		if (this.TimeRoot.activeSelf)
		{
			this.CountDown -= Time.deltaTime;
			this.TimeValue.text = GameTimeManager.ConvertToString((int)this.CountDown);
			this.UpgradeButtonCostValue.text = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown).ToString();
			this.EquipButtonCostValue.text = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown).ToString();
			if (this.CountDown < 10f && this.CountDown > 0f)
			{
				if (!this.startEffect)
				{
					this.ShowTimeEffect(true);
				}
			}
			else
			{
				this.ShowTimeEffect(false);
			}
			if (this.CountDown <= 0f)
			{
				EquipmentData equipmentData = this.EquipmentDatas[this.SelectIndex];
				if (equipmentData.State == EquipmentState.制作中)
				{
					EquipmentDataManager.FinishFabricate(equipmentData.ID);
					EquipmentDataManager.Collect(equipmentData.ID);
				}
				else if (equipmentData.State == EquipmentState.升级中)
				{
					EquipmentDataManager.FinishUpgrade(equipmentData.ID);
					Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.WeaponUpClip, false);
					FunctionPage.Instance.ShowUpgradeEffect();
				}
				UITick.setItemSec(equipmentData.ID, 0);
				this.RefreshInfo();
				this.RefreshChildren();
			}
		}
	}

	public Transform InfoLayer;

	public Transform EquipmentLayer;

	public Image EquipmentIcon;

	public Text EquipmentPower;

	public Text EquipmentName;

	public Text EquipmentPart;

	public Text BasicAttributeTitle;

	public Text BasicAttributeValue;

	public Text SetAttributeTitle;

	public Text SetAttributeValue;

	public Text SetTitle;

	public Text SetCapName;

	public Text SetCoatName;

	public Text SetShoesName;

	public GameObject TimeRoot;

	public Text TimeValue;

	public GameObject CostRoot;

	public Text CostValue;

	public Text ShuoMingTxt;

	public Button EquipButton;

	public Text EquipButtonName;

	public GameObject EquipButtonCost;

	public Text EquipButtonCostValue;

	public Button UpgradeButton;

	public Text UpgradeButtonName;

	public GameObject UpgradeButtonCost;

	public Text UpgradeButtonCostValue;

	public GameObject curEffect;

	public DOTweenAnimation effectAni;

	public Transform EquipmentRoot;

	public Transform SelectImage;

	public Transform LeftMark;

	public Transform RightMark;

	public Transform CenterTimeAndCost;

	public Image PlayerHeadIcon;

	public Text PlayerHeadPart;

	public Image PlayerBagIcon;

	public Text PlayerBagPart;

	public Image PlayerHandsIcon;

	public Text PlayerHandsPart;

	public Transform RightAnchor;

	public Transform DownAnchor;

	public GameObject HeadButtonGO;

	public GameObject Buttons;

	public Text ShowAllTxt;

	private int count;

	public GameObject Buybtn;

	public Text BuyBtnName;

	public Image CostTypeImg;

	public Text CostNumTxt;

	private List<EquipmentChild> EquipmentChildren = new List<EquipmentChild>();

	private List<EquipmentData> EquipmentDatas = new List<EquipmentData>();

	private float RootWidth;

	private int SelectIndex;

	private bool IsAction;

	private float CountDown;

	private Vector3 pos01;

	private Vector3 pos02;

	private UITeachPage curTeachPage;

	public Sprite AddSprite;

	public GameObject[] ChooseGo;

	public Image ChooseImage;

	private int ChooseIndex;

	private bool startEffect;
}
