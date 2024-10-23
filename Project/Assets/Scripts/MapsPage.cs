using System;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapsPage : GamePage, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public new void Awake()
	{
		MapsPage.instance = this;
		this.MapWidth = (float)Screen.width;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.IsAction)
		{
			return;
		}
		this.TouchPosition = eventData.position;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.IsAction)
		{
			return;
		}
		if (eventData.position.x - this.TouchPosition.x > this.MapWidth / 8f)
		{
			this.ChangeChapter(1);
		}
		else if (eventData.position.x - this.TouchPosition.x < -this.MapWidth / 8f)
		{
			this.ChangeChapter(-1);
		}
	}

	private void InitMaps()
	{
		for (int i = 0; i < this.MaxChapter; i++)
		{
			this.Maps[i].transform.localPosition = new Vector3(0f, 0f, 0f);
			this.MapChapters[i].transform.localPosition = new Vector3(0f, 0f, 0f);
			if (i == this.PageIndex)
			{
				this.Maps[i].gameObject.SetActive(true);
				this.MapChapters[i].gameObject.SetActive(true);
				this.MapChapters[i].Refresh();
				this.Maps[i].DOColor(new Color(1f, 1f, 1f, 0f), 0.2f).From<Tweener>();
			}
			else
			{
				this.Maps[i].gameObject.SetActive(false);
				this.MapChapters[i].gameObject.SetActive(false);
			}
		}
	}

	public void ChangeChapter(int dir)
	{
		if (this.IsAction)
		{
			return;
		}
		if (dir < 0)
		{
			if (this.PageIndex < this.MaxChapter - 1)
			{
				this.IsAction = true;
				this.HideSpecialCheckpoints();
				this.DownloadPart.gameObject.SetActive(false);
				this.Maps[this.PageIndex].gameObject.SetActive(false);
				this.MapChapters[this.PageIndex].gameObject.SetActive(false);
				this.PageIndex++;
				this.MapChapters[this.PageIndex].transform.localPosition = Vector3.zero;
				this.MapChapters[this.PageIndex].Refresh();
				this.MapChapters[this.PageIndex].gameObject.SetActive(true);
				this.MapChapters[this.PageIndex].transform.DOLocalMoveX(200f, 0.4f, false).From<Tweener>();
				this.MapChapters[this.PageIndex].GetComponent<CanvasGroup>().DOFade(0.5f, 0.5f).From<Tweener>();
				this.Maps[this.PageIndex].transform.localPosition = Vector3.zero;
				this.Maps[this.PageIndex].gameObject.SetActive(true);
				this.Maps[this.PageIndex].transform.DOLocalMoveX(200f, 0.4f, false).From<Tweener>();
				this.Maps[this.PageIndex].DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).From<Tweener>().OnComplete(delegate
				{
					this.IsAction = false;
					this.RefreshPage();
				});
			}
		}
		else if (dir > 0)
		{
			if (this.PageIndex > 0)
			{
				this.IsAction = true;
				this.HideSpecialCheckpoints();
				this.DownloadPart.gameObject.SetActive(false);
				this.Maps[this.PageIndex].gameObject.SetActive(false);
				this.MapChapters[this.PageIndex].gameObject.SetActive(false);
				this.PageIndex--;
				this.MapChapters[this.PageIndex].transform.localPosition = Vector3.zero;
				this.MapChapters[this.PageIndex].gameObject.SetActive(true);
				this.MapChapters[this.PageIndex].Refresh();
				this.MapChapters[this.PageIndex].transform.DOLocalMoveX(-200f, 0.4f, false).From<Tweener>();
				this.MapChapters[this.PageIndex].GetComponent<CanvasGroup>().DOFade(0.5f, 0.5f).From<Tweener>();
				this.Maps[this.PageIndex].transform.localPosition = Vector3.zero;
				this.Maps[this.PageIndex].gameObject.SetActive(true);
				this.Maps[this.PageIndex].transform.DOLocalMoveX(-200f, 0.4f, false).From<Tweener>();
				this.Maps[this.PageIndex].DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).From<Tweener>().OnComplete(delegate
				{
					this.IsAction = false;
					this.RefreshPage();
				});
			}
		}
		else
		{
			this.Maps[this.PageIndex].transform.DOLocalMoveX(0f, 0.2f, false);
			this.MapChapters[this.PageIndex].transform.DOLocalMoveX(0f, 0.2f, false).OnComplete(delegate
			{
				this.IsAction = false;
				this.RefreshPage();
			});
		}
	}

	public new void OnEnable()
	{
		Ads.ShowInter();
        //Advertisements.Instance.ShowInterstitial();
		//Singleton<GlobalData>.Instance.ShowAdvertisement(15, null, null);
		this.SelectData = CheckpointDataManager.GetCurrentCheckpoint();
		this.PageIndex = this.SelectData.Chapters - ChapterEnum.CHAPTERNAME_01;
		this.RandomData = CheckpointDataManager.GetRandomCheckpoint();
		WeaponDataManager.SetTryWeapon();
		this.InitMaps();
		this.RefreshPage();
		this.RefreshInfoPart();
		this.PassButton.gameObject.SetActive(Singleton<GlobalData>.Instance.IngoreChange);
		this.isScence = false;
		if (CheckpointDataManager.GetCurrentCheckpoint().ID == 4 && !PlayerPrefs.GetString("FACEBOOK_NOTICE_IN_MAP").Equals("true"))
		{
			PlayerPrefs.SetString("FACEBOOK_NOTICE_IN_MAP", "true");
			if (!PlayerPrefs.GetString("FACEBOOK_NOTICE_RESULT").Equals("SUCCESS") && Singleton<GlobalData>.Instance.FacebookNoticeCounts > 0)
			{
				Singleton<UiManager>.Instance.ShowFacebookGroupPage(8);
			}
		}
	}

	public override void Refresh()
	{
		base.Refresh();
		this.RefreshPage();
	}

	public void RefreshChapterTag()
	{
		this.titleNameTxt.text = Singleton<GlobalData>.Instance.GetText((this.PageIndex + ChapterEnum.CHAPTERNAME_01).ToString());
		Singleton<FontChanger>.Instance.SetFont(titleNameTxt);
		for (int i = 0; i < this.ChapterTags.Length; i++)
		{
			this.ChapterTags[i].interactable = (this.PageIndex == i);
		}
	}

	public void OnclickNoEnough(int num)
	{
		this.OnclickWeapon(num);
	}

	public bool CheckTeach(PlayerData player)
	{
		if (CheckpointDataManager.GetCurrentCheckpoint().Chapters != ChapterEnum.CHAPTERNAME_01 || CheckpointDataManager.GetCurrentCheckpoint().Index != 5)
		{
			return false;
		}
		WeaponData weaponData = WeaponDataManager.GetWeaponData(player.Weapon1);
		if (player.Weapon2 != 0)
		{
			WeaponData weaponData2 = WeaponDataManager.GetWeaponData(player.Weapon2);
			return WeaponPartSystem.GetWaponPartData(player.Weapon1, WeaponPartEnum.BULLET).Level == 1 && WeaponPartSystem.GetWaponPartData(player.Weapon2, WeaponPartEnum.BULLET).Level == 1 && weaponData.State == WeaponState.待升级 && weaponData2.State == WeaponState.待升级;
		}
		return WeaponPartSystem.GetWaponPartData(player.Weapon1, WeaponPartEnum.BULLET).Level == 1;
	}

	public void CheckTeachLevel()
	{
		if (CheckpointDataManager.GetCurrentCheckpointIndex() == 11 && Singleton<GlobalData>.Instance.FirstSpecial == 1)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
			UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			component.type = TeachUIType.MapsSpecialLevel;
			component.Button = component.ProduceGo(this.RandomCheckpoint.gameObject);
			component.RefreshPage();
		}
	}

	public void CheckFight(PlayerData player)
	{
		int requireFighting = this.SelectData.RequireFighting;
		WeaponData weaponData = WeaponDataManager.GetWeaponData(player.Weapon1);
		WeaponData weaponData2 = WeaponDataManager.GetWeaponData(player.Weapon2);
		int num = (weaponData != null) ? WeaponDataManager.GetCurrentFightingStrength(weaponData) : 0;
		int num2 = (weaponData2 != null) ? WeaponDataManager.GetCurrentFightingStrength(weaponData2) : 0;
		if (num > num2)
		{
			if (requireFighting > num)
			{
				this.MainFight.SetActive(true);
				if ((float)requireFighting * 0.7f > (float)num)
				{
					this.BgMainColorImg.color = this.WarnAndDangerColor[1];
					this.MainTipColorImg.color = this.WarnAndDangerColor[1];
				}
				else
				{
					this.BgMainColorImg.color = this.WarnAndDangerColor[0];
					this.MainTipColorImg.color = this.WarnAndDangerColor[0];
				}
				if (WeaponDataManager.GetWeaponData(player.Weapon1).State == WeaponState.已满级)
				{
					this.MainFightTxt.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER_CHANGE");
				}
				else
				{
					this.MainFightTxt.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER_NOENOUGH");
				}
				Singleton<FontChanger>.Instance.SetFont(MainFightTxt);
				this.MainWarning.SetActive(true);
				this.SecondFight.SetActive(false);
				if (this.CheckTeach(player))
				{
					this.MainWarning.SetActive(false);
					Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, delegate()
					{
						UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
						component.type = TeachUIType.MapUpgrade;
						component.Button = component.ProduceGo(this.MainWeaponGo);
						component.RefreshPage();
					});
				}
			}
			else
			{
				this.MainFight.SetActive(false);
				this.SecondFight.SetActive(false);
			}
		}
		else if (requireFighting > num2)
		{
			this.MainFight.SetActive(false);
			if (WeaponDataManager.GetWeaponData(player.Weapon2).State == WeaponState.已满级)
			{
				this.SecondFightTxt.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER_CHANGE");
			}
			else
			{
				this.SecondFightTxt.text = Singleton<GlobalData>.Instance.GetText("FIREPOWER_NOENOUGH");
			}
			Singleton<FontChanger>.Instance.SetFont(SecondFightTxt);
			this.SecondFight.SetActive(true);
			this.SecondWarning.SetActive(true);
			if ((float)requireFighting * 0.7f > (float)num)
			{
				this.BgSecondColorImg.color = this.WarnAndDangerColor[1];
				this.SecTipColorImg.color = this.WarnAndDangerColor[1];
			}
			else
			{
				this.BgSecondColorImg.color = this.WarnAndDangerColor[0];
				this.SecTipColorImg.color = this.WarnAndDangerColor[0];
			}
			if (this.CheckTeach(player))
			{
				this.SecondWarning.SetActive(false);
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, delegate()
				{
					UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
					component.type = TeachUIType.MapUpgrade;
					component.Button = component.ProduceGo(this.SecondWeaponGo);
					component.RefreshPage();
				});
			}
		}
		else
		{
			this.MainFight.SetActive(false);
			this.SecondFight.SetActive(false);
		}
	}

	public void OnclickPass()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		CheckpointDataManager.SetCheckpointPassed(this.SelectData);
		if (!CheckpointDataManager.GetCurrentCheckpoint().Passed)
		{
			this.SelectData = CheckpointDataManager.GetCurrentCheckpoint();
		}
		if (this.SelectData.Chapters > this.PageIndex + ChapterEnum.CHAPTERNAME_01)
		{
			this.ChangeChapter(-1);
		}
		else
		{
			this.MapChapters[this.PageIndex].Refresh();
		}
		this.RefreshInfoPart();
		this.RefreshPage();
	}

	private void HideSpecialCheckpoints()
	{
		this.BossCheckpoint.gameObject.SetActive(false);
		this.GoldCheckpoint.gameObject.SetActive(false);
		this.RandomCheckpoint.gameObject.SetActive(false);
		this.WeaponCheckpoint.gameObject.SetActive(false);
		this.SniperCheckpoint.gameObject.SetActive(false);
	}

	private void RefreshSpecialCheckpoints()
	{
		CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
		if (this.PageIndex + ChapterEnum.CHAPTERNAME_01 <= currentCheckpoint.Chapters)
		{
			if (currentCheckpoint.ID > 5)
			{
				this.BossCheckpoint.transform.position = this.MapChapters[this.PageIndex].BossPoint.position;
				this.BossCheckpoint.gameObject.SetActive(true);
			}
			else
			{
				this.BossCheckpoint.gameObject.SetActive(false);
			}
		}
		else
		{
			this.BossCheckpoint.gameObject.SetActive(false);
		}
		if (currentCheckpoint.Chapters == this.PageIndex + ChapterEnum.CHAPTERNAME_01)
		{
			if (currentCheckpoint.ID > 3)
			{
				this.GoldCheckpoint.transform.position = this.MapChapters[this.PageIndex].GoldPoint.position;
				this.GoldCheckpoint.gameObject.SetActive(true);
			}
			else
			{
				this.GoldCheckpoint.gameObject.SetActive(false);
			}
			if (currentCheckpoint.ID > 10)
			{
				this.RandomCheckpoint.transform.position = this.MapChapters[this.PageIndex].RandomPoint.position;
				this.RandomCheckpoint.gameObject.SetActive(true);
			}
			else
			{
				this.RandomCheckpoint.gameObject.SetActive(false);
			}
			if (currentCheckpoint.ID > 4)
			{
				this.SniperCheckpoint.transform.position = this.MapChapters[this.PageIndex].SniperPoint.position;
				this.SniperCheckpoint.gameObject.SetActive(true);
				if (PlayerPrefs.GetString("InitSnipeTeach", "true").Equals("true"))
				{
					PlayerPrefs.SetString("InitSnipeTeach", "false");
					GameLogManager.SendSnipeLog(1);
					WeaponDataManager.CollectWeapon(2101);
					this.SelectData = CheckpointDataManager.GetSnipeCheckpoint();
					this.RefreshInfoPart();
					this.SnipeTeachDescribe.text = Singleton<GlobalData>.Instance.GetText("SNIPE_TEACH_DESCRIBE");
					Singleton<FontChanger>.Instance.SetFont(SnipeTeachDescribe);
					this.SnipeTeachDescribe.transform.localPosition = this.MapChapters[this.PageIndex].SniperPoint.localPosition + new Vector3(0f, -80f, 0f);
					this.SnipeTeachPart.gameObject.SetActive(true);
					this.SniperCheckpoint.transform.SetAsLastSibling();
					this.SelectEffect.gameObject.SetActive(false);
					Singleton<UiManager>.Instance.SetTopEnable(false, false);
					Singleton<UiManager>.Instance.CanBack = false;
				}
			}
			else
			{
				this.SniperCheckpoint.gameObject.SetActive(false);
			}
			WeaponData tryWeapon = WeaponDataManager.GetTryWeapon();
			if (currentCheckpoint.ID > 7 && tryWeapon != null)
			{
				this.WeaponCheckpoint.transform.position = this.MapChapters[this.PageIndex].WeaponPoint.position;
				this.WeaponCheckpoint.gameObject.SetActive(true);
			}
			else
			{
				this.WeaponCheckpoint.gameObject.SetActive(false);
			}
		}
		else
		{
			this.GoldCheckpoint.gameObject.SetActive(false);
			this.RandomCheckpoint.gameObject.SetActive(false);
			this.WeaponCheckpoint.gameObject.SetActive(false);
			this.SniperCheckpoint.gameObject.SetActive(false);
		}
	}

	public void OnclickRandomLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.SelectData = this.RandomData;
		this.SelectEffect.SetActive(false);
		if (Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) != null)
		{
			UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			if (component.type == TeachUIType.MapsSpecialLevel)
			{
				component.type = TeachUIType.Map;
				component.Button.SetActive(false);
				component.GuideTxt.transform.localPosition = new Vector3(488f, -100f, 1000f);
			}
		}
		this.RefreshInfoPart();
		this.MapChapters[this.PageIndex].Refresh();
	}

	public void ClickOnWeaponLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.WeaponLevelPopup, null);
	}

	public void OnclickBossLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.BossLevelPopup, null);
	}

	public void OnclickGoldLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.GoldLevelPopup, null);
	}

	public void ClickOnSniperLevel()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (this.SnipeTeachPart.gameObject.activeSelf)
		{
			this.SnipeTeachPart.gameObject.SetActive(false);
			Singleton<UiManager>.Instance.SetTopEnable(true, true);
			Singleton<UiManager>.Instance.CanBack = true;
		}
		this.SelectData = CheckpointDataManager.GetSnipeCheckpoint();
		this.SelectEffect.SetActive(false);
		this.RefreshInfoPart();
		this.MapChapters[this.PageIndex].Refresh();
	}

	private bool isEquipeSniperRifle()
	{
		WeaponData weaponData = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
		if (weaponData != null && weaponData.Type == WeaponType.SNIPER_RIFLE)
		{
			return true;
		}
		WeaponData weaponData2 = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
		return weaponData2 != null && weaponData2.Type == WeaponType.SNIPER_RIFLE;
	}

	private void initRemindWapon()
	{
		WeaponData maxFightingSnipeRifle = WeaponDataManager.GetMaxFightingSnipeRifle();
		if (maxFightingSnipeRifle != null)
		{
			this.RemindWeaponIcon.sprite = Singleton<UiManager>.Instance.GetSprite(maxFightingSnipeRifle.Icon);
			this.RemindWeaponName.text = Singleton<GlobalData>.Instance.GetText(maxFightingSnipeRifle.Name);
			this.RemindButtonName.text = Singleton<GlobalData>.Instance.GetText("CHANGE");
			Singleton<FontChanger>.Instance.SetFont(RemindWeaponName);
			Singleton<FontChanger>.Instance.SetFont(RemindButtonName);
			this.RemindWeaponFighting.text = WeaponDataManager.GetCurrentFightingStrength(maxFightingSnipeRifle).ToString();
			this.RemindTip.text = Singleton<GlobalData>.Instance.GetText("REMIND_WEAPON_TIP");
			Singleton<FontChanger>.Instance.SetFont(RemindTip);
			this.RemindWeaponPart.gameObject.SetActive(true);
		}
	}

	public void RefreshPage()
	{
		if (LevelPassGiftSystem.GetCurrentData() != null)
		{
			this.LevelPassGiftButton.gameObject.SetActive(true);
			this.LevelPassGiftName.text = Singleton<GlobalData>.Instance.GetText("LEVEL_PASS_AWARD");
			Singleton<FontChanger>.Instance.SetFont(LevelPassGiftName);
			if (CheckpointDataManager.GetMaxPassedCheckpoint().ID >= LevelPassGiftSystem.GetCurrentData().RequireLevel)
			{
				this.LevelPassGiftTag.SetActive(true);
			}
			else
			{
				this.LevelPassGiftTag.SetActive(false);
			}
		}
		else
		{
			this.LevelPassGiftButton.gameObject.SetActive(false);
		}
		this.RefreshChapterTag();
		this.RefreshSpecialCheckpoints();
		this.RefreshDownloadPart();
	}

	public override void Close()
	{
		if (!this.isScence)
		{
			base.Close();
		}
		else
		{
			Singleton<UiManager>.Instance.PageStack.Pop();
			base.gameObject.SetActive(false);
			Singleton<UiManager>.Instance.SetCurrentPage();
		}
	}

	public void OnclickStart()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		CheckpointDataManager.SelectCheckpoint = this.SelectData;
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.Map)
		{
			UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			component.Button.SetActive(false);
			UnityEngine.Object.Destroy(component.Button);
			component.type = TeachUIType.None;
			if (CheckpointDataManager.SelectCheckpoint.Type > (CheckpointType)10)
			{
				Singleton<GlobalData>.Instance.FirstSpecial = 0;
			}
			Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).Close();
		}
		else
		{
			if ((CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.MAINLINE_SNIPE || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.SNIPE) && !this.isEquipeSniperRifle())
			{
				if (this.RemindWeaponPart.gameObject.activeSelf)
				{
					this.RemindWeaponPart.gameObject.SetActive(false);
				}
				Singleton<UiManager>.Instance.ShowRemind("NEED_SNIPE_WEAPON", "JUMP", delegate
				{
					Singleton<UiManager>.Instance.CurrentPage.Hide();
					Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
					{
						Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
					});
				}, null);
				return;
			}
			if (Singleton<GlobalData>.Instance.GetEnergy() <= 0)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.EnergyShortagePage, null);
				return;
			}
			if (PlayerDataManager.isFightingStrengthShortage(CheckpointDataManager.SelectCheckpoint))
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.PowerShortagePage, null);
				return;
			}
			Singleton<GlobalData>.Instance.SetEnergy(-1);
		}
		this.isScence = true;
		this.Close();
		Singleton<UiManager>.Instance.ShowTopBar(false);
		Singleton<GlobalData>.Instance.AdvertisementReviveTimes = 0;
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
		{
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InRacingPage.ToString());
			Singleton<UiManager>.Instance.ShowLoadingPage("RacingScene", PageName.InRacingPage);
		}
		else
		{
			GameLogManager.SendPageLog(this.Name.ToString(), PageName.InGamePage.ToString());
			Singleton<UiManager>.Instance.ShowLoadingPage(CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID), PageName.InGamePage);
		}
	}

	public void OnclickEquip()
	{
		base.gameObject.SetActive(false);
		Singleton<UiManager>.Instance.ShowPage(PageName.EquipmentPage, null);
		Singleton<UiManager>.Instance.ShowTopBar(true);
	}

	public void OnclickGOLD()
	{
		this.RefreshInfoPart();
	}

	public void RefreshInfoPart()
	{
		PlayerData player = PlayerDataManager.Player;
		this.StartButtonName.text = Singleton<GlobalData>.Instance.GetText("START");
		Singleton<FontChanger>.Instance.SetFont(StartButtonName);
		if (this.SelectData.Type == CheckpointType.WEAPON)
		{
			WeaponData tryWeapon = WeaponDataManager.GetTryWeapon();
			this.PlayerImgs[0].gameObject.SetActive(true);
			this.PlayerDes[0].gameObject.SetActive(true);
			this.PlayerDes[0].text = Singleton<GlobalData>.Instance.GetText("FIREPOWER2");
			this.PlayerNum[0].text = WeaponDataManager.GetInitFightingStrength(tryWeapon).ToString();
			this.PlayerImgs[0].sprite = Singleton<UiManager>.Instance.GetSprite(tryWeapon.Icon);
			this.PlayerImgs[1].gameObject.SetActive(false);
			this.PlayerDes[1].gameObject.SetActive(false);
			this.PlayerNum[1].gameObject.SetActive(false);
		}
		else
		{
			if (player.Weapon1 == 0)
			{
				this.PlayerImgs[0].gameObject.SetActive(false);
				this.PlayerDes[0].gameObject.SetActive(false);
			}
			else
			{
				this.PlayerImgs[0].gameObject.SetActive(true);
				this.PlayerDes[0].gameObject.SetActive(true);
				this.PlayerDes[0].text = Singleton<GlobalData>.Instance.GetText("FIREPOWER2");
				this.PlayerNum[0].text = WeaponDataManager.GetCurrentFightingStrength(WeaponDataManager.GetWeaponData(player.Weapon1)).ToString();
				this.PlayerImgs[0].sprite = Singleton<UiManager>.Instance.GetSprite(WeaponDataManager.GetWeaponData(player.Weapon1).Icon);
			}
			if (player.Weapon2 == 0)
			{
				this.PlayerImgs[1].gameObject.SetActive(false);
				this.PlayerDes[1].gameObject.SetActive(false);
				this.PlayerNum[1].gameObject.SetActive(false);
			}
			else
			{
				this.PlayerNum[1].gameObject.SetActive(true);
				this.PlayerImgs[1].gameObject.SetActive(true);
				this.PlayerDes[1].gameObject.SetActive(true);
				this.PlayerDes[1].text = Singleton<GlobalData>.Instance.GetText("FIREPOWER2");
				this.PlayerNum[1].text = WeaponDataManager.GetCurrentFightingStrength(WeaponDataManager.GetWeaponData(player.Weapon2)).ToString();
				this.PlayerImgs[1].sprite = Singleton<UiManager>.Instance.GetSprite(WeaponDataManager.GetWeaponData(player.Weapon2).Icon);
			}
		}
		if (player.Prop1 == 0)
		{
			this.PlayerImgs[2].gameObject.SetActive(false);
			this.PlayerDes[2].gameObject.SetActive(false);
			this.PlayerNum[2].gameObject.SetActive(false);
		}
		else
		{
			this.PlayerImgs[2].gameObject.SetActive(true);
			this.PlayerDes[2].gameObject.SetActive(true);
			this.PlayerNum[2].gameObject.SetActive(true);
			this.PlayerDes[2].text = Singleton<GlobalData>.Instance.GetText("AMOUNT");
			this.PlayerNum[2].text = PropDataManager.GetPropData(player.Prop1).Count.ToString();
			this.PlayerImgs[2].sprite = Singleton<UiManager>.Instance.GetSprite(PropDataManager.GetPropData(player.Prop1).Icon);
		}
		if (player.Prop2 == 0)
		{
			this.PlayerImgs[3].gameObject.SetActive(false);
			this.PlayerDes[3].gameObject.SetActive(false);
			this.PlayerNum[3].gameObject.SetActive(false);
		}
		else
		{
			this.PlayerImgs[3].gameObject.SetActive(true);
			this.PlayerDes[3].gameObject.SetActive(true);
			this.PlayerNum[3].gameObject.SetActive(true);
			this.PlayerDes[3].text = Singleton<GlobalData>.Instance.GetText("AMOUNT");
			this.PlayerNum[3].text = PropDataManager.GetPropData(player.Prop2).Count.ToString();
			this.PlayerImgs[3].sprite = Singleton<UiManager>.Instance.GetSprite(PropDataManager.GetPropData(player.Prop2).Icon);
		}
		Singleton<FontChanger>.Instance.SetFont(PlayerDes[0]);
		Singleton<FontChanger>.Instance.SetFont(PlayerNum[0]);
		Singleton<FontChanger>.Instance.SetFont(PlayerDes[1]);
		Singleton<FontChanger>.Instance.SetFont(PlayerNum[1]);
		Singleton<FontChanger>.Instance.SetFont(PlayerDes[2]);
		Singleton<FontChanger>.Instance.SetFont(PlayerNum[2]);
		Singleton<FontChanger>.Instance.SetFont(PlayerDes[3]);
		Singleton<FontChanger>.Instance.SetFont(PlayerNum[3]);
		this.modelNameTxt.text = Singleton<GlobalData>.Instance.GetText(this.SelectData.Name);
		this.modelTypeTxt.text = Singleton<GlobalData>.Instance.GetText(this.SelectData.Mission);
		this.descTxt.text = Singleton<GlobalData>.Instance.GetText(this.SelectData.Describe);
		Singleton<FontChanger>.Instance.SetFont(modelNameTxt);
		Singleton<FontChanger>.Instance.SetFont(modelTypeTxt);
		Singleton<FontChanger>.Instance.SetFont(descTxt);
		if (this.SelectData.Type == CheckpointType.MAINLINE)
		{
			if (this.SelectData.Passed)
			{
				this.coinsNumTxt.text = ((int)((float)this.SelectData.AwardCount[0] * 0.1f)).ToString();
			}
			else
			{
				this.coinsNumTxt.text = this.SelectData.AwardCount[0].ToString();
			}
		}
		else if (this.SelectData.Type == CheckpointType.WEAPON)
		{
			this.coinsNumTxt.text = (CheckpointDataManager.GetMaxPassedCheckpoint().AwardCount[0] * 2).ToString();
		}
		else if (this.SelectData.Type >= (CheckpointType)10)
		{
			Text text = this.coinsNumTxt;
			int num = CheckpointDataManager.GetMaxPassedCheckpoint().AwardCount[0];
			text.text = num.ToString();
		}
		this.RecommendText.text = Singleton<GlobalData>.Instance.GetText("POWER_RECOMMEND") + " : " + this.SelectData.RequireFighting;
		Singleton<FontChanger>.Instance.SetFont(RecommendText);
		if (this.SelectData.Type == CheckpointType.SNIPE || this.SelectData.Type == CheckpointType.MAINLINE_SNIPE)
		{
			if (!this.isEquipeSniperRifle())
			{
				this.initRemindWapon();
			}
		}
		else
		{
			this.RemindWeaponPart.gameObject.SetActive(false);
		}
		this.InfoPart.localPosition = this.InfoPartPostion.localPosition;
		this.InfoPart.DOLocalMoveX(this.InfoPart.transform.localPosition.x + 250f, 0.2f, false).From<Tweener>().SetEase(Ease.OutQuad);
		this.InfoPart.GetComponent<CanvasGroup>().alpha = 0f;
		this.InfoPart.GetComponent<CanvasGroup>().DOFade(1f, 0.2f).OnComplete(delegate
		{
			if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.Map)
			{
				UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StartBtn.gameObject);
				gameObject.transform.SetParent(component.Pos01);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.position = this.StartBtn.transform.position;
				component.Button = gameObject;
				component.EffectObj.transform.position = this.StartBtn.transform.position;
			}
			if (player.Weapon2 != 0)
			{
				this.CheckFight(player);
			}
			else
			{
				this.MainFight.SetActive(false);
				this.SecondFight.SetActive(false);
			}
		});
	}

	public void OnclickLeft()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.ChangeChapter(1);
	}

	public void OnclickRight()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		this.ChangeChapter(-1);
	}

	public void OnclickResetMap(int i)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
	}

	public void OnclickWeapon(int num)
	{
		if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index < 4)
		{
			return;
		}
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.Hide();
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.MapUpgrade)
		{
			UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			component.Button.SetActive(false);
			UnityEngine.Object.Destroy(component.Button);
			component.type = TeachUIType.WeaponUpgrade;
			component.EffectObj.SetActive(false);
		}
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<GlobalData>.Instance.selectWeapon = num;
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
		});
	}

	public void OnclickProp(int num)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<GlobalData>.Instance.selectProp = num;
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
		});
	}

	public void OnclickBoxes()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.BoxPage.ToString());
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
	}

	public void ClickOnPassGift()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (LevelPassGiftSystem.GetCurrentData() != null)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.LevelPassGiftPage, null);
		}
	}

	public void ClickOnSnipeTeach()
	{
		this.SelectEffect.gameObject.SetActive(true);
		this.SnipeTeachPart.gameObject.SetActive(false);
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
		Singleton<UiManager>.Instance.CanBack = true;
	}

	public void ChangeRemindWeapon()
	{
		WeaponData maxFightingSnipeRifle = WeaponDataManager.GetMaxFightingSnipeRifle();
		PlayerDataManager.Equip(EquipmentPosition.Weapon1, maxFightingSnipeRifle.ID);
		this.RemindWeaponPart.gameObject.SetActive(false);
		this.RefreshInfoPart();
	}

	private void SetDownloadIndex(AssetDownloadData asset)
	{
		if (asset != null)
		{
			this.AssetIndex = 0;
			this.AssetPaths = AssetDownloadManager.GetAssetPath(asset);
			while (this.AssetIndex < this.AssetPaths.Length)
			{
				if (!Singleton<AssetBundleManager>.Instance.IsAssetBundleExist(this.AssetPaths[this.AssetIndex]))
				{
					break;
				}
				this.AssetIndex++;
			}
		}
		else
		{
			this.AssetIndex = -1;
		}
	}

	public void RefreshDownloadPart()
	{
		AssetDownloadData downloadData = AssetDownloadManager.GetDownloadData(this.PageIndex + 1);
		this.SetDownloadIndex(downloadData);
		if (this.AssetIndex == -1 || this.AssetIndex == this.AssetPaths.Length)
		{
			this.DownloadPart.gameObject.SetActive(false);
			if (!this.MapChapters[this.PageIndex].gameObject.activeSelf)
			{
				this.MapChapters[this.PageIndex].gameObject.SetActive(true);
			}
			if (!this.InfoPart.gameObject.activeSelf)
			{
				this.InfoPart.gameObject.SetActive(true);
			}
			if (!this.LevelPassGiftButton.gameObject.activeSelf && LevelPassGiftSystem.GetCurrentData() != null)
			{
				this.LevelPassGiftButton.gameObject.SetActive(true);
			}
		}
		else
		{
			this.DownloadPart.gameObject.SetActive(true);
			this.HideSpecialCheckpoints();
			this.MapChapters[this.PageIndex].gameObject.SetActive(false);
			this.InfoPart.gameObject.SetActive(false);
			this.LevelPassGiftButton.gameObject.SetActive(false);
			this.ConfirmDownloadButton.gameObject.SetActive(!downloadData.Downloading);
			this.CancelDownloadButton.gameObject.SetActive(downloadData.Downloading);
			this.DownloadSlider.gameObject.SetActive(downloadData.Downloading);
			this.DownProgress.gameObject.SetActive(downloadData.Downloading);
			//this.DownTitle.text = Singleton<GlobalData>.Instance.GetText("DOWNLOAD_TITLE");
			this.DownTitle.text = Singleton<GlobalData>.Instance.GetText("COMING_SOON_TITLE");
			this.ConfirmDownloadText.text = Singleton<GlobalData>.Instance.GetText("DOWNLOAD");
			this.DownProgress.text = string.Concat(new object[]
			{
				"(",
				this.AssetIndex + 1,
				"/",
				this.AssetPaths.Length,
				")",
				Singleton<GlobalData>.Instance.GetText("DOWNLOAD_TIP")
			});
			Singleton<FontChanger>.Instance.SetFont(DownTitle);
			Singleton<FontChanger>.Instance.SetFont(ConfirmDownloadText);
			Singleton<FontChanger>.Instance.SetFont(DownProgress);
		}
	}

	private void DownLoad()
	{
		Debug.Log($"!!! {AssetPaths[this.AssetIndex]}");
		Singleton<AssetBundleManager>.Instance.DownLoadAssetBundle(this.AssetPaths[this.AssetIndex], delegate(float dt)
		{
			this.DownloadSlider.value = dt;
		}, delegate
		{
			if (this.AssetIndex < this.AssetPaths.Length - 1)
			{
				this.AssetIndex++;
				this.DownProgress.text = string.Concat(new object[]
				{
					"(",
					this.AssetIndex + 1,
					"/",
					this.AssetPaths.Length,
					")",
					Singleton<GlobalData>.Instance.GetText("DOWNLOAD_TIP")
				});
				this.DownLoad();
			}
			else
			{
				GameLogManager.SendPageLog("DownloadComplete", "null");
				Singleton<UiManager>.Instance.ShowMessage("DOWNLOAD_COMPLETE", 0.5f);
				AssetDownloadManager.SetDownloadingState(this.PageIndex + 1, false);
				this.RefreshPage();
			}
		}, delegate
		{
			Singleton<UiManager>.Instance.ShowMessage("DOWNLOAD_FAILD", 0.5f);
			GameLogManager.SendPageLog("DownloadComplete", (this.AssetIndex + 1).ToString());
			AssetDownloadManager.SetDownloadingState(this.PageIndex + 1, false);
			this.RefreshDownloadPart();
		});
	}

	public void ClickOnDownload()
	{
		if (this.IsAction)
		{
			return;
		}
		AssetDownloadManager.SetDownloadingState(this.PageIndex + 1, true);
		GameLogManager.SendPageLog("StartDownload", "null");
		this.DownLoad();
		this.RefreshDownloadPart();
	}

	public void ClickOnCancelDownload()
	{
		if (this.IsAction)
		{
			return;
		}
		Singleton<UiManager>.Instance.ShowRemind(Singleton<GlobalData>.Instance.GetText("CANCEL_DOWNLOAD_TIP"), Singleton<GlobalData>.Instance.GetText("CONFIRM"), delegate
		{
			Singleton<AssetBundleManager>.Instance.CancleDownLoad();
			AssetDownloadManager.SetDownloadingState(this.PageIndex + 1, false);
			this.RefreshDownloadPart();
		}, null);
	}

	public static MapsPage instance;

	public int MaxChapter;

	public int PageIndex;

	public float MoveSpeed = 0.24f;

	public Transform InfoPart;

	public Transform InfoPartPostion;

	public Transform DownloadPart;

	public Text DownTitle;

	public Slider DownloadSlider;

	public Button ConfirmDownloadButton;

	public Button CancelDownloadButton;

	public Text ConfirmDownloadText;

	public Text DownProgress;

	public GameObject SelectEffect;

	public Text titleNameTxt;

	public Text modelNameTxt;

	public Text modelTypeTxt;

	public Text descTxt;

	public Text coinsNumTxt;

	public Text StartButtonName;

	public Text RecommendText;

	public CheckpointButtonInfo GoldCheckpoint;

	public CheckpointButtonInfo BossCheckpoint;

	public CheckpointButtonInfo WeaponCheckpoint;

	public CheckpointButtonInfo RandomCheckpoint;

	public CheckpointButtonInfo SniperCheckpoint;

	public MapChapter[] MapChapters;

	public Image[] Maps;

	public Button[] ChapterTags;

	public GameObject MainFight;

	public Text MainFightTxt;

	public Text SecondFightTxt;

	public GameObject MainWarning;

	public GameObject SecondFight;

	public GameObject SecondWarning;

	public GameObject MainWeaponGo;

	public GameObject SecondWeaponGo;

	public Color[] WarnAndDangerColor;

	public Image BgMainColorImg;

	public Image BgSecondColorImg;

	public Image MainTipColorImg;

	public Image SecTipColorImg;

	public Image[] PlayerImgs;

	public Text[] PlayerDes;

	public Text[] PlayerNum;

	public Button StartBtn;

	public List<int> MapsPosPointList = new List<int>();

	private float MapWidth;

	private Vector3 TouchPosition;

	private CheckpointData RandomData;

	[HideInInspector]
	public CheckpointData SelectData;

	public Button LevelPassGiftButton;

	public Text LevelPassGiftName;

	public GameObject LevelPassGiftTag;

	public Button PassButton;

	public Transform SnipeTeachPart;

	public Text SnipeTeachDescribe;

	public Transform RemindWeaponPart;

	public Image RemindWeaponIcon;

	public Text RemindTip;

	public Text RemindWeaponName;

	public Text RemindWeaponFighting;

	public Text RemindButtonName;

	public bool isScence;

	private string[] AssetPaths;

	private int AssetIndex;
}
