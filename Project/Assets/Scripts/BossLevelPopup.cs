using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelPopup : GamePage
{
	public override void Show()
	{
		this.data = CheckpointDataManager.GetBossCheckpoint(MapsPage.instance.PageIndex);
		int @int = PlayerPrefs.GetInt("BOSS_CHECKPOINT_" + MapsPage.instance.PageIndex);
		this.price = ((@int != 0) ? (10 * (int)Mathf.Pow(2f, (float)(@int - 1))) : 0);
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.4f);
	}

	public override void Refresh()
	{
		this.TitleText.text = Singleton<GlobalData>.Instance.GetText("BOSSLEVEL");
		this.DescribeText.text = Singleton<GlobalData>.Instance.GetText("BOSS_DES");
		this.RecommendText.text = Singleton<GlobalData>.Instance.GetText("POWER_RECOMMEND") + " : " + this.data.RequireFighting;
		this.CurrentFighting.text = Singleton<GlobalData>.Instance.GetText("POWER_SELF") + " : " + PlayerDataManager.GetCurrentFighting();
		Singleton<FontChanger>.Instance.SetFont(TitleText);
		Singleton<FontChanger>.Instance.SetFont(DescribeText);
		Singleton<FontChanger>.Instance.SetFont(RecommendText);
		Singleton<FontChanger>.Instance.SetFont(CurrentFighting);
		this.ShowAwards();
		this.ShowBossModel(MapsPage.instance.PageIndex);
		this.FreeText.text = Singleton<GlobalData>.Instance.GetText("CHALLENGE");
		Singleton<FontChanger>.Instance.SetFont(FreeText);
		if (this.price == 0)
		{
			this.PriceObject.SetActive(false);
		}
		else
		{
			this.PriceObject.SetActive(true);
			this.PriceText.text = this.price.ToString();
		}
	}

	private void ShowAwards()
	{
		List<BossExtraAward> bossExtraAwardList = ItemDataManager.GetBossExtraAwardList(this.data.ID);
		for (int i = 0; i < this.AwardObjects.Length; i++)
		{
			if (i < bossExtraAwardList.Count)
			{
				this.AwardObjects[i].SetActive(true);
				this.AwardNames[i].text = Singleton<GlobalData>.Instance.GetText(ItemDataManager.GetItemData(bossExtraAwardList[i].AwardID).Name);
				Singleton<FontChanger>.Instance.SetFont(AwardNames[i]);
				if (bossExtraAwardList[i].AwardID == 1)
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.SpecialIcons[0];
				}
				else if (bossExtraAwardList[i].AwardID == 2)
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.SpecialIcons[1];
				}
				else if (bossExtraAwardList[i].AwardID == 3)
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.SpecialIcons[2];
				}
				else if (bossExtraAwardList[i].AwardID >= 8200 && bossExtraAwardList[i].AwardID < 8300)
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.GetSprite("debris_weapon");
				}
				else if (bossExtraAwardList[i].AwardID >= 8400 && bossExtraAwardList[i].AwardID < 8500)
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.GetSprite("debris_prop");
				}
				else
				{
					this.AwardImages[i].sprite = Singleton<UiManager>.Instance.GetSprite(ItemDataManager.GetItemData(bossExtraAwardList[i].AwardID).Icon);
				}
				this.AwardImages[i].preserveAspect = true;
			}
			else
			{
				this.AwardObjects[i].SetActive(false);
			}
		}
	}

	private void ShowBossModel(int index)
	{
		int num = this.BossIndex[index];
		for (int i = 0; i < this.BossModels.Length; i++)
		{
			this.BossModels[i].gameObject.SetActive(i == num);
		}
	}

	public void OnclickClose()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.CancelClip, false);
		this.Close();
	}

	public void OnclickChallenge()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.Close();
		if (Singleton<GlobalData>.Instance.GetEnergy() <= 1)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.EnergyShortagePage, null);
			return;
		}
		if (ItemDataManager.GetCurrency(CommonDataType.DIAMOND) < this.price)
		{
			Singleton<UiManager>.Instance.ShowLackOfMoney(CommonDataType.DIAMOND, this.price);
			return;
		}
		CheckpointDataManager.SelectCheckpoint = this.data;
		if (PlayerDataManager.isFightingStrengthShortage(this.data))
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.PowerShortagePage, null);
			return;
		}
		Singleton<GlobalData>.Instance.SetEnergy(-2);
		ItemDataManager.SetCurrency(CommonDataType.DIAMOND, -this.price);
		Singleton<GlobalData>.Instance.AdvertisementReviveTimes = 0;
		MapsPage.instance.isScence = true;
		Singleton<UiManager>.Instance.ShowTopBar(false);
		MapsPage.instance.Close();
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
		Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(this.data.SceneID), PageName.InGamePage);
	}

	public CanvasGroup Content;

	public Text TitleText;

	public Text DescribeText;

	public Text FreeText;

	public Text PriceText;

	public Text RecommendText;

	public Text CurrentFighting;

	public GameObject PriceObject;

	public GameObject[] BossModels;

	public int[] BossIndex;

	public GameObject[] AwardObjects;

	public Image[] AwardImages;

	public Text[] AwardNames;

	private CheckpointData data;

	private int price;
}
