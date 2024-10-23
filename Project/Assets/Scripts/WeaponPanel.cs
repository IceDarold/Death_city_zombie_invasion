using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using inApps;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
	private void Awake()
	{
		this.WeaponDataList = WeaponDataManager.Weapons;
		this.WeaponRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(this.WeaponDataList.Count * 195), 140f);
		this.RootWidth = (float)(this.WeaponDataList.Count * 195);
		for (int i = 0; i < this.WeaponDataList.Count; i++)
		{
			int index = i;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/WeaponInfoChild")) as GameObject;
			gameObject.transform.SetParent(this.WeaponRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectWeapon(index);
			});
			WeaponInfoChild component = gameObject.GetComponent<WeaponInfoChild>();
			this.WeaponChildren.Add(component);
		}
	}

	private void OnEnable()
	{
		this.CloseEffect();
		this.WeaponIndex = 0;
		this.WeaponPartIndex = 0;
		this.isPart = false;
		this.chooseIndex = Singleton<GlobalData>.Instance.selectWeapon;
		this.SelectImage.transform.position = this.WeaponChildren[this.WeaponIndex].transform.position;
		this.SelectImage.gameObject.SetActive(false);
		this.CheckPosAndIndex();
		this.CheckNewUnlock();
		this.InitPartButtons();
		this.Refresh();
		this.InfoLayer.localPosition = this.RightAnchor.localPosition + new Vector3(320f, 0f, 0f);
		this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.PartsLayer.gameObject.SetActive(false);
		this.WeaponLayer.gameObject.SetActive(true);
		this.WeaponLayer.localPosition = this.DownAnchor.localPosition - new Vector3(0f, 140f, 0f);
		this.WeaponLayer.DOLocalMove(this.DownAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo).OnComplete(delegate
		{
			if (Singleton<GlobalData>.Instance.FirstWeapon == 2)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.BootomPos.gameObject.SetActive(false);
				this.curTeachPage.EffectObj.gameObject.SetActive(true);
				this.curTeachPage.type = TeachUIType.Weapon;
				this.curTeachPage.RefreshPage();
				this.chooseIndex = 1;
				Singleton<GlobalData>.Instance.selectWeapon = this.chooseIndex;
				this.WeaponIndex = 1;
				this.curTeachPage.BootomPos.gameObject.SetActive(true);
				this.curTeachPage.EffectObj.gameObject.SetActive(false);
				this.curTeachPage.BootomPos.position = this.WeaponChildren[this.WeaponIndex].transform.position;
				this.curTeachPage.curWeaponInfo.init(this.WeaponDataList[this.WeaponIndex], true);
				this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
				this.curTeachPage.GuideGo.transform.position = this.curTeachPage.curWeaponInfo.transform.position;
				this.curTeachPage.Guide02Txt.text = Singleton<GlobalData>.Instance.GetText("GUIDETEXT_05");
				this.curTeachPage.GuideTxt.text = string.Empty;
				Singleton<FontChanger>.Instance.SetFont(curTeachPage.Guide02Txt);
				Singleton<FontChanger>.Instance.SetFont(curTeachPage.GuideTxt);
				this.RefreshChildren();
				this.RefreshInfo();
				this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.EquipButton.gameObject);
				this.curTeachPage.Button.gameObject.SetActive(false);
			}
			else
			{
				this.curTeachPage = null;
			}
			if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.WeaponUpgrade)
			{
				UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				component.Button = component.ProduceGo(this.UpgradeButton.gameObject);
				component.EffectObj.SetActive(true);
				this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
				component.RefreshPage();
			}
		});
	}

	public void CheckPosAndIndex()
	{
		int weapon = PlayerDataManager.Player.Weapon1;
		int weapon2 = PlayerDataManager.Player.Weapon2;
		if (weapon2 == 0)
		{
			return;
		}
		if (Singleton<GlobalData>.Instance.selectWeapon == 0)
		{
			for (int i = 0; i < this.WeaponDataList.Count; i++)
			{
				if (this.WeaponDataList[i].ID == weapon)
				{
					this.WeaponIndex = i;
					if ((float)(this.WeaponIndex * 190) > this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x / 4f && (float)(this.WeaponIndex * 190) < this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x - 740f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3((float)(-(float)(this.WeaponIndex + 1) * 190), this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
					else if ((float)(this.WeaponIndex * 190) <= this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x / 4f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3(-640f, this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
					else if ((float)(this.WeaponIndex * 190) >= this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x - 740f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3(-2240f, this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < this.WeaponDataList.Count; j++)
			{
				if (this.WeaponDataList[j].ID == weapon2)
				{
					this.WeaponIndex = j;
					if ((float)(this.WeaponIndex * 190) > this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x / 4f && (float)(this.WeaponIndex * 190) < this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x - 740f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3((float)(-(float)(this.WeaponIndex + 1) * 190), this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
					else if ((float)(this.WeaponIndex * 190) <= this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x / 4f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3(-640f, this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
					else if ((float)(this.WeaponIndex * 190) >= this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x - 740f)
					{
						this.WeaponRoot.DOLocalMove(new Vector3(-2240f, this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z), 0.2f, false);
					}
				}
			}
		}
	}

	public void CheckNewUnlock()
	{
		if (Singleton<GlobalData>.Instance.UnlockShowWd == null)
		{
			return;
		}
		for (int i = 0; i < this.WeaponDataList.Count; i++)
		{
			if (this.WeaponDataList[i].ID == Singleton<GlobalData>.Instance.UnlockShowWd.ID)
			{
				this.WeaponIndex = i;
				this.WeaponRoot.localPosition = new Vector3((float)(-(float)this.WeaponIndex * 190) - this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x / 2f, this.WeaponRoot.localPosition.y, this.WeaponRoot.localPosition.z);
				Singleton<GlobalData>.Instance.UnlockShowWd = null;
				break;
			}
		}
	}

	public void RefreshChildren()
	{
		this.count = 0;
		for (int i = 0; i < this.WeaponDataList.Count; i++)
		{
			this.WeaponChildren[i].init(this.WeaponDataList[i], this.WeaponIndex == i);
			if (this.WeaponDataList[i].isNew)
			{
				this.count++;
			}
		}
		FunctionPage.Instance.FunctionButtons[0].Tip.gameObject.SetActive(this.count > 0);
	}

	public void RefreshCurrentChild()
	{
		WeaponData data = this.WeaponDataList[this.WeaponIndex];
		this.WeaponChildren[this.WeaponIndex].init(data, true);
	}

	public void CloseEffect()
	{
		this.ClickEffect.gameObject.SetActive(false);
	}

	public void SetEffectOnclick(Transform trans)
	{
	}

	private void ShowClickEffect(Vector3 position)
	{
		this.ClickEffect.transform.position = position;
		this.ClickEffect.color = Color.yellow;
		this.ClickEffect.transform.localScale = Vector3.one;
		this.ClickEffect.gameObject.SetActive(true);
		this.ClickEffect.DOColor(new Color(1f, 1f, 1f, 0f), 0.8f).SetEase(Ease.OutQuad);
		this.ClickEffect.transform.DOScale(new Vector3(2f, 2f, 2f), 0.8f).SetEase(Ease.InQuad);
	}

	public void Refresh()
	{
		WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
		Singleton<FontChanger>.Instance.SetFont(WeaponPower);
		this.WeaponPower.text = string.Concat(new string[]
		{
			Singleton<GlobalData>.Instance.GetText("FIREPOWER"),
			" ",
			WeaponDataManager.GetCurrentFightingStrength(weaponData).ToString(),
			" / ",
			WeaponDataManager.GetMaxFightingStrength(weaponData).ToString()
		});
		if (this.isPart)
		{
			this.SelectPositionPart.gameObject.SetActive(false);
			this.DebrisInfo.SetActive(false);
			this.RefrshPartButtons();
		}
		else
		{
			this.SelectPositionPart.gameObject.SetActive(true);
			this.RefreshSelectPositionPart(weaponData);
			if (weaponData.State == WeaponState.未解锁)
			{
				this.DebrisInfo.SetActive(true);
				DebrisData debrisData = DebrisDataManager.GetDebrisData(weaponData.DebrisID);
				this.DebrisSlider.maxValue = (float)weaponData.RequiredDebris;
				this.DebrisSlider.value = (float)debrisData.Count;
				this.DebrisCounts.text = debrisData.Count + "/" + weaponData.RequiredDebris;
				this.DebrisNameTxt.text = Singleton<GlobalData>.Instance.GetText("DRAWING");
				Singleton<FontChanger>.Instance.SetFont(DebrisNameTxt);
			}
			else
			{
				this.DebrisInfo.SetActive(false);
			}
		}
		this.RefreshInfo();
		this.RefreshChildren();
	}

	public void RefreshInfo()
	{
		WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
		if (weaponData.Type == WeaponType.SNIPER_RIFLE)
		{
			this.WeaponIcon.gameObject.SetActive(false);
			this.WeaponModelRoot.gameObject.SetActive(true);
			this.DisplayWeaponModel(weaponData.UiModel);
			if (this.isPart)
			{
				WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(weaponData.ID, (WeaponPartEnum)this.WeaponPartIndex);
				this.WeaponName.text = Singleton<GlobalData>.Instance.GetText(waponPartData.Name);
				this.WeaponTypeName.gameObject.SetActive(false);
				this.WeaponLevel.gameObject.SetActive(waponPartData.Level > 0);
				this.WeaponLevel.text = Singleton<GlobalData>.Instance.GetText("LEVEL") + waponPartData.Level.ToString().PadLeft(3, ' ');
				this.RefreshAttributes(weaponData, waponPartData);
				this.RefreshButtonState(weaponData, waponPartData);
			}
			else
			{
				this.WeaponName.text = Singleton<GlobalData>.Instance.GetText(weaponData.Name);
				this.WeaponTypeName.gameObject.SetActive(true);
				this.WeaponTypeName.text = Singleton<GlobalData>.Instance.GetText(weaponData.Type.ToString());
				this.WeaponLevel.gameObject.SetActive(false);
				if (weaponData.Type == WeaponType.SNIPER_RIFLE && this.CurrentModel != null)
				{
					this.CurrentModel.ShowAction("Idel");
				}
				this.RefreshAttributes(weaponData, null);
				this.RefreshButtonState(weaponData, null);
			}
		}
		else
		{
			WeaponPartData waponPartData2 = WeaponPartSystem.GetWaponPartData(weaponData.ID, WeaponPartEnum.BULLET);
			this.WeaponIcon.gameObject.SetActive(true);
			this.WeaponIcon.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
			this.WeaponModelRoot.gameObject.SetActive(false);
			this.WeaponName.text = Singleton<GlobalData>.Instance.GetText(weaponData.Name);
			this.WeaponTypeName.gameObject.SetActive(true);
			this.WeaponTypeName.text = Singleton<GlobalData>.Instance.GetText(weaponData.Type.ToString());
			this.WeaponLevel.gameObject.SetActive(waponPartData2.Level > 0);
			this.WeaponLevel.text = Singleton<GlobalData>.Instance.GetText("LEVEL") + waponPartData2.Level.ToString().PadLeft(3, ' ');
			this.RefreshAttributes(weaponData, null);
			this.RefreshButtonState(weaponData, null);
		}
		Singleton<FontChanger>.Instance.SetFont(WeaponName);
		Singleton<FontChanger>.Instance.SetFont(WeaponTypeName);
		Singleton<FontChanger>.Instance.SetFont(WeaponLevel);
	}

	private void RefreshAttributes(WeaponData _weapon, WeaponPartData _part = null)
	{
		for (int i = 0; i < this.WeaponAttributes.Count; i++)
		{
			if (_part == null)
			{
				this.WeaponAttributes[i].Refresh(_weapon);
			}
			else
			{
				this.WeaponAttributes[i].Refresh(_weapon, _part);
			}
		}
	}

	private void DisplayWeaponModel(string name)
	{
		this.CurrentModel = null;
		for (int i = 0; i < this.WeaponModels.Count; i++)
		{
			this.WeaponModels[i].gameObject.SetActive(false);
			if (this.WeaponModels[i].name == name)
			{
				this.CurrentModel = this.WeaponModels[i];
			}
		}
		if (this.CurrentModel == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UIWeapons/" + name)) as GameObject;
			gameObject.name = name;
			gameObject.transform.SetParent(this.WeaponModelRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = new Vector3(0f, 235f, 0f);
			gameObject.transform.localScale = Vector3.one * 650f;
			WeaponModelEffect component = gameObject.GetComponent<WeaponModelEffect>();
			this.WeaponModels.Add(component);
			this.CurrentModel = component;
		}
		this.CurrentModel.gameObject.SetActive(true);
	}

	private void RefreshButtonState(WeaponData _weapon, WeaponPartData _part = null)
	{
		Singleton<FontChanger>.Instance.SetFont(BuyButtonName);
		Singleton<FontChanger>.Instance.SetFont(EquipButtonName);
		Singleton<FontChanger>.Instance.SetFont(UpgradeButtonName);
		Singleton<FontChanger>.Instance.SetFont(BuyPrice);
		switch (_weapon.State)
		{
		case WeaponState.未解锁:
			this.BuyButtonName.text = Singleton<GlobalData>.Instance.GetText("BUY");
			this.BuyButton.gameObject.SetActive(true);
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("BUYDEBRIS");
			this.EquipButtonCost.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.UpgradeButton.gameObject.SetActive(false);
			this.CostRoot.SetActive(false);
			this.TimeRoot.SetActive(false);
			switch (_weapon.UnlockType)
			{
			case ItemUnlockType.NONE:
				this.BuyButton.gameObject.SetActive(false);
				break;
			case ItemUnlockType.RMB:
				this.BuyTypeIcon.sprite = Singleton<UiManager>.Instance.CommonSprites[10];
				// this.BuyPrice.text = StoreDataManager.GetChargePoint(_weapon.UnlockPrice).Price.ToString();
				this.BuyPrice.text = Singleton<InApps>.Instance.GetPrice(_weapon.UnlockPrice);
				break;
			case ItemUnlockType.GOLD:
				this.BuyTypeIcon.sprite = Singleton<UiManager>.Instance.CommonSprites[0];
				this.BuyPrice.text = _weapon.UnlockPrice.ToString();
				break;
			case ItemUnlockType.DIAMOND:
				this.BuyTypeIcon.sprite = Singleton<UiManager>.Instance.CommonSprites[1];
				this.BuyPrice.text = _weapon.UnlockPrice.ToString();
				break;
			case ItemUnlockType.DNA:
				this.BuyTypeIcon.sprite = Singleton<UiManager>.Instance.CommonSprites[5];
				this.BuyPrice.text = _weapon.UnlockPrice.ToString();
				break;
			}
			break;
		case WeaponState.待制作:
			this.BuyButton.gameObject.SetActive(false);
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("PRODUCE");
			this.EquipButtonCost.SetActive(false);
			this.EquipADImageGo.SetActive(false);
			this.EquipButton.gameObject.SetActive(true);
			this.UpgradeButton.gameObject.SetActive(false);
			this.CostRoot.transform.localPosition = new Vector3(0f, this.CostRoot.transform.localPosition.y, this.CostRoot.transform.localPosition.z);
			this.CostValue.text = _weapon.ProducePrice.ToString();
			this.CostRoot.SetActive(true);
			this.TimeRoot.SetActive(false);
			break;
		case WeaponState.制作中:
			this.CountDown = (float)UITick.getItemRemainderSec(_weapon.ID, _weapon.ProduceTime);
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
			if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
			{
				this.EquipADImageGo.SetActive(true);
				this.EquipButtonCost.SetActive(false);
			}
			else
			{
				this.EquipADImageGo.SetActive(false);
				this.EquipButtonCost.SetActive(true);
			}
			this.EquipButton.gameObject.SetActive(true);
			this.BuyButton.gameObject.SetActive(false);
			this.UpgradeButton.gameObject.SetActive(false);
			this.CostRoot.SetActive(false);
			this.TimeRoot.transform.localPosition = new Vector3(0f, this.TimeRoot.transform.localPosition.y, this.TimeRoot.transform.localPosition.z);
			this.TimeRoot.SetActive(true);
			break;
		case WeaponState.待领取:
		case WeaponState.待升级:
			if (_weapon.State == WeaponState.待领取)
			{
				_weapon.State = WeaponState.待升级;
			}
			this.BuyButton.gameObject.SetActive(false);
			if (_weapon.Type == WeaponType.SNIPER_RIFLE)
			{
				if (_part != null)
				{
					this.EquipButtonCost.SetActive(false);
					this.EquipButton.gameObject.SetActive(false);
					WeaponAttributeState state = _part.State;
					if (state != WeaponAttributeState.IDEL)
					{
						if (state != WeaponAttributeState.UPGRADE)
						{
							if (state == WeaponAttributeState.MAX)
							{
								this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("LEVELMAX");
								this.UpgradeButtonCost.SetActive(false);
								this.UpADImageGo.SetActive(false);
								this.UpgradeButton.interactable = false;
								this.UpgradeButton.gameObject.SetActive(true);
								this.CostRoot.SetActive(false);
								this.TimeRoot.SetActive(false);
							}
						}
						else
						{
							this.CountDown = (float)_part.RequiredTime[_part.Level] - (float)GameTimeManager.CalculateTimeToNow(_part.ID.ToString("D3") + _part.Name);
							this.UpgradeButton.interactable = true;
							if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
							{
								this.UpADImageGo.SetActive(true);
								this.UpgradeButtonCost.SetActive(false);
							}
							else
							{
								this.UpADImageGo.SetActive(false);
								this.UpgradeButtonCost.SetActive(true);
							}
							this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
							this.UpgradeButton.gameObject.SetActive(true);
							this.CostRoot.SetActive(false);
							this.TimeRoot.transform.localPosition = new Vector3(0f, this.TimeRoot.transform.localPosition.y, this.TimeRoot.transform.localPosition.z);
							this.TimeRoot.SetActive(true);
						}
					}
					else
					{
						this.UpgradeButton.interactable = true;
						this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
						this.UpgradeButtonCost.SetActive(false);
						this.UpADImageGo.SetActive(false);
						this.UpgradeButton.gameObject.SetActive(true);
						this.CostRoot.transform.localPosition = new Vector3(0f, this.CostRoot.transform.localPosition.y, this.CostRoot.transform.localPosition.z);
						this.CostValue.text = _part.RequiredPrice[_part.Level].ToString();
						this.CostRoot.SetActive(true);
						this.TimeRoot.SetActive(false);
					}
				}
				else
				{
					if (_weapon.ID == PlayerDataManager.Player.Weapon1 || _weapon.ID == PlayerDataManager.Player.Weapon2)
					{
						this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
					}
					else
					{
						this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
					}
					this.EquipButtonCost.SetActive(false);
					this.EquipADImageGo.SetActive(false);
					this.EquipButton.gameObject.SetActive(true);
					this.UpADImageGo.SetActive(false);
					this.UpgradeButton.interactable = true;
					this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("REMOULD");
					this.UpgradeButtonCost.SetActive(false);
					this.UpgradeButton.gameObject.SetActive(true);
					this.CostRoot.SetActive(false);
					this.TimeRoot.SetActive(false);
				}
			}
			else
			{
				WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(_weapon.ID, WeaponPartEnum.BULLET);
				if (_weapon.ID == PlayerDataManager.Player.Weapon1 || _weapon.ID == PlayerDataManager.Player.Weapon2)
				{
					this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
				}
				else
				{
					this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
				}
				this.EquipButtonCost.SetActive(false);
				this.EquipADImageGo.SetActive(false);
				this.EquipButton.gameObject.SetActive(true);
				WeaponAttributeState state2 = waponPartData.State;
				if (state2 != WeaponAttributeState.IDEL)
				{
					if (state2 != WeaponAttributeState.UPGRADE)
					{
						if (state2 == WeaponAttributeState.MAX)
						{
							this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("LEVELMAX");
							this.UpgradeButtonCost.SetActive(false);
							this.UpgradeButton.interactable = false;
							this.UpADImageGo.SetActive(false);
							this.UpgradeButton.gameObject.SetActive(true);
							this.CostRoot.SetActive(false);
							this.TimeRoot.SetActive(false);
						}
					}
					else
					{
						this.CountDown = (float)waponPartData.RequiredTime[waponPartData.Level] - (float)GameTimeManager.CalculateTimeToNow(waponPartData.ID.ToString("D3") + waponPartData.Name);
						this.UpgradeButton.interactable = true;
						if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
						{
							this.UpADImageGo.SetActive(true);
							this.UpgradeButtonCost.SetActive(false);
						}
						else
						{
							this.UpADImageGo.SetActive(false);
							this.UpgradeButtonCost.SetActive(true);
						}
						this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
						this.UpgradeButton.gameObject.SetActive(true);
						this.CostRoot.SetActive(false);
						this.TimeRoot.transform.localPosition = new Vector3(80f, this.TimeRoot.transform.localPosition.y, this.TimeRoot.transform.localPosition.z);
						this.TimeRoot.SetActive(true);
					}
				}
				else
				{
					this.UpgradeButton.interactable = true;
					this.UpgradeButtonName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
					this.UpgradeButtonCost.SetActive(false);
					this.UpADImageGo.SetActive(false);
					this.UpgradeButton.gameObject.SetActive(true);
					this.CostRoot.transform.localPosition = new Vector3(80f, this.TimeRoot.transform.localPosition.y, this.TimeRoot.transform.localPosition.z);
					this.CostValue.text = waponPartData.RequiredPrice[waponPartData.Level].ToString();
					this.CostRoot.SetActive(true);
					this.TimeRoot.SetActive(false);
				}
			}
			break;
		case WeaponState.升级中:
		case WeaponState.已满级:
			_weapon.State = WeaponState.待升级;
			this.RefreshButtonState(_weapon, null);
			break;
		}
	}

	private void InitPartButtons()
	{
		for (int i = 0; i < this.PartButtons.Count; i++)
		{
			Text name = this.PartButtons[i].Name;
			GlobalData instance = Singleton<GlobalData>.Instance;
			WeaponPartEnum weaponPartEnum = (WeaponPartEnum)i;
			name.text = instance.GetText(weaponPartEnum.ToString());
			Singleton<FontChanger>.Instance.SetFont(name);
		}
		this.PartsLayer.gameObject.SetActive(false);
	}

	private void RefrshPartButtons()
	{
		WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
		for (int i = 0; i < weaponData.Parts.Length; i++)
		{
			this.PartButtons[i].Select.gameObject.SetActive(this.WeaponPartIndex == i);
			this.PartButtons[i].Content.interactable = (this.WeaponPartIndex != i);
			if (weaponData.Parts[i] == 0)
			{
				this.PartButtons[i].gameObject.SetActive(false);
			}
			else
			{
				this.PartButtons[i].gameObject.SetActive(true);
			}
		}
	}

	private void SelectWeapon(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (index == this.WeaponIndex || this.IsAction)
		{
			return;
		}
		this.IsAction = true;
		this.SelectImage.position = this.WeaponChildren[this.WeaponIndex].transform.position;
		this.WeaponChildren[this.WeaponIndex].SelectImage.gameObject.SetActive(false);
		this.SelectImage.gameObject.SetActive(true);
		this.WeaponIndex = index;
		this.SelectImage.DOMoveX(this.WeaponChildren[index].transform.position.x, 0.2f, false).OnComplete(delegate
		{
			this.IsAction = false;
			this.SelectImage.gameObject.SetActive(false);
			WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
			if (weaponData.isNew)
			{
				WeaponDataManager.RemoveNewTag(weaponData.ID);
				this.count--;
				FunctionPage.Instance.FunctionButtons[0].Tip.gameObject.SetActive(this.count > 0);
			}
			this.Refresh();
			this.InfoLayer.localPosition = this.RightAnchor.localPosition + new Vector3(320f, 0f, 0f);
			this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		});
	}

	private void SelectWeaponPart(int index)
	{
		this.WeaponPartIndex = index;
		this.RefreshInfo();
		this.RefrshPartButtons();
		this.CurrentModel.SetHighLinghting(index);
		this.InfoLayer.localPosition = this.RightAnchor.localPosition + new Vector3(320f, 0f, 0f);
		this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
	}

	public void OnclickBuy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		WeaponData data = this.WeaponDataList[this.WeaponIndex];
		switch (data.UnlockType)
		{
		case ItemUnlockType.RMB:
			Singleton<GlobalData>.Instance.DoCharge(data.UnlockPrice, delegate
			{
				WeaponDataManager.CollectWeapon(data.ID);
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
		this.Refresh();
	}

	public void CostByType(CommonDataType type, WeaponData data)
	{
		if (ItemDataManager.GetCurrency(type) >= data.UnlockPrice)
		{
			ItemDataManager.SetCurrency(type, -data.UnlockPrice);
			WeaponDataManager.CollectWeapon(data.ID);
		}
		else
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(type, data.UnlockPrice - ItemDataManager.GetCurrency(type));
		}
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            WeaponData data = this.WeaponDataList[this.WeaponIndex];
            this.CloseEffect();
            WeaponDataManager.FinishFabricate(data.ID);
            WeaponDataManager.CollectWeapon(data.ID);
            this.RefreshChildren();
            this.RefreshInfo();
        }

    }
    public void ClickOnEquip()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		WeaponData data = this.WeaponDataList[this.WeaponIndex];
		switch (data.State)
		{
		case WeaponState.未解锁:
			if (Singleton<GlobalData>.Instance.isDebug)
			{
				WeaponDataManager.Unlock(data.ID);
			}
			else
			{
				GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMeterialPopup.ToString());
				Singleton<UiManager>.Instance.ShowPage(PageName.LackOfMeterialPopup, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Clear();
					for (int i = 0; i < data.DropID.Length; i++)
					{
						Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Add(data.DropID[i]);
					}
				});
			}
			break;
		case WeaponState.待制作:
			if (ItemDataManager.GetCurrency(CommonDataType.GOLD) >= data.ProducePrice)
			{
				ItemDataManager.SetCurrency(CommonDataType.GOLD, -data.ProducePrice);
				WeaponDataManager.StartFabricate(data.ID);
				UITick.setItemSec(data.ID, 1);
				this.ShowClickEffect(this.EquipButton.transform.position);
				GameLogManager.SendCostLog(1, data.ProducePrice, data.ID, 1);
				GameLogManager.SendPageLog(data.Name, "Produce");
				Singleton<UiManager>.Instance.TopBar.Refresh();
			}
			else
			{
				int num = data.ProducePrice - ItemDataManager.GetCurrency(CommonDataType.GOLD);
				Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.GOLD, num);
				GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
			}
			break;
		case WeaponState.制作中:
			if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
			{
                    //Advertisements.Instance.ShowRewardedVideo(OnFinished);
                Ads.ShowReward(() =>
                {
                    WeaponData data = this.WeaponDataList[this.WeaponIndex];
                    this.CloseEffect();
                    WeaponDataManager.FinishFabricate(data.ID);
                    WeaponDataManager.CollectWeapon(data.ID);
                    this.RefreshChildren();
                    this.RefreshInfo();
                });
                Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount--;
				//Singleton<GlobalData>.Instance.ShowAdvertisement(-10, delegate
				//{
				//	this.CloseEffect();
				//	WeaponDataManager.FinishFabricate(data.ID);
				//	WeaponDataManager.CollectWeapon(data.ID);
				//	this.RefreshChildren();
				//	this.RefreshInfo();
				//}, null);
			}
			else
			{
				int skipDiamond = Singleton<GlobalData>.Instance.GetSkipDiamond(UITick.getItemRemainderSec(data.ID, data.ProduceTime));
				if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= skipDiamond)
				{
					GameLogManager.SendCostLog(2, skipDiamond, data.ID, 3);
					ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -skipDiamond);
					this.CloseEffect();
					WeaponDataManager.FinishFabricate(data.ID);
					WeaponDataManager.CollectWeapon(data.ID);
					Singleton<UiManager>.Instance.TopBar.Refresh();
				}
				else
				{
					Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, skipDiamond);
					GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
				}
			}
			break;
		case WeaponState.待领取:
			WeaponDataManager.CollectWeapon(data.ID);
			break;
		default:
			if (this.chooseIndex == 0)
			{
				int weapon = PlayerDataManager.Player.Weapon1;
				PlayerDataManager.Equip(EquipmentPosition.Weapon1, data.ID);
				if (data.ID == PlayerDataManager.Player.Weapon2)
				{
					PlayerDataManager.Equip(EquipmentPosition.Weapon2, weapon);
				}
				this.SelectEquipPosition(1);
				if (null != this.curTeachPage)
				{
					Singleton<GlobalData>.Instance.FirstWeapon = 1;
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
					Singleton<UiManager>.Instance.SetTopEnable(true, true);
					this.curTeachPage.Close();
				}
			}
			else
			{
				int weapon2 = PlayerDataManager.Player.Weapon2;
				PlayerDataManager.Equip(EquipmentPosition.Weapon2, data.ID);
				if (data.ID == PlayerDataManager.Player.Weapon1)
				{
					PlayerDataManager.Equip(EquipmentPosition.Weapon1, weapon2);
				}
				this.SelectEquipPosition(2);
				if (null != this.curTeachPage)
				{
					Singleton<GlobalData>.Instance.FirstWeapon = 1;
					if (null != this.curTeachPage.Button)
					{
						this.curTeachPage.Button.SetActive(false);
						UnityEngine.Object.Destroy(this.curTeachPage.Button);
					}
					this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
					Singleton<UiManager>.Instance.SetTopEnable(true, true);
					this.curTeachPage.Close();
				}
			}
			break;
		}
		this.Refresh();
	}
    private void OnFinished1(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
            WeaponPartData part = WeaponPartSystem.GetWaponPartData(weaponData.ID, (WeaponPartEnum)this.WeaponPartIndex);
            this.CloseEffect();
            WeaponPartSystem.FinishUpgrade(part);
            this.Refresh();
            FunctionPage.Instance.ShowUpgradeEffect();
        }

    }
    public void ClickOnUpgrade()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
		if (weaponData.Type == WeaponType.SNIPER_RIFLE)
		{
			if (this.isPart)
			{
				WeaponPartData part = WeaponPartSystem.GetWaponPartData(weaponData.ID, (WeaponPartEnum)this.WeaponPartIndex);
				WeaponAttributeState state = part.State;
				if (state != WeaponAttributeState.IDEL)
				{
					if (state == WeaponAttributeState.UPGRADE)
					{
						if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
						{
							Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount--;
                            //Advertisements.Instance.ShowRewardedVideo(OnFinished1);
                            Ads.ShowReward(() =>
                            {
	                            this.CloseEffect();
	                            WeaponPartSystem.FinishUpgrade(part);
	                            this.Refresh();
	                            FunctionPage.Instance.ShowUpgradeEffect();
                            });
       //                     Singleton<GlobalData>.Instance.ShowAdvertisement(-10, delegate
							//{
							//	this.CloseEffect();
							//	WeaponPartSystem.FinishUpgrade(part);
							//	this.Refresh();
							//	FunctionPage.Instance.ShowUpgradeEffect();
							//}, null);
						}
						else
						{
							int skipDiamond = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown);
							if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= skipDiamond)
							{
								GameLogManager.SendCostLog(2, skipDiamond, part.ID, 3);
								ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -skipDiamond);
								WeaponPartSystem.FinishUpgrade(part);
								Singleton<UiManager>.Instance.TopBar.Refresh();
								FunctionPage.Instance.ShowUpgradeEffect();
							}
							else
							{
								Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, skipDiamond);
								GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
							}
						}
					}
				}
				else if (ItemDataManager.GetCurrency(CommonDataType.GOLD) >= part.RequiredPrice[part.Level])
				{
					this.ShowClickEffect(this.UpgradeButton.transform.position);
					ItemDataManager.SetCurrency(CommonDataType.GOLD, -part.RequiredPrice[part.Level]);
					WeaponPartSystem.StartUpgrade(part);
					GameTimeManager.RecordTime(part.ID.ToString("D3") + part.Name);
					GameLogManager.SendCostLog(1, part.RequiredPrice[part.Level], part.ID, 2);
					GameLogManager.SendPageLog(part.Name, "Upgrade");
					Singleton<UiManager>.Instance.TopBar.Refresh();
				}
				else
				{
					int num = part.RequiredPrice[part.Level] - ItemDataManager.GetCurrency(CommonDataType.GOLD);
					Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.GOLD, num);
					GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
				}
			}
			else
			{
				this.isPart = true;
				this.WeaponPartIndex = 0;
				this.Refresh();
				this.WeaponLayer.DOLocalMoveY(this.DownAnchor.localPosition.y - 200f, 0.5f, false).OnComplete(delegate
				{
					if (this.CurrentModel != null)
					{
						this.CurrentModel.ShowAction("Snipe_Chai");
						this.CurrentModel.SetHighLinghting(this.WeaponPartIndex);
					}
					this.WeaponLayer.gameObject.SetActive(false);
					this.PartsLayer.gameObject.SetActive(true);
					this.PartsLayer.localPosition = this.DownAnchor.localPosition + new Vector3(0f, -200f, 0f);
					this.PartsLayer.DOLocalMoveY(this.DownAnchor.localPosition.y, 0.5f, false);
				});
			}
		}
		else
		{
			WeaponPartData part = WeaponPartSystem.GetWaponPartData(weaponData.ID, WeaponPartEnum.BULLET);
			WeaponAttributeState state2 = part.State;
			if (state2 != WeaponAttributeState.IDEL)
			{
				if (state2 != WeaponAttributeState.UPGRADE)
				{
					if (state2 != WeaponAttributeState.MAX)
					{
					}
				}
				else
				{
					if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.WeaponUpgrade)
					{
						return;
					}
					if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
					{
						Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount--;
                        //Advertisements.Instance.ShowRewardedVideo(OnFinished2);
                        Ads.ShowReward(() =>
                        {
	                        this.CloseEffect();
	                        WeaponPartSystem.FinishUpgrade(part);
	                        this.Refresh();
	                        FunctionPage.Instance.ShowUpgradeEffect(); 
                        });
      //                  Singleton<GlobalData>.Instance.ShowAdvertisement(-10, delegate
						//{
						//	this.CloseEffect();
						//	WeaponPartSystem.FinishUpgrade(part);
						//	this.Refresh();
						//	FunctionPage.Instance.ShowUpgradeEffect();
						//}, null);
					}
					else
					{
						int skipDiamond2 = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown);
						if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) >= skipDiamond2)
						{
							GameLogManager.SendCostLog(2, skipDiamond2, part.ID, 3);
							ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -skipDiamond2);
							WeaponPartSystem.FinishUpgrade(part);
							Singleton<UiManager>.Instance.TopBar.Refresh();
							FunctionPage.Instance.ShowUpgradeEffect();
						}
						else
						{
							Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, skipDiamond2);
							GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
						}
					}
					FunctionPage.Instance.ShowUpgradeEffect();
				}
			}
			else if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.WeaponUpgrade)
			{
				Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().Guide02Txt.text = string.Empty;
				Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().Button.SetActive(false);
				WeaponPartSystem.StartUpgrade(part);
				GameTimeManager.RecordTime(part.ID.ToString("D3") + part.Name);
				UnityEngine.Debug.Log(part.ID.ToString("D3") + part.Name);
				this.ShowClickEffect(this.UpgradeButton.transform.position);
				GameLogManager.SendCostLog(1, part.RequiredPrice[part.Level], part.ID, 2);
				GameLogManager.SendPageLog(part.Name, "Upgrade");
			}
			else
			{
				int num2 = part.RequiredPrice[part.Level];
				if (ItemDataManager.GetCurrency(CommonDataType.GOLD) >= num2)
				{
					this.ShowClickEffect(this.UpgradeButton.transform.position);
					ItemDataManager.SetCurrency(CommonDataType.GOLD, -num2);
					WeaponPartSystem.StartUpgrade(part);
					GameLogManager.SendCostLog(1, num2, part.ID, 2);
					GameLogManager.SendPageLog(part.Name, "Upgrade");
					Singleton<UiManager>.Instance.TopBar.Refresh();
					GameTimeManager.RecordTime(part.ID.ToString("D3") + part.Name);
				}
				else
				{
					int num3 = part.RequiredPrice[part.Level] - ItemDataManager.GetCurrency(CommonDataType.GOLD);
					Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.GOLD, num3);
					GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
				}
			}
		}
		this.Refresh();
	}
    private void OnFinished2(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
            WeaponPartData part = WeaponPartSystem.GetWaponPartData(weaponData.ID, (WeaponPartEnum)this.WeaponPartIndex); this.CloseEffect();
            WeaponPartSystem.FinishUpgrade(part);
            this.Refresh();
            FunctionPage.Instance.ShowUpgradeEffect();
        }

    }
    private void RefreshSelectPositionPart(WeaponData weapon)
	{
		Singleton<FontChanger>.Instance.SetFont(PosChooseEquipTxt);
		this.PosChooseEquipTxt.text = Singleton<GlobalData>.Instance.GetText("ChOOSE_POS");
		WeaponData weaponData = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
		WeaponData weaponData2 = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
		if (weaponData != null)
		{
			this.WeaponButton1.interactable = (weapon.ID != PlayerDataManager.Player.Weapon1);
			this.WeaponButtonIcon1.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
			Singleton<FontChanger>.Instance.SetFont(WeaponButtonPower1);
			this.WeaponButtonPower1.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER") + " " + WeaponDataManager.GetCurrentFightingStrength(weaponData);
			this.WeaponButtonPower1.gameObject.SetActive(true);
			Singleton<FontChanger>.Instance.SetFont(WeaponEquipTag1);
			this.WeaponEquipTag1.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			this.WeaponEquipTag1.gameObject.SetActive(true);
		}
		else
		{
			this.WeaponButton1.interactable = true;
			this.WeaponButtonIcon1.sprite = null;
			this.WeaponButtonPower1.gameObject.SetActive(false);
			this.WeaponEquipTag1.gameObject.SetActive(false);
		}
		if (weaponData2 != null)
		{
			this.WeaponButton2.interactable = (weapon.ID != PlayerDataManager.Player.Weapon2);
			this.WeaponButtonIcon2.gameObject.SetActive(true);
			this.WeaponButtonIcon2.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData2.Icon);
			Singleton<FontChanger>.Instance.SetFont(WeaponButtonPower2);
			this.WeaponButtonPower2.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER") + " " + WeaponDataManager.GetCurrentFightingStrength(weaponData2);
			this.WeaponButtonPower2.gameObject.SetActive(true);
			Singleton<FontChanger>.Instance.SetFont(WeaponEquipTag2);
			this.WeaponEquipTag2.text = Singleton<GlobalData>.Instance.GetText("HAVEEQUIP");
			this.WeaponEquipTag2.gameObject.SetActive(true);
		}
		else
		{
			this.WeaponButton1.interactable = true;
			this.WeaponButtonIcon2.gameObject.SetActive(false);
			this.WeaponButtonPower2.gameObject.SetActive(false);
			this.WeaponEquipTag2.gameObject.SetActive(false);
		}
	}

	public void SelectEquipPosition(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.EquipClip, false);
		WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
		if (index == 1)
		{
			this.chooseIndex = 0;
			Singleton<GlobalData>.Instance.selectWeapon = this.chooseIndex;
			this.CheckPosAndIndex();
		}
		else if (index == 2)
		{
			this.chooseIndex = 1;
			Singleton<GlobalData>.Instance.selectWeapon = this.chooseIndex;
			this.CheckPosAndIndex();
		}
		this.Refresh();
	}

	public void OnLayerMove()
	{
		if (this.WeaponRoot.GetComponent<RectTransform>().sizeDelta.x < 1470f)
		{
			this.LeftMark.gameObject.SetActive(false);
			this.RightMark.gameObject.SetActive(false);
		}
		else
		{
			if (this.WeaponRoot.localPosition.x < -830f)
			{
				this.LeftMark.gameObject.SetActive(true);
			}
			else
			{
				this.LeftMark.gameObject.SetActive(false);
			}
			if (this.RootWidth - 640f + this.WeaponRoot.localPosition.x > 190f)
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
			if (this.tween1 != null)
			{
				this.tween1.Restart(true, -1f);
				this.tween2.Restart(true, -1f);
			}
			else
			{
				this.tween1 = this.TimeValue.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
				this.tween2 = this.TimeValue.DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
			}
		}
	}

	public void OnBack()
	{
		this.isPart = false;
		this.Refresh();
		this.PartsLayer.gameObject.SetActive(false);
		this.WeaponLayer.gameObject.SetActive(true);
		this.WeaponLayer.localPosition = this.DownAnchor.localPosition + new Vector3(0f, -200f, 0f);
		this.WeaponLayer.DOLocalMoveY(this.DownAnchor.localPosition.y, 0.5f, false);
		if (this.CurrentModel != null)
		{
			this.CurrentModel.SetDefault();
			this.CurrentModel.ShowAction("Snipe_Zu");
		}
	}

	private void Update()
	{
		if (this.TimeRoot.activeSelf)
		{
			this.CountDown -= Time.deltaTime;
			this.TimeValue.text = GameTimeManager.ConvertToString((int)this.CountDown);
			this.EquipButtonCostValue.text = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown).ToString();
			this.UpgradePrice.text = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown).ToString();
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
				this.TimeValue.transform.localScale = Vector3.one;
				this.TimeValue.color = new Color(this.TimeValue.color.r, this.TimeValue.color.g, this.TimeValue.color.b, 1f);
			}
			if (this.CountDown <= 0f)
			{
				WeaponData weaponData = this.WeaponDataList[this.WeaponIndex];
				WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(weaponData.ID, (WeaponPartEnum)this.WeaponPartIndex);
				if (weaponData.State == WeaponState.制作中)
				{
					WeaponDataManager.FinishFabricate(weaponData.ID);
					WeaponDataManager.CollectWeapon(weaponData.ID);
				}
				else if (waponPartData.State == WeaponAttributeState.UPGRADE)
				{
					WeaponPartSystem.FinishUpgrade(waponPartData);
					if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.WeaponUpgrade)
					{
						Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type = TeachUIType.None;
						this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
						Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type = TeachUIType.WeaponUpgradeBack;
						Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().RefreshPage();
					}
				}
				FunctionPage.Instance.ShowUpgradeEffect();
				this.Refresh();
			}
		}
	}

	public Transform InfoLayer;

	public Transform WeaponLayer;

	public Transform PartsLayer;

	public Transform SelectPositionPart;

	public Transform RightAnchor;

	public Transform DownAnchor;

	[Space(10f)]
	public Transform WeaponModelRoot;

	public Image WeaponIcon;

	public Text WeaponPower;

	public GameObject DebrisInfo;

	public Slider DebrisSlider;

	public Text DebrisCounts;

	public Text DebrisNameTxt;

	[Space(10f)]
	public Text WeaponName;

	public Text WeaponLevel;

	public Text WeaponTypeName;

	public Button BuyButton;

	public Text BuyButtonName;

	public Image BuyTypeIcon;

	public Text BuyPrice;

	public Button EquipButton;

	public Text EquipButtonName;

	public GameObject EquipButtonCost;

	public Text EquipButtonCostValue;

	public GameObject EquipADImageGo;

	public Button UpgradeButton;

	public Text UpgradeButtonName;

	public GameObject UpgradeButtonCost;

	public Text UpgradePrice;

	public GameObject UpADImageGo;

	public GameObject TimeRoot;

	public Text TimeValue;

	public GameObject CostRoot;

	public Image CostIcon;

	public Text CostValue;

	public List<WeaponAttributeChild> WeaponAttributes = new List<WeaponAttributeChild>();

	[Space(10f)]
	public Text PosChooseEquipTxt;

	public Button WeaponButton1;

	public Image WeaponButtonIcon1;

	public Text WeaponButtonPower1;

	public Text WeaponEquipTag1;

	public Button WeaponButton2;

	public Image WeaponButtonIcon2;

	public Text WeaponButtonPower2;

	public Text WeaponEquipTag2;

	[Space(10f)]
	public Image ClickEffect;

	public Transform WeaponRoot;

	public Transform SelectImage;

	public Transform RightMark;

	public Transform LeftMark;

	public GameObject Buttons;

	public List<WeaponPartButton> PartButtons = new List<WeaponPartButton>();

	private List<WeaponInfoChild> WeaponChildren = new List<WeaponInfoChild>();

	private List<WeaponData> WeaponDataList = new List<WeaponData>();

	private List<WeaponModelEffect> WeaponModels = new List<WeaponModelEffect>();

	private WeaponModelEffect CurrentModel;

	private float RootWidth;

	private int WeaponIndex;

	private int WeaponPartIndex;

	private bool IsAction;

	private bool isOnback;

	private float CountDown;

	private int count;

	private UITeachPage curTeachPage;

	private int chooseIndex;

	[HideInInspector]
	public bool isPart;

	private bool startEffect;

	private Tweener tween1;

	private Tweener tween2;
}
