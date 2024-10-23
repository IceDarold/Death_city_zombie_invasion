using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class PropPanel : MonoBehaviour
{
	private void Awake()
	{
		this.PropDataList = PropDataManager.Props;
		this.PropRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(20 + this.PropDataList.Count * 195), 140f);
		this.RootWidth = (float)(this.PropDataList.Count * 195);
		for (int i = 0; i < this.PropDataList.Count; i++)
		{
			int index = i;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/PropInfoChild")) as GameObject;
			gameObject.transform.SetParent(this.PropRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectProp(index);
			});
			PropInfoChild component = gameObject.GetComponent<PropInfoChild>();
			this.PropChildren.Add(component);
		}
	}

	private void OnEnable()
	{
		Singleton<UiManager>.Instance.SetTopEnable(false, false);
		this.CloseEffect();
		this.SelectIndex = 0;
		this.chooseIndex = Singleton<GlobalData>.Instance.selectProp;
		this.SelectImage.transform.position = this.PropChildren[this.SelectIndex].transform.position;
		this.SelectImage.gameObject.SetActive(false);
		this.CheckPosAndIndex();
		this.RefreshChidren();
		this.RefreshInfo();
		this.InfoLayer.localPosition = new Vector3(850f, 60f, 0f);
		this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.PropLayer.localPosition = new Vector3(0f, -500f, 0f);
		this.PropLayer.DOLocalMove(this.DownAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo).OnComplete(delegate
		{
			if (Singleton<GlobalData>.Instance.FirstShouLei == 2)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.EffectObj.gameObject.SetActive(true);
				this.curTeachPage.type = TeachUIType.Prop;
				this.curTeachPage.RefreshPage();
				this.chooseIndex = 0;
				this.SelectIndex = 2;
				this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = false;
				this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.EquipButton.gameObject);
				this.curTeachPage.Button.SetActive(false);
				this.curTeachPage.BootomPos.gameObject.SetActive(true);
				this.curTeachPage.EffectObj.gameObject.SetActive(false);
				this.curTeachPage.BootomPos.position = this.PropChildren[this.SelectIndex].transform.position;
				this.curTeachPage.curPropInfo.init(this.PropDataList[this.SelectIndex], true);
				this.RefreshChidren();
				this.RefreshInfo();
			}
			else
			{
				this.curTeachPage = null;
				Singleton<UiManager>.Instance.SetTopEnable(true, true);
			}
		});
	}

	public void CheckPosAndIndex()
	{
		int prop = PlayerDataManager.Player.Prop1;
		int prop2 = PlayerDataManager.Player.Prop2;
		if (prop2 == 0)
		{
			return;
		}
		if (Singleton<GlobalData>.Instance.selectProp == 0)
		{
			for (int i = 0; i < this.PropDataList.Count; i++)
			{
				if (this.PropDataList[i].ID == prop)
				{
					this.SelectIndex = i;
					this.PropRoot.localPosition = new Vector3((float)(-(float)this.SelectIndex * 190), this.PropRoot.localPosition.y, this.PropRoot.localPosition.z);
				}
			}
		}
		else
		{
			for (int j = 0; j < this.PropDataList.Count; j++)
			{
				if (this.PropDataList[j].ID == prop2)
				{
					this.SelectIndex = j;
					this.PropRoot.localPosition = new Vector3((float)(-(float)this.SelectIndex * 190), this.PropRoot.localPosition.y, this.PropRoot.localPosition.z);
				}
			}
		}
	}

	public void OnclickBootom()
	{
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

	private void RefreshChidren()
	{
		this.count = 0;
		for (int i = 0; i < this.PropDataList.Count; i++)
		{
			this.PropChildren[i].init(this.PropDataList[i], i == this.SelectIndex);
			if (this.PropDataList[i].isNew)
			{
				this.count++;
			}
		}
		FunctionPage.Instance.FunctionButtons[3].Tip.gameObject.SetActive(this.count > 0);
	}

	private void RefreshSelectChlid()
	{
		PropData data = this.PropDataList[this.SelectIndex];
		this.PropChildren[this.SelectIndex].init(data, true);
	}

	public void ChooseImage()
	{
		for (int i = 0; i < this.ChooseImgs.Length; i++)
		{
			if (i == this.chooseIndex)
			{
				this.ChooseImgs[i].gameObject.SetActive(true);
			}
			else
			{
				this.ChooseImgs[i].gameObject.SetActive(false);
			}
		}
	}

	public void RefreshInfo()
	{
		this.PosChooseEquipTxt.text = Singleton<GlobalData>.Instance.GetText("ChOOSE_POS");
		PropData propData = this.PropDataList[this.SelectIndex];
		this.ChooseImage();
		this.PropIcon.sprite = Singleton<UiManager>.Instance.GetSprite(propData.Icon);
		this.PropIcon.SetNativeSize();
		this.PropName.text = Singleton<GlobalData>.Instance.GetText(propData.Name);
		this.PropCount.text = Singleton<GlobalData>.Instance.GetText("AMOUNT") + " " + propData.Count;
		this.PropDescribe.text = Singleton<GlobalData>.Instance.GetText(propData.Describe);
		Singleton<FontChanger>.Instance.SetFont(PropName);
		Singleton<FontChanger>.Instance.SetFont(PropCount);
		Singleton<FontChanger>.Instance.SetFont(PropDescribe);
		Singleton<FontChanger>.Instance.SetFont(DebrisTxt);
		Singleton<FontChanger>.Instance.SetFont(DebrisNameTxt);
		Singleton<FontChanger>.Instance.SetFont(EffectValue);
		Singleton<FontChanger>.Instance.SetFont(EffectName);
		if (propData.State == PropState.未解锁)
		{
			this.DebrisSlider.gameObject.SetActive(true);
			DebrisData debrisData = DebrisDataManager.GetDebrisData(propData.DebrisID);
			this.DebrisSlider.maxValue = (float)propData.RequiredDebris;
			this.DebrisSlider.value = (float)debrisData.Count;
			this.DebrisTxt.text = debrisData.Count + "/" + propData.RequiredDebris;
			this.DebrisNameTxt.text = Singleton<GlobalData>.Instance.GetText("DRAWING");
		}
		else
		{
			this.DebrisSlider.gameObject.SetActive(false);
		}
		if (propData.Type == PropType.MEDKIT)
		{
			this.EffectValue.text = propData.Value.ToString();
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("REP_LIFE");
		}
		else if (propData.Type == PropType.SNEER_BOMB)
		{
			this.EffectValue.text = propData.Duration.ToString() + "s";
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("Duration");
		}
		else if (propData.Type == PropType.TURRET)
		{
			this.EffectValue.text = propData.Duration.ToString() + "s";
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("Duration");
		}
		else if (propData.Type == PropType.ADRENALINE)
		{
			this.EffectValue.text = propData.Duration.ToString() + "s";
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("Duration");
		}
		else if (propData.Type == PropType.LANDMINE)
		{
			this.EffectValue.text = propData.Value.ToString();
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("CREATE") + Singleton<GlobalData>.Instance.GetText("DAMAGEES");
		}
		else if (propData.Type == PropType.GRENADE)
		{
			this.EffectValue.text = propData.Value.ToString();
			this.EffectName.text = Singleton<GlobalData>.Instance.GetText("CREATE") + Singleton<GlobalData>.Instance.GetText("DAMAGEES");
		}
		this.ProduceADImageGo.SetActive(false);
		this.SetButtonState(propData);
	}

	public void SetButtonState(PropData prop)
	{
		Singleton<FontChanger>.Instance.SetFont(EquipButtonName);
		Singleton<FontChanger>.Instance.SetFont(FabricateButtonName);
		switch (prop.State)
		{
		case PropState.未解锁:
			this.EquipButtonName.text = ((prop.Count <= 0) ? Singleton<GlobalData>.Instance.GetText("GOTOGAIN") : Singleton<GlobalData>.Instance.GetText("EQUIP"));
			this.EquipButton.gameObject.SetActive(true);
			this.FabricateButton.gameObject.SetActive(false);
			this.CostRoot.gameObject.SetActive(false);
			this.TimeRoot.gameObject.SetActive(false);
			break;
		case PropState.待制作:
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
			this.EquipButton.gameObject.SetActive(true);
			if (prop.Count < prop.LimitCount)
			{
				this.FabricateButton.interactable = true;
				this.FabricateButtonName.text = Singleton<GlobalData>.Instance.GetText("PRODUCE") + "X" + prop.FabricateNumber.ToString();
			}
			else
			{
				this.FabricateButton.interactable = false;
				this.FabricateButtonName.text = Singleton<GlobalData>.Instance.GetText("NUM_FULL");
			}
			this.FabricateButtonCost.SetActive(false);
			this.FabricateButton.gameObject.SetActive(true);
			this.CostValue.text = prop.FabricatePrice.ToString();
			this.CostRoot.gameObject.SetActive(true);
			this.TimeRoot.gameObject.SetActive(false);
			break;
		case PropState.制作中:
			this.CountDown = (this.CountDown = (float)UITick.getItemRemainderSec(prop.ID, prop.FabricateTime));
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
			this.EquipButton.gameObject.SetActive(true);
			this.FabricateButtonName.text = Singleton<GlobalData>.Instance.GetText("SPEEDUP");
			this.FabricateButton.interactable = true;
			if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
			{
				this.FabricateButtonCost.SetActive(false);
				this.ProduceADImageGo.SetActive(true);
			}
			else
			{
				this.FabricateButtonCost.SetActive(true);
				this.ProduceADImageGo.SetActive(false);
			}
			this.FabricateButton.gameObject.SetActive(true);
			this.CostRoot.gameObject.SetActive(false);
			this.TimeRoot.gameObject.SetActive(true);
			break;
		case PropState.待领取:
			prop.State = PropState.待制作;
			this.EquipButtonName.text = Singleton<GlobalData>.Instance.GetText("EQUIP");
			this.EquipButton.gameObject.SetActive(true);
			if (prop.Count < prop.LimitCount)
			{
				this.FabricateButton.interactable = true;
				this.FabricateButtonName.text = Singleton<GlobalData>.Instance.GetText("PRODUCE") + "X" + prop.FabricateNumber.ToString();
			}
			else
			{
				this.FabricateButton.interactable = false;
				this.FabricateButtonName.text = Singleton<GlobalData>.Instance.GetText("NUM_FULL");
			}
			this.FabricateButtonCost.SetActive(false);
			this.FabricateButton.gameObject.SetActive(true);
			this.CostValue.text = prop.FabricatePrice.ToString();
			this.CostRoot.gameObject.SetActive(true);
			this.TimeRoot.gameObject.SetActive(false);
			break;
		}
		this.ShowSelectPositionPart(true);
	}

	public void SelectProp(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (index == this.SelectIndex || this.IsAction)
		{
			return;
		}
		this.IsAction = true;
		this.SelectImage.position = this.PropChildren[this.SelectIndex].transform.position;
		this.PropChildren[this.SelectIndex].SelectImage.gameObject.SetActive(false);
		this.SelectImage.gameObject.SetActive(true);
		this.SelectIndex = index;
		this.SelectImage.DOMoveX(this.PropChildren[index].transform.position.x, 0.3f, false).OnComplete(delegate
		{
			this.IsAction = false;
			this.SelectImage.gameObject.SetActive(false);
			PropData propData = this.PropDataList[this.SelectIndex];
			if (propData.isNew)
			{
				PropDataManager.RemoveNewTag(propData.ID);
				this.count--;
				FunctionPage.Instance.FunctionButtons[3].Tip.gameObject.SetActive(this.count > 0);
			}
			this.RefreshSelectChlid();
			this.RefreshInfo();
		});
	}

	public void ClickOnEquipButton()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		PropData prop = this.PropDataList[this.SelectIndex];
		if (prop.State == PropState.未解锁)
		{
			if (prop.Count > 0)
			{
				if (this.chooseIndex == 0)
				{
					int prop5 = PlayerDataManager.Player.Prop1;
					PlayerDataManager.Equip(EquipmentPosition.Prop1, prop.ID);
					if (prop.ID == PlayerDataManager.Player.Prop2)
					{
						PlayerDataManager.Equip(EquipmentPosition.Prop2, prop5);
					}
					this.SelectEquipPosition(1);
				}
				else
				{
					this.SelectEquipPosition(2);
					int prop2 = PlayerDataManager.Player.Prop2;
					PlayerDataManager.Equip(EquipmentPosition.Prop2, prop.ID);
					if (prop.ID == PlayerDataManager.Player.Prop1)
					{
						PlayerDataManager.Equip(EquipmentPosition.Prop1, prop2);
					}
				}
				this.ShowSelectPositionPart(true);
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				if (Singleton<GlobalData>.Instance.isDebug)
				{
					PropDataManager.Unlock(prop.ID);
				}
				else
				{
					Singleton<UiManager>.Instance.ShowPage(PageName.LackOfMeterialPopup, delegate()
					{
						Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Clear();
						for (int i = 0; i < prop.DropID.Length; i++)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Add(prop.DropID[i]);
						}
					});
					GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMeterialPopup.ToString());
				}
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				if (Singleton<GlobalData>.Instance.isDebug)
				{
					PropDataManager.Unlock(prop.ID);
				}
				else
				{
					Singleton<UiManager>.Instance.ShowPage(PageName.LackOfMeterialPopup, delegate()
					{
						Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Clear();
						for (int i = 0; i < prop.DropID.Length; i++)
						{
							Singleton<UiManager>.Instance.GetPage(PageName.LackOfMeterialPopup).GetComponent<LackOfMeterialPopup>().DropWay.Add(prop.DropID[i]);
						}
					});
					GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMeterialPopup.ToString());
				}
			}
		}
		else
		{
			if (this.chooseIndex == 0)
			{
				int prop3 = PlayerDataManager.Player.Prop1;
				PlayerDataManager.Equip(EquipmentPosition.Prop1, prop.ID);
				if (prop.ID == PlayerDataManager.Player.Prop2)
				{
					PlayerDataManager.Equip(EquipmentPosition.Prop2, prop3);
				}
				this.SelectEquipPosition(1);
			}
			else
			{
				this.SelectEquipPosition(2);
				int prop4 = PlayerDataManager.Player.Prop2;
				PlayerDataManager.Equip(EquipmentPosition.Prop2, prop.ID);
				if (prop.ID == PlayerDataManager.Player.Prop1)
				{
					PlayerDataManager.Equip(EquipmentPosition.Prop1, prop4);
				}
			}
			this.ShowSelectPositionPart(true);
		}
		this.RefreshInfo();
		this.RefreshSelectChlid();
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            PropData prop = this.PropDataList[this.SelectIndex];
            PropDataManager.FinishFabricate(prop.ID);
            PropDataManager.CollectProp(prop.ID, prop.FabricateNumber);
            this.CloseEffect();
            this.RefreshInfo();
            this.RefreshSelectChlid();
        }

    }
    public void ClickOnFabricateButton()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		PropData prop = this.PropDataList[this.SelectIndex];
		switch (prop.State)
		{
		case PropState.未解锁:
			return;
		case PropState.待制作:
			if (this.CheckGold(prop))
			{
				PropDataManager.StartFabricate(prop.ID);
				UITick.setItemSec(prop.ID, 1);
				GameLogManager.SendCostLog(1, prop.FabricatePrice, prop.ID, 1);
				GameLogManager.SendPageLog(prop.Name, "Produce");
				this.SetEffectOnclick(this.FabricateButtonName.transform);
			}
			break;
		case PropState.制作中:
			if (Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount > 0)
			{
				Singleton<GlobalData>.Instance.SpeedUpAdvertisementCount--; 
				//Advertisements.Instance.ShowRewardedVideo(OnFinished);
				Ads.ShowReward(() =>
				{
					PropData prop = this.PropDataList[this.SelectIndex];
					PropDataManager.FinishFabricate(prop.ID);
					PropDataManager.CollectProp(prop.ID, prop.FabricateNumber);
					this.CloseEffect();
					this.RefreshInfo();
					this.RefreshSelectChlid();
				});
                    //Singleton<GlobalData>.Instance.ShowAdvertisement(-10, delegate
                    //{
                    //	PropDataManager.FinishFabricate(prop.ID);
                    //	PropDataManager.CollectProp(prop.ID, prop.FabricateNumber);
                    //	this.CloseEffect();
                    //	this.RefreshInfo();
                    //	this.RefreshSelectChlid();
                    //}, null);
            }
			else if (this.CheckDiamond(prop))
			{
				PropDataManager.FinishFabricate(prop.ID);
				PropDataManager.CollectProp(prop.ID, prop.FabricateNumber);
				this.CloseEffect();
			}
			break;
		case PropState.待领取:
			PropDataManager.CollectProp(prop.ID, prop.FabricateNumber);
			break;
		}
		this.RefreshInfo();
		this.RefreshSelectChlid();
	}

	public bool CheckGold(PropData data)
	{
		if (ItemDataManager.GetCurrency(CommonDataType.GOLD) >= data.FabricatePrice)
		{
			ItemDataManager.SetCurrency(CommonDataType.GOLD, -data.FabricatePrice);
			Singleton<UiManager>.Instance.TopBar.Refresh();
			return true;
		}
		int num = data.FabricatePrice - ItemDataManager.GetCurrency(CommonDataType.GOLD);
		Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.GOLD, num);
		GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
		return false;
	}

	public bool CheckDiamond(PropData data)
	{
		int skipDiamond = Singleton<GlobalData>.Instance.GetSkipDiamond(UITick.getItemRemainderSec(data.ID, data.FabricateTime));
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

	public void ShowSelectPositionPart(bool show)
	{
		if (show)
		{
			PropData propData = this.PropDataList[this.SelectIndex];
			PropData propData2 = PropDataManager.GetPropData(PlayerDataManager.Player.Prop1);
			PropData propData3 = PropDataManager.GetPropData(PlayerDataManager.Player.Prop2);
			if (propData2 != null)
			{
				this.PropButtonIcon1.sprite = Singleton<UiManager>.Instance.GetSprite(propData2.Icon);
				this.PropButtonIcon1.gameObject.SetActive(true);
				this.PropButtonNumber1.text = propData2.Count.ToString();
			}
			else
			{
				this.PropButton1.interactable = true;
				this.PropButtonIcon1.gameObject.SetActive(false);
				this.PropButtonNumber1.text = string.Empty;
			}
			if (propData3 != null)
			{
				this.PropButtonIcon2.sprite = Singleton<UiManager>.Instance.GetSprite(propData3.Icon);
				this.PropButtonIcon2.gameObject.SetActive(true);
				this.PropButtonNumber2.text = propData3.Count.ToString();
			}
			else
			{
				this.PropButton2.interactable = true;
				this.PropButtonIcon2.gameObject.SetActive(false);
				this.PropButtonNumber2.text = string.Empty;
			}
			this.SelectPositionPart.SetActive(true);
		}
		else
		{
			this.SelectPositionPart.SetActive(false);
		}
	}

	public void SelectEquipPosition(int pos)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		PropData propData = this.PropDataList[this.SelectIndex];
		if (propData.State == PropState.未解锁 && propData.Count <= 0)
		{
			return;
		}
		if (pos != 1)
		{
			if (pos == 2)
			{
				this.chooseIndex = 1;
				Singleton<GlobalData>.Instance.selectProp = this.chooseIndex;
			}
		}
		else
		{
			this.chooseIndex = 0;
			Singleton<GlobalData>.Instance.selectProp = this.chooseIndex;
			if (null != this.curTeachPage)
			{
				Singleton<GlobalData>.Instance.FirstShouLei = 1;
				this.Buttons.GetComponent<HorizontalLayoutGroup>().enabled = true;
				this.curTeachPage.type = TeachUIType.TopBar;
				this.curTeachPage.RefreshPage();
			}
		}
		this.RefreshInfo();
		this.RefreshChidren();
		this.ShowSelectPositionPart(true);
	}

	public void OnLayerMove()
	{
		if (this.PropRoot.GetComponent<RectTransform>().sizeDelta.x < 1470f)
		{
			this.LeftMark.gameObject.SetActive(false);
			this.RightMark.gameObject.SetActive(false);
		}
		else
		{
			if (this.PropRoot.localPosition.x < -830f)
			{
				this.LeftMark.gameObject.SetActive(true);
			}
			else
			{
				this.LeftMark.gameObject.SetActive(false);
			}
			if (this.RootWidth - 640f + this.PropRoot.localPosition.x > 190f)
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
			this.FabricateButtonCostValue.text = Singleton<GlobalData>.Instance.GetSkipDiamond((int)this.CountDown).ToString();
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
				PropDataManager.FinishFabricate(this.PropDataList[this.SelectIndex].ID);
				PropDataManager.CollectProp(this.PropDataList[this.SelectIndex].ID, this.PropDataList[this.SelectIndex].FabricateNumber);
				this.RefreshInfo();
			}
		}
	}

	public Transform InfoLayer;

	public Transform PropLayer;

	public Image PropIcon;

	public Image[] ChooseImgs;

	public Text PropCount;

	public Text PropName;

	public Text PropDescribe;

	public Text EffectName;

	public Text EffectValue;

	public GameObject TimeRoot;

	public Text TimeValue;

	public GameObject CostRoot;

	public Text CostValue;

	public Button EquipButton;

	public Text EquipButtonName;

	public Button FabricateButton;

	public Text FabricateButtonName;

	public GameObject FabricateButtonCost;

	public Text FabricateButtonCostValue;

	public GameObject SelectPositionPart;

	public Button PropButton1;

	public Image PropButtonIcon1;

	public Text PropButtonNumber1;

	public Button PropButton2;

	public Image PropButtonIcon2;

	public Text PropButtonNumber2;

	public Button PropButton3;

	public Image PropButtonIcon3;

	public Text PropButtonNumber3;

	public GameObject ProduceADImageGo;

	public Transform PropRoot;

	public Transform SelectImage;

	public Transform LeftMark;

	public Transform RightMark;

	public Transform RightAnchor;

	public Transform DownAnchor;

	public GameObject Buttons;

	private int count;

	public GameObject curEffect;

	public DOTweenAnimation effectAni;

	public Text PosChooseEquipTxt;

	public Transform ContentPos;

	public Slider DebrisSlider;

	public Text DebrisTxt;

	public Text DebrisNameTxt;

	private UITeachPage curTeachPage;

	private List<PropInfoChild> PropChildren = new List<PropInfoChild>();

	private List<PropData> PropDataList = new List<PropData>();

	private int SelectIndex;

	private int ProductionLine;

	private bool IsAction;

	private float CountDown;

	private float RootWidth;

	private int chooseIndex;

	private bool startEffect;
}
