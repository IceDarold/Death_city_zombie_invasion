using System;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class TopBarPage : GamePage
{
	public override void Show()
	{
		base.transform.SetAsFirstSibling();
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	public override void Refresh()
	{
		this.BackButtonText.text = Singleton<GlobalData>.Instance.GetText("BACK");
		Singleton<FontChanger>.Instance.SetFont(BackButtonText);
		this.GoldText.text = ItemDataManager.GetCurrency(CommonDataType.GOLD).ToString();
		this.DiamondText.text = ItemDataManager.GetCurrency(CommonDataType.DIAMOND).ToString();
		this.DnaText.text = ItemDataManager.GetCurrency(CommonDataType.DNA).ToString();
		this.PlayerLevel.text = PlayerDataManager.Player.Level.ToString();
		this.PlayerExp.fillAmount = (float)PlayerDataManager.Player.Experience / (float)PlayerDataManager.GetLevelUpExp();
		this.SetEnergy(Singleton<GlobalData>.Instance.GetEnergy());
		this.isTiming = (Singleton<GlobalData>.Instance.GetEnergy() < Singleton<GlobalData>.Instance.GetMaxEnergy());
		this.timer = Singleton<GlobalData>.Instance.GetEnergyRecoveryTime();
	}

	public void OnBackClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.OnBack();
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.TopBar)
		{
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().Button.SetActive(false);
			Singleton<UiManager>.Instance.CurrentPage.OnBack();
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type = TeachUIType.Main;
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().RefreshPage();
		}
		else if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.WeaponUpgradeBack)
		{
			Singleton<UiManager>.Instance.CurrentPage.OnBack();
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().Button.SetActive(false);
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type = TeachUIType.Map;
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().RefreshPage();
		}
	}

	public void OnPlayerInfoClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(PageName.PlayerInfoPage.ToString(), "null");
		Singleton<UiManager>.Instance.ShowPage(PageName.PlayerInfoPage, null);
	}

	public void OnCurrencyClick()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (Singleton<UiManager>.Instance.CurrentPage.Name == PageName.StorePage)
		{
			return;
		}
		GameLogManager.SendPageLog(PageName.StorePage.ToString(), "null");
		Singleton<UiManager>.Instance.CurrentPage.Hide();
		Singleton<UiManager>.Instance.ShowStorePage(0);
		GameLogManager.SendPageLog(PageName.StorePage.ToString(), "null");
	}

	public void ClickOnEnergy()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(PageName.EnergyShortagePage.ToString(), "null");
		Singleton<UiManager>.Instance.ShowPage(PageName.EnergyShortagePage, null);
	}

	private void SetEnergy(int count)
	{
		this.EnergyCount.text = count.ToString();
		float fillAmount = (float)count / (float)Singleton<GlobalData>.Instance.GetMaxEnergy();
		this.EnergyProgress.fillAmount = fillAmount;
	}

	private void Update()
	{
		if (this.isTiming)
		{
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
			}
			else
			{
				this.Refresh();
			}
		}
	}

	public Button BackButton;

	public Button EnergyButton;

	public Button PlayerInfoButton;

	public Button GoldButton;

	public Button DiamondButton;

	public Button DNAButton;

	public Text BackButtonText;

	public Text EnergyCount;

	public Text PlayerLevel;

	public Text GoldText;

	public Text DiamondText;

	public Text DnaText;

	public Image PlayerExp;

	public Transform DiamondRoot;

	public Transform GoldRoot;

	public Transform DNARoot;

	public Image EnergyProgress;

	private float timer;

	private bool isTiming;
}
