using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassGiftPage : GamePage
{
	public override void Show()
	{
		this.CurrentGift = LevelPassGiftSystem.GetCurrentData();
		base.Show();
		this.Content.alpha = 0f;
		this.Content.DOFade(1f, 0.5f);
	}

	public override void Refresh()
	{
		this.Title.text = Singleton<GlobalData>.Instance.GetText("LEVEL_PASS_AWARD");
		this.Describe.text = Singleton<GlobalData>.Instance.GetText(this.CurrentGift.Describe);
		this.ReciveName.text = Singleton<GlobalData>.Instance.GetText("RECEIVE");
		Singleton<FontChanger>.Instance.SetFont(Title);
		Singleton<FontChanger>.Instance.SetFont(Describe);
		Singleton<FontChanger>.Instance.SetFont(ReciveName);
		for (int i = 0; i < this.GiftChildren.Count; i++)
		{
			if (i < this.CurrentGift.AwardID.Length)
			{
				this.GiftChildren[i].Init(this.CurrentGift.AwardID[i], this.CurrentGift.AwardCount[i]);
				this.GiftChildren[i].gameObject.SetActive(true);
			}
			else
			{
				this.GiftChildren[i].gameObject.SetActive(false);
			}
		}
		CheckpointData maxPassedCheckpoint = CheckpointDataManager.GetMaxPassedCheckpoint();
		if (maxPassedCheckpoint != null)
		{
			if (maxPassedCheckpoint.ID >= this.CurrentGift.RequireLevel)
			{
				this.ReciveButton.interactable = true;
				this.RecieveLight.gameObject.SetActive(true);
			}
			else
			{
				this.ReciveButton.interactable = false;
				this.RecieveLight.gameObject.SetActive(false);
			}
		}
		else
		{
			this.ReciveButton.interactable = false;
			this.RecieveLight.gameObject.SetActive(false);
		}
	}

	public void ClickOnReceive()
	{
		this.Close();
		int golds = 0;
		int diamods = 0;
		int dnas = 0;
		for (int i = 0; i < this.CurrentGift.AwardID.Length; i++)
		{
			if (this.CurrentGift.AwardID[i] == 1)
			{
				golds = this.CurrentGift.AwardCount[i];
			}
			else if (this.CurrentGift.AwardID[i] == 2)
			{
				diamods = this.CurrentGift.AwardCount[i];
			}
			else if (this.CurrentGift.AwardID[i] == 3)
			{
				dnas = this.CurrentGift.AwardCount[i];
			}
			else if (this.CurrentGift.AwardID[i] >= 8200 && this.CurrentGift.AwardID[i] < 8300)
			{
				DebrisData debrisData = DebrisDataManager.GetDebrisData(this.CurrentGift.AwardID[i]);
				WeaponData weaponData = WeaponDataManager.GetWeaponData(debrisData.ItemID);
			}
			ItemDataManager.CollectItem(this.CurrentGift.AwardID[i], this.CurrentGift.AwardCount[i]);
		}
		LevelPassGiftSystem.SetReceived(this.CurrentGift.ID);
		Singleton<UiManager>.Instance.TopBar.Refresh();
		Singleton<UiManager>.Instance.ShowEarnMoneyEffect(golds, diamods, dnas);
		Singleton<UiManager>.Instance.CurrentPage.Refresh();
	}

	public CanvasGroup Content;

	public Text Title;

	public Text Describe;

	public Text ReciveName;

	public Button ReciveButton;

	public Image RecieveLight;

	public List<LevelPassGiftChild> GiftChildren;

	private LevelPassGiftData CurrentGift;

	private bool isAlreadyUnlock;
}
