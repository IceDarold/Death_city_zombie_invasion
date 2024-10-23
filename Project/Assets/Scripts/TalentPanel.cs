using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class TalentPanel : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.Layers.Count; i++)
		{
			for (int j = 0; j < this.Layers[i].Skills.Count; j++)
			{
				int rank = i;
				int tag = j + 1;
				this.Layers[i].Skills[j].gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SelectTalent(rank + 1, tag);
				});
			}
		}
	}

	private void OnEnable()
	{
		this.CloseEffect();
		this.TalentTreeID = 1;
		this.CurrentTalent = TalentDataManager.GetTalentData(this.TalentTreeID, 1, 1);
		this.ResetButtonName.text = Singleton<GlobalData>.Instance.GetText("RESET");
		Singleton<FontChanger>.Instance.SetFont(ResetButtonName);
		this.RefreshButtons();
		this.RefreshInfo();
		this.TanlentPart.localPosition = new Vector3(450f, -20f, 0f);
		this.TanlentPart.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.InfoRoot.localPosition = new Vector3(-400f, -40f, 0f);
		this.InfoRoot.DOLocalMove(this.LeftAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo).OnComplete(delegate
		{
			if (Singleton<GlobalData>.Instance.FirstTalent == 1)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.BootomPos.gameObject.SetActive(false);
				this.curTeachPage.EffectObj.gameObject.SetActive(true);
				this.curTeachPage.type = TeachUIType.Talent;
				this.curTeachPage.RefreshPage();
				if (Singleton<GlobalData>.Instance.FirstTalent == 1)
				{
					this.curTeachPage.Button = this.curTeachPage.ProduceGo(this.UpgradeButton.gameObject);
					this.curTeachPage.BootomPos.gameObject.SetActive(true);
					this.curTeachPage.Button.SetActive(false);
					this.curTeachPage.EffectObj.gameObject.SetActive(false);
					this.curTeachPage.BootomPos.position = this.Layers[0].Skills[0].transform.position;
					TalentData talentData = TalentDataManager.GetTalentData(this.TalentTreeID, 1, 1);
					this.CurrentTalent = talentData;
					this.curTeachPage.curTalent.Refresh(talentData, true);
					this.RefreshButtons();
				}
			}
			else
			{
				this.curTeachPage = null;
			}
		});
	}

	public void RefreshButtons()
	{
		for (int i = 0; i < this.Layers.Count; i++)
		{
			this.Layers[i].LayerName.text = Singleton<GlobalData>.Instance.GetText(this.LayerName[i]);
			Singleton<FontChanger>.Instance.SetFont(Layers[i].LayerName);
			Singleton<FontChanger>.Instance.SetFont(Layers[i].LayerInfo);
			if (TalentDataManager.isTierUnlocked(this.TalentTreeID, i + 1))
			{
				this.Layers[i].LayerInfo.text = TalentDataManager.GetCurrentPoints(this.TalentTreeID, i + 1) + "/" + TalentDataManager.GetTotalPoints(this.TalentTreeID, i + 1);
				this.Layers[i].LayerInfo.color = Color.white;
			}
			else
			{
				this.Layers[i].LayerInfo.text = string.Concat(new object[]
				{
					Singleton<GlobalData>.Instance.GetText("NEED_TANLET_POINT"),
					"(",
					TalentDataManager.GetCurrentPoints(this.TalentTreeID, 0),
					"/",
					TalentDataManager.GetUnlockNeedPoints(this.TalentTreeID, i + 1),
					")"
				});
				this.Layers[i].LayerInfo.color = Color.red;
			}
			for (int j = 0; j < this.Layers[i].Skills.Count; j++)
			{
				int num = i;
				int index = j + 1;
				TalentData talentData = TalentDataManager.GetTalentData(this.TalentTreeID, num + 1, index);
				this.Layers[i].Skills[j].Refresh(talentData, talentData.ID == this.CurrentTalent.ID);
			}
		}
	}

	private void RefreshInfo()
	{
		if (this.CurrentTalent != null)
		{
			this.TalentName.text = Singleton<GlobalData>.Instance.GetText(this.CurrentTalent.Name);
			this.TalentDescribe.text = Singleton<GlobalData>.Instance.GetText(this.CurrentTalent.Describe);
			this.TanletIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.CurrentTalent.Icon);
			this.TanletIcon.SetNativeSize();
			this.TalentLevel.text = Singleton<GlobalData>.Instance.GetText("LEVEL") + this.CurrentTalent.Level.ToString();
			this.AttributeName.text = Singleton<GlobalData>.Instance.GetText(this.CurrentTalent.Type.ToString());
			this.AttributeSlider.maxValue = (float)this.CurrentTalent.Value[this.CurrentTalent.MaxLevel];
			this.AttributeSlider.value = (float)this.CurrentTalent.Value[this.CurrentTalent.Level];
			this.AttributeName.text = Singleton<GlobalData>.Instance.GetText(this.CurrentTalent.Name);
			this.AttributeValue.text = "+" + (float)this.CurrentTalent.Value[this.CurrentTalent.Level] * 0.01f + "%";
			if (this.CurrentTalent.Unlock)
			{
				this.UpgradeButton.gameObject.SetActive(true);
				if (this.CurrentTalent.Level < this.CurrentTalent.MaxLevel)
				{
					this.UpgradeName.text = Singleton<GlobalData>.Instance.GetText("UPGRADE");
					this.UpgradeButton.interactable = true;
					this.UpgradeCostObject.SetActive(true);
					this.UpgrageCostValue.text = TalentDataManager.GetUpgradeCost(this.TalentTreeID).ToString();
					FunctionPage.Instance.CheckTip(3);
				}
				else
				{
					this.UpgradeName.text = Singleton<GlobalData>.Instance.GetText("LEVELMAX");
					this.UpgradeButton.interactable = false;
					this.UpgradeCostObject.SetActive(false);
				}
			}
			else
			{
				this.UpgradeButton.gameObject.SetActive(false);
				this.UpgradeCostObject.SetActive(false);
			}
			Singleton<FontChanger>.Instance.SetFont(TalentName);
			Singleton<FontChanger>.Instance.SetFont(TalentDescribe);
			Singleton<FontChanger>.Instance.SetFont(TalentLevel);
			Singleton<FontChanger>.Instance.SetFont(AttributeName);
			Singleton<FontChanger>.Instance.SetFont(UpgradeName);
		}
	}

	private void SelectTalent(int tier, int tag)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.CurrentTalent = TalentDataManager.GetTalentData(this.TalentTreeID, tier, tag);
		this.RefreshInfo();
		this.RefreshButtons();
	}

	public void CloseEffect()
	{
		this.curEffect.SetActive(false);
	}

	public void ClickOnUpgrade()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.TalentUpClip, false);
		if (null != this.curTeachPage && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).gameObject.activeInHierarchy)
		{
			Singleton<GlobalData>.Instance.FirstTalent = 0;
			this.curTeachPage.Button.SetActive(false);
			UnityEngine.Object.Destroy(this.curTeachPage.Button);
			Singleton<UiManager>.Instance.SetTopEnable(true, true);
			this.curTeachPage.Close();
			TalentDataManager.Upgrade(this.CurrentTalent.ID);
			Singleton<UiManager>.Instance.TopBar.Refresh();
			this.RefreshInfo();
			this.RefreshButtons();
		}
		else if (this.CurrentTalent.Unlock && this.CurrentTalent.Level <= this.CurrentTalent.MaxLevel)
		{
			if (ItemDataManager.GetCurrency(CommonDataType.DNA) >= TalentDataManager.GetUpgradeCost(this.TalentTreeID))
			{
				this.curEffect.SetActive(true);
				this.effectAni.DORestart(false);
				this.effectAni.DORestartById("2");
				GameLogManager.SendCostLog(3, TalentDataManager.GetUpgradeCost(this.TalentTreeID), 3, 2);
				GameLogManager.SendPageLog("TalentTree", "Upgrade");
				ItemDataManager.SetCurrency(CommonDataType.DNA, -TalentDataManager.GetUpgradeCost(this.TalentTreeID));
				TalentDataManager.Upgrade(this.CurrentTalent.ID);
				Singleton<UiManager>.Instance.TopBar.Refresh();
				this.RefreshInfo();
				this.RefreshButtons();
			}
			else
			{
				Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DNA, TalentDataManager.GetUpgradeCost(this.CurrentTalent.ID) - ItemDataManager.GetCurrency(CommonDataType.DNA));
				GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.LackOfMoneyPopup.ToString());
			}
		}
	}

	public void ClickOnReset()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPayPromot(CommonDataType.DIAMOND, 100, Singleton<GlobalData>.Instance.GetText("RESET"), delegate
		{
			this.SuccessReset();
		});
		GameLogManager.SendPageLog(PageName.FunctionPage.ToString(), PageName.PayPromptPopup.ToString());
	}

	public void SuccessReset()
	{
		TalentDataManager.Reset(this.TalentTreeID);
		this.CurrentTalent = TalentDataManager.GetTalentData(this.TalentTreeID, 1, 1);
		this.RefreshInfo();
		this.RefreshButtons();
	}

	public Transform InfoRoot;

	public Transform TanlentPart;

	public Text TalentName;

	public Text TalentDescribe;

	public Text TalentLevel;

	public Image TanletIcon;

	public Text AttributeName;

	public Text AttributeValue;

	public Slider AttributeSlider;

	public Text ResetButtonName;

	public GameObject UpgradeCostObject;

	public Text UpgrageCostValue;

	public Button UpgradeButton;

	public Text UpgradeName;

	public List<TalentLayer> Layers = new List<TalentLayer>();

	private string[] LayerName = new string[]
	{
		"LAYER_01",
		"LAYER_02",
		"LAYER_03",
		"LAYER_04",
		"LAYER_05"
	};

	private int TalentTreeID;

	private TalentData CurrentTalent;

	private UITeachPage curTeachPage;

	public Transform LeftAnchor;

	public Transform RightAnchor;

	public GameObject curEffect;

	public DOTweenAnimation effectAni;
}
