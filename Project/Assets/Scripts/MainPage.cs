using System;
using System.Collections;
using System.Collections.Generic;
using ads;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : GamePage
{
	public new void Awake()
	{
		MainPage.instance = this;
		foreach (Transform transform in this.NpcBtn)
		{
			transform.gameObject.SetActive(false);
		}
		this.openTech = false;
	}

	public override void Close()
	{
		base.Close();
		Singleton<UiManager>.Instance.ShowPage(PageName.CoverPage, null);
		Singleton<UiManager>.Instance.ShowTopBar(false);
	}

	public override void OnBack()
	{
		if (this.isTeaching)
		{
			return;
		}
		base.OnBack();
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Refresh()
	{
		base.Refresh();
		this.RefleshPage();
	}

	public override void Show()
	{
		base.Show();
	}

	public new void OnEnable()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.4f);
		if (Singleton<GameAudioManager>.Instance.MusicSource.isPlaying && Singleton<GameAudioManager>.Instance.GameMusic && Singleton<GameAudioManager>.Instance.MusicSource.clip != Singleton<GameAudioManager>.Instance.UIBgm)
		{
			Singleton<GameAudioManager>.Instance.PauseSoundInGame();
			Singleton<GameAudioManager>.Instance.PlayMusic(Singleton<GameAudioManager>.Instance.UIBgm);
		}
		if (null != ScenceControl.instance)
		{
			ScenceControl.instance.ShowUI();
		}
		this.OpenBoxGo.SetActive(CheckpointDataManager.GetCurrentCheckpoint().ID > 3);
		if (Singleton<GlobalData>.Instance.FirstBoxGuide > 0 & AchievementDataManager.GetAchievementData(DataCenter.AchievementType.OPEN_BOXES).CurrentValue > 0)
		{
			Singleton<GlobalData>.Instance.FirstBoxGuide = 0;
		}
		if (null != NpcScenceControl.instance)
		{
			NpcScenceControl.instance.ResetPos();
			NpcScenceControl.instance.CanShow = false;
			NpcScenceControl.instance.RoomGo.SetActive(true);
		}
		this.RefleshPage();
		this.CheckTeach();
		this.ShowDebris();
		if (null == this.curTeachPage)
		{
			if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage))
			{
				if (!Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).gameObject.activeInHierarchy)
				{
					this.AllPush();
				}
			}
			else
			{
				this.AllPush();
			}
		}
		Singleton<UiManager>.Instance.SetTopEnable(true, true);
		Singleton<UiManager>.Instance.TopBar.Refresh();
	}

	private void Start()
	{
		if (null == this.curTeachPage && (null == Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) || !Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).gameObject.activeInHierarchy) && Singleton<GlobalData>.Instance.GameLogonTimes > 1 && ((CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index >= 6) || CheckpointDataManager.GetCurrentCheckpoint().Chapters > ChapterEnum.CHAPTERNAME_01) && Singleton<GlobalData>.Instance.AdvertisementLotteryTimes > 0)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.AdvertisementLotteryPage, null);
		}
	}

	private void ShowLevelCompletePush()
	{
		if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.WEAPON)
		{
			if (Singleton<UiManager>.Instance.GameSuccess == 1)
			{
				Singleton<UiManager>.Instance.GameSuccess = 0;
				if (WeaponDataManager.GetTryWeapon().State == WeaponState.未解锁)
				{
					if (WeaponDataManager.GetTryWeapon().ID == 2005)
					{
						Singleton<UiManager>.Instance.ShowPushGift(9030);
					}
					else
					{
						Singleton<UiManager>.Instance.ShowWeaponPush(WeaponDataManager.GetTryWeapon().ID);
					}
				}
			}
		}
		else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.GOLD)
		{
			if (Singleton<UiManager>.Instance.GameSuccess == 1)
			{
				Singleton<UiManager>.Instance.GameSuccess = 0;
				if (!PlayerPrefs.GetString("FACEBOOK_NOTICE_RESULT").Equals("SUCCESS") && Singleton<GlobalData>.Instance.FacebookNoticeCounts > 0)
				{
					Singleton<UiManager>.Instance.ShowFacebookGroupPage(2);
				}
			}
		}
		else
		{
			if (this.PushGiftCounts >= 5)
			{
				return;
			}
			if (Singleton<GlobalData>.Instance.GameLogonTimes > 1 && Singleton<GlobalData>.Instance.CheckpointFinishTimes == 1 && this.PushGiftCounts == 0)
			{
				Singleton<UiManager>.Instance.GameSuccess = 0;
				this.PushGift();
			}
			else if (Singleton<UiManager>.Instance.GameSuccess == 1)
			{
				Singleton<UiManager>.Instance.GameSuccess = 0;
				if (CheckpointDataManager.SelectCheckpoint.ID == CheckpointDataManager.GetCurrentCheckpoint().ID - 1 && this.PushCheckpoints.Contains(CheckpointDataManager.GetCurrentCheckpoint().ID - 1))
				{
					this.PushGift();
				}
			}
			else if (Singleton<UiManager>.Instance.GameSuccess == -1)
			{
				Singleton<UiManager>.Instance.GameSuccess = 0;
				this.PushGift();
			}
		}
	}

	private void PushGift()
	{
		this.PushGiftCounts++;
		StoreData pushGift = StoreDataManager.GetPushGift();
		if (pushGift != null)
		{
			Singleton<UiManager>.Instance.ShowPushGift(pushGift.ID);
		}
	}

	public void ShowDebris()
	{
		this.DebrisSlider.value = 0f;
		if (Singleton<GlobalData>.Instance.InGameCurItem != null)
		{
			this.DebrisGo.SetActive(true);
			this.DebrisGo.GetComponent<DOTweenAnimation>().DORestart(false);
			DebrisData inGameCurItem = Singleton<GlobalData>.Instance.InGameCurItem;
			ItemData itemData = ItemDataManager.GetItemData(inGameCurItem.ItemID);
			if (itemData.ItemTag == DataCenter.ItemType.Prop)
			{
				PropData propData = PropDataManager.GetPropData(itemData.ID);
				this.NumDebris.text = inGameCurItem.Count + "/" + propData.RequiredDebris;
				this.DebrisSlider.DOValue((float)inGameCurItem.Count, 0.5f, false);
				this.DebrisSlider.maxValue = (float)propData.RequiredDebris;
				this.DebrisItemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(propData.Icon);
			}
			else if (itemData.ItemTag == DataCenter.ItemType.Weapon)
			{
				WeaponData weaponData = WeaponDataManager.GetWeaponData(itemData.ID);
				this.NumDebris.text = inGameCurItem.Count + "/" + weaponData.RequiredDebris;
				this.DebrisSlider.DOValue((float)inGameCurItem.Count, 0.5f, false).SetDelay(1.2f);
				this.DebrisSlider.maxValue = (float)weaponData.RequiredDebris;
				this.DebrisItemIcon.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
			}
			Singleton<GlobalData>.Instance.InGameCurItem = null;
		}
		else
		{
			this.DebrisGo.SetActive(false);
		}
	}

	public void AllPush()
	{
		if (PlayerDataManager.CanUpgrade())
		{
			Singleton<GlobalData>.Instance.EnergyBackToFull();
			this.ShowUpReward();
		}
		if (LogonAwardSystem.CanReceive() && CheckpointDataManager.GetCurrentCheckpoint().ID > 4)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.LogonAwardPage, null);
		}
		if (Singleton<GlobalData>.Instance.TodayVIPReward > 0 && ItemDataManager.GetCommonItem(CommonDataType.VIP).Count > 0)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.VipDayRewardPopup, null);
		}
		this.ShowLevelCompletePush();
		if (!RoleDataManager.Roles[1].Enable && CheckpointDataManager.GetCheckpointData(ChapterEnum.CHAPTERNAME_04, 10).Passed)
		{
			RoleDataManager.Unlcok(RoleDataManager.Roles[1].ID);
		}
	}

	public void Update()
	{
		if (UITick.getPushGiftNeedSec().Length == 0)
		{
			this.PushBtn.SetActive(false);
			UITick.setPushGiftSec(0, 0);
		}
	}

	public void CheckTeach()
	{
		if (Singleton<GlobalData>.Instance.FirstShouLei == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 3)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
			this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			this.curTeachPage.type = TeachUIType.MainProp;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EquipBtnTran.gameObject);
			gameObject.transform.SetParent(this.curTeachPage.Pos01);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.position = this.EquipBtnTran.position;
			this.curTeachPage.Button = gameObject;
			this.curTeachPage.RefreshPage();
		}
		else if (Singleton<GlobalData>.Instance.FirstBoxGuide != 1 || CheckpointDataManager.GetCurrentCheckpoint().Chapters != ChapterEnum.CHAPTERNAME_02 || CheckpointDataManager.GetCurrentCheckpoint().Index != 1)
		{
			if (Singleton<GlobalData>.Instance.FirstWeapon == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 4)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.type = TeachUIType.MainWeapon;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.EquipBtnTran.gameObject);
				gameObject2.transform.SetParent(this.curTeachPage.Pos01);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.position = this.EquipBtnTran.transform.position;
				this.curTeachPage.Button = gameObject2;
				this.curTeachPage.RefreshPage();
			}
			else if (Singleton<GlobalData>.Instance.FirstTalent == 1 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 8)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.type = TeachUIType.MainTalent;
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.EquipBtnTran.gameObject);
				gameObject3.transform.SetParent(this.curTeachPage.Pos01);
				gameObject3.transform.localScale = Vector3.one;
				gameObject3.transform.position = this.EquipBtnTran.transform.position;
				this.curTeachPage.Button = gameObject3;
				this.curTeachPage.RefreshPage();
			}
			else if (Singleton<GlobalData>.Instance.FirstEquipMent > 0 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 10)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.UITeachPage, null);
				EquipmentDataManager.Collect(3001);
				EquipmentDataManager.Collect(3002);
				EquipmentDataManager.Collect(3003);
				this.curTeachPage = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
				this.curTeachPage.type = TeachUIType.MainEquipMent;
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.EquipBtnTran.gameObject);
				gameObject4.transform.SetParent(this.curTeachPage.Pos01);
				gameObject4.transform.localScale = Vector3.one;
				gameObject4.transform.position = this.EquipBtnTran.transform.position;
				this.curTeachPage.Button = gameObject4;
				this.curTeachPage.RefreshPage();
			}
			else
			{
				this.curTeachPage = null;
			}
		}
	}

	public void CheckNpcAndTeach()
	{
		List<int> list = new List<int>();
		if (Singleton<GlobalData>.Instance.FirstShouLei == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 3)
		{
			NpcScenceControl.instance.curPropGO.SetActive(true);
			NpcScenceControl.instance.curTechGO.SetActive(false);
			NpcScenceControl.instance.curWeaponGO.SetActive(false);
			this.EquipBtnTran.gameObject.SetActive(false);
			NpcScenceControl.instance.curMedicGO.SetActive(false);
			list.Clear();
			list.Add(3);
			this.isTeaching = true;
		}
		else if (Singleton<GlobalData>.Instance.FirstMedic == 1008 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index < 10)
		{
			NpcScenceControl.instance.curPropGO.SetActive(true);
			NpcScenceControl.instance.curTechGO.SetActive(false);
			NpcScenceControl.instance.curWeaponGO.SetActive(true);
			this.EquipBtnTran.gameObject.SetActive(true);
			NpcScenceControl.instance.curMedicGO.SetActive(false);
			list.Clear();
			list.Add(3);
			list.Add(2);
			this.isTeaching = true;
		}
		else if (Singleton<GlobalData>.Instance.FirstWeapon == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 4)
		{
			NpcScenceControl.instance.curPropGO.SetActive(true);
			NpcScenceControl.instance.curTechGO.SetActive(false);
			this.EquipBtnTran.gameObject.SetActive(true);
			NpcScenceControl.instance.curWeaponGO.SetActive(true);
			NpcScenceControl.instance.curMedicGO.SetActive(false);
			list.Clear();
			list.Add(3);
			list.Add(2);
			this.isTeaching = true;
		}
		else if (Singleton<GlobalData>.Instance.FirstTalent == 1 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 8)
		{
			NpcScenceControl.instance.curPropGO.SetActive(true);
			NpcScenceControl.instance.curTechGO.SetActive(true);
			NpcScenceControl.instance.curWeaponGO.SetActive(true);
			this.EquipBtnTran.gameObject.SetActive(true);
			NpcScenceControl.instance.curMedicGO.SetActive(false);
			list.Clear();
			list.Add(3);
			list.Add(2);
			list.Add(0);
			this.isTeaching = true;
		}
		else if (Singleton<GlobalData>.Instance.FirstEquipMent > 0 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 10)
		{
			NpcScenceControl.instance.curPropGO.SetActive(true);
			NpcScenceControl.instance.curTechGO.SetActive(true);
			NpcScenceControl.instance.curWeaponGO.SetActive(true);
			this.EquipBtnTran.gameObject.SetActive(true);
			NpcScenceControl.instance.curMedicGO.SetActive(false);
			list.Clear();
			list.Add(3);
			list.Add(2);
			list.Add(1);
			list.Add(0);
			this.isTeaching = true;
		}
		else
		{
			if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01)
			{
				if (CheckpointDataManager.GetCurrentCheckpoint().Index == 3)
				{
					NpcScenceControl.instance.curPropGO.SetActive(true);
					NpcScenceControl.instance.curTechGO.SetActive(false);
					NpcScenceControl.instance.curWeaponGO.SetActive(false);
					this.EquipBtnTran.gameObject.SetActive(false);
					NpcScenceControl.instance.curMedicGO.SetActive(false);
					list.Clear();
					list.Add(3);
				}
				else if (CheckpointDataManager.GetCurrentCheckpoint().Index < 8 && CheckpointDataManager.GetCurrentCheckpoint().Index > 3)
				{
					NpcScenceControl.instance.curPropGO.SetActive(true);
					NpcScenceControl.instance.curTechGO.SetActive(false);
					NpcScenceControl.instance.curWeaponGO.SetActive(true);
					this.EquipBtnTran.gameObject.SetActive(true);
					NpcScenceControl.instance.curMedicGO.SetActive(false);
					list.Clear();
					list.Add(3);
					list.Add(2);
				}
				else if (CheckpointDataManager.GetCurrentCheckpoint().Index < 10 && CheckpointDataManager.GetCurrentCheckpoint().Index >= 8)
				{
					NpcScenceControl.instance.curPropGO.SetActive(true);
					NpcScenceControl.instance.curTechGO.SetActive(true);
					NpcScenceControl.instance.curWeaponGO.SetActive(true);
					this.EquipBtnTran.gameObject.SetActive(true);
					NpcScenceControl.instance.curMedicGO.SetActive(false);
					list.Clear();
					list.Add(3);
					list.Add(2);
					list.Add(0);
				}
				else if (CheckpointDataManager.GetCurrentCheckpoint().Index >= 10)
				{
					NpcScenceControl.instance.curPropGO.SetActive(true);
					NpcScenceControl.instance.curTechGO.SetActive(true);
					NpcScenceControl.instance.curWeaponGO.SetActive(true);
					this.EquipBtnTran.gameObject.SetActive(true);
					NpcScenceControl.instance.curMedicGO.SetActive(true);
					list.Clear();
					list.Add(3);
					list.Add(2);
					list.Add(1);
					list.Add(0);
				}
			}
			else
			{
				NpcScenceControl.instance.curPropGO.SetActive(true);
				NpcScenceControl.instance.curTechGO.SetActive(true);
				NpcScenceControl.instance.curWeaponGO.SetActive(true);
				this.EquipBtnTran.gameObject.SetActive(true);
				NpcScenceControl.instance.curMedicGO.SetActive(true);
				list.Clear();
				list.Add(3);
				list.Add(2);
				list.Add(1);
				list.Add(0);
			}
			this.isTeaching = false;
		}
		this.ShowGuide(list);
	}

	public void ShowAllNpc()
	{
		NpcScenceControl.instance.curPropGO.SetActive(true);
		NpcScenceControl.instance.curTechGO.SetActive(true);
		NpcScenceControl.instance.curWeaponGO.SetActive(true);
		NpcScenceControl.instance.curMedicGO.SetActive(true);
		this.EquipBtnTran.gameObject.SetActive(true);
		for (int i = 0; i < this.NpcTrans.Length; i++)
		{
			this.NpcBtn[i].gameObject.SetActive(true);
			this.NpcTrans[i].gameObject.SetActive(true);
			this.GuideTrans[i].gameObject.SetActive(false);
		}
	}

	public void ShowGuide(List<int> num)
	{
		Singleton<UiManager>.Instance.ShowTopBar(true);
		if (this.isTeaching)
		{
			Singleton<UiManager>.Instance.SetTopEnable(false, false);
			if (Singleton<GlobalData>.Instance.FirstShouLei == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 3)
			{
				this.GuideTrans[3].gameObject.SetActive(true);
			}
			else if (Singleton<GlobalData>.Instance.FirstWeapon == 2 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 4)
			{
				this.GuideTrans[2].gameObject.SetActive(true);
			}
			else if (Singleton<GlobalData>.Instance.FirstTalent == 1 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 8)
			{
				this.GuideTrans[0].gameObject.SetActive(true);
			}
			else if (Singleton<GlobalData>.Instance.FirstEquipMent > 0 && CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index == 10)
			{
				this.GuideTrans[1].gameObject.SetActive(true);
				NpcScenceControl.instance.curMedicGO.SetActive(true);
			}
			else
			{
				for (int i = 0; i < this.GuideTrans.Length; i++)
				{
					this.GuideTrans[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			Singleton<UiManager>.Instance.SetTopEnable(true, true);
			for (int j = 0; j < this.GuideTrans.Length; j++)
			{
				this.GuideTrans[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.NpcTrans.Length; k++)
		{
			for (int l = 0; l < num.Count; l++)
			{
				if (k == num[l])
				{
					this.NpcBtn[k].GetComponent<Button>().enabled = true;
					this.NpcTrans[k].gameObject.SetActive(true);
					break;
				}
				this.NpcBtn[k].GetComponent<Button>().enabled = false;
				this.NpcTrans[k].gameObject.SetActive(false);
			}
		}
	}

	public void OnclickPushGift()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		int pushGiftType = UITick.getPushGiftType();
		Singleton<UiManager>.Instance.ShowPushGift(pushGiftType);
	}

	public void OnDisable()
	{
		Write.LogWarning("MainPage被关闭，注意啦注意啦");
		if (null != NpcScenceControl.instance)
		{
			NpcScenceControl.instance.CanShow = false;
		}
	}

	public void InitBtnPos(Transform trans, Transform transBtn, int id)
	{
		for (int i = 0; i < this.NpcBtn.Length; i++)
		{
			if (i == id && NpcScenceControl.instance.CanShow)
			{
				this.NpcBtn[i].gameObject.SetActive(true);
			}
			else
			{
				this.NpcBtn[i].gameObject.SetActive(true);
			}
		}
		if (id != -1)
		{
			Vector3 position = Camera.main.WorldToScreenPoint(trans.position);
			Vector3 position2 = Singleton<UiManager>.Instance.UiCamera.ScreenToWorldPoint(position);
			Vector3 position3 = Camera.main.WorldToScreenPoint(transBtn.position);
			Vector3 position4 = Singleton<UiManager>.Instance.UiCamera.ScreenToWorldPoint(position3);
			this.NpcBtn[id].transform.position = position4;
			this.NpcBtn[id].transform.localPosition = new Vector3(this.NpcBtn[id].transform.localPosition.x, this.NpcBtn[id].transform.localPosition.y, 0f);
			this.NpcTrans[id].transform.position = position2;
			this.NpcTrans[id].transform.localPosition = new Vector3(this.NpcTrans[id].transform.localPosition.x, this.NpcTrans[id].transform.localPosition.y, 0f);
		}
	}

	public void RefleshPage()
	{
		this.InitBtnPos(NpcScenceControl.instance.techTrans, NpcScenceControl.instance.techEnterTrans, 0);
		this.InitBtnPos(NpcScenceControl.instance.medicTrans, NpcScenceControl.instance.medicEnterTrans, 1);
		this.InitBtnPos(NpcScenceControl.instance.weaponTrans, NpcScenceControl.instance.weaponEnterTrans, 2);
		this.InitBtnPos(NpcScenceControl.instance.propTrans, NpcScenceControl.instance.propTrans, 3);
		this.equipTxt.text = Singleton<GlobalData>.Instance.GetText("EQUIPMENT");
		this.worldTxt.text = Singleton<GlobalData>.Instance.GetText("WORLD");
		this.openBoxTxt.text = Singleton<GlobalData>.Instance.GetText("RO_NAME_01");
		this.propNpcTxt.text = Singleton<GlobalData>.Instance.GetText("RO_NAME_02");
		this.weaponNpcTxt.text = Singleton<GlobalData>.Instance.GetText("RO_NAME_03");
		this.OpenBoxBtnTxt.text = Singleton<GlobalData>.Instance.GetText("OPENBOX");
		this.AdButton.gameObject.SetActive(Singleton<GlobalData>.Instance.MainPageAdvertisementCount > 0);
		this.AdButtonName.text = Singleton<GlobalData>.Instance.GetText("FREE");
		this.AchievementButton.Name.text = Singleton<GlobalData>.Instance.GetText("ACHIEVEMENT");
		this.AchievementButton.Tip.SetActive(AchievementDataManager.CheckAchievement());
		this.DailyLimitButton.gameObject.SetActive(true);
		this.DailyLimitButton.Name.text = Singleton<GlobalData>.Instance.GetText("DAILY_LIMITED_TITLE");
		this.DailyLimitButton.Tip.SetActive(!StoreDataManager.IsAllLimitGiftsBought());
		this.DailyMissionButton.gameObject.SetActive(CheckpointDataManager.GetCurrentCheckpoint().ID > 5);
		this.DailyMissionButton.Name.text = Singleton<GlobalData>.Instance.GetText("DAILY_MISSION");
		this.DailyMissionButton.Tip.SetActive(DailyMissionSystem.HasMissionCompleted());
		this.LogonAwardButton.gameObject.SetActive(CheckpointDataManager.GetCurrentCheckpoint().ID > 4);
		this.LogonAwardButton.Name.text = Singleton<GlobalData>.Instance.GetText("DAILY_LOGON_AWARD");
		this.LogonAwardButton.Tip.SetActive(LogonAwardSystem.CanReceive());
		EachLevelGiftSystem.SetReceiveState();
		this.LevelAwardButton.gameObject.SetActive(!EachLevelGiftSystem.isAllReceived());
		this.LevelAwardButton.Name.text = Singleton<GlobalData>.Instance.GetText("STORE_NAME_43");
		this.LevelAwardButton.Tip.SetActive(EachLevelGiftSystem.CanReceive());
		this.LotteryButton.gameObject.SetActive(Singleton<GlobalData>.Instance.AdvertisementLotteryTimes > 0);
		this.LotteryButton.Name.text = Singleton<GlobalData>.Instance.GetText("LOTTERY");
		this.LevelAwardButton.Tip.SetActive(false);
		this.pushData = StoreDataManager.GetPushGift();
		this.GreatDealsButton.gameObject.SetActive(this.pushData != null);
		this.GreatDealsName.text = Singleton<GlobalData>.Instance.GetText("GREAT_DEALS");
		this.GiftMp5Button.gameObject.SetActive(!StoreDataManager.GetStoreData(9040).Purchased);
		this.GiftMp5Name.text = "MP5";
		Singleton<FontChanger>.Instance.SetFont(equipTxt);
		Singleton<FontChanger>.Instance.SetFont(worldTxt);
		Singleton<FontChanger>.Instance.SetFont(openBoxTxt);
		Singleton<FontChanger>.Instance.SetFont(propNpcTxt);
		Singleton<FontChanger>.Instance.SetFont(weaponNpcTxt);
		Singleton<FontChanger>.Instance.SetFont(OpenBoxBtnTxt);
		Singleton<FontChanger>.Instance.SetFont(AdButtonName);
		Singleton<FontChanger>.Instance.SetFont(AchievementButton.Name);
		Singleton<FontChanger>.Instance.SetFont(DailyLimitButton.Name);
		Singleton<FontChanger>.Instance.SetFont(DailyMissionButton.Name);
		Singleton<FontChanger>.Instance.SetFont(LogonAwardButton.Name);
		Singleton<FontChanger>.Instance.SetFont(LevelAwardButton.Name);
		Singleton<FontChanger>.Instance.SetFont(LotteryButton.Name);
		Singleton<FontChanger>.Instance.SetFont(GreatDealsName);
		Singleton<FontChanger>.Instance.SetFont(GiftMp5Name);
		StoreData storeData = StoreDataManager.GetStoreData(9041);
		if (storeData != null)
		{
			if (storeData.Purchased)
			{
				this.HallowmasGiftButton.gameObject.SetActive(false);
			}
			else if (WeaponDataManager.GetWeaponData(2010).State == WeaponState.未解锁 || WeaponDataManager.GetWeaponData(2103).State == WeaponState.未解锁)
			{
				this.HallowmasGiftButton.gameObject.SetActive(true);
				this.HallowmasGiftName.text = Singleton<GlobalData>.Instance.GetText(storeData.Name);
				Singleton<FontChanger>.Instance.SetFont(HallowmasGiftName);
			}
			else
			{
				this.HallowmasGiftButton.gameObject.SetActive(false);
			}
		}
		else
		{
			this.HallowmasGiftButton.gameObject.SetActive(false);
		}
	}

	public void ShowUpReward()
	{
		PlayerDataManager.Upgrade();
		int[] id = new int[]
		{
			1,
			2
		};
		int[] upgradeAward = PlayerDataManager.GetUpgradeAward();
		ItemDataManager.SetCurrency(CommonDataType.GOLD, upgradeAward[0]);
		ItemDataManager.SetCurrency(CommonDataType.DIAMOND, upgradeAward[1]);
		Singleton<UiManager>.Instance.TopBar.Refresh();
		Singleton<UiManager>.Instance.ShowAward(id, upgradeAward, Singleton<GlobalData>.Instance.GetText("UPGRDE_REWARD"));
	}

	public void OnclickMapWorld()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.MapsPage.ToString());
		this.Hide();
		Singleton<UiManager>.Instance.ShowPage(PageName.MapsPage, null);
		if (null != Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>().type == TeachUIType.Main)
		{
			UITeachPage component = Singleton<UiManager>.Instance.GetPage(PageName.UITeachPage).GetComponent<UITeachPage>();
			component.Button.SetActive(false);
			component.type = TeachUIType.Map;
			component.RefreshPage();
		}
	}

	public void OnclickEquip()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.Hide();
		if (null != this.curTeachPage)
		{
			this.curType = this.curTeachPage.type;
			this.curTeachPage.OnclickMainType();
			switch (this.curType)
			{
			case TeachUIType.MainEquipMent:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Equipment;
				});
				break;
			case TeachUIType.MainProp:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
				});
				break;
			case TeachUIType.MainTalent:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Talent;
				});
				break;
			case TeachUIType.MainWeapon:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
				});
				break;
			}
		}
		else if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index < 4)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
			{
				Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
			});
		}
		else
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
			{
				Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
			});
		}
	}

	private IEnumerator StartTeach()
	{
		this.Hide();
		yield return new WaitForSeconds(1f);
		if (null != this.curTeachPage)
		{
			this.curType = this.curTeachPage.type;
			this.curTeachPage.OnclickMainType();
			switch (this.curType)
			{
			case TeachUIType.MainEquipMent:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Equipment;
				});
				yield return new WaitForSeconds(1f);
				break;
			case TeachUIType.MainProp:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
				});
				yield return new WaitForSeconds(1f);
				break;
			case TeachUIType.MainTalent:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Talent;
				});
				yield return new WaitForSeconds(1f);
				break;
			case TeachUIType.MainWeapon:
				Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
				{
					Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
				});
				yield return new WaitForSeconds(1f);
				break;
			}
		}
		else if (CheckpointDataManager.GetCurrentCheckpoint().Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.GetCurrentCheckpoint().Index < 4)
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
			{
				Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
			});
		}
		else
		{
			Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
			{
				Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
			});
		}
		yield break;
	}

	public void OnclickWeapon()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.NpcBtn[2].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curWeaponGO, 30);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curWeaponGO, true, true);
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Weapon;
		});
		Singleton<UiManager>.Instance.ShowTopBar(true);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.weaponCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			NpcScenceControl.instance.curWeaponGO.GetComponent<NpcRoleControll>().SetAni(1);
		});
	}

	public void OnclickEngineer()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.NpcBtn[3].gameObject.SetActive(false);
		this.Hide();
		NpcScenceControl.instance.CanShow = false;
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curPropGO, 30);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curPropGO, true, true);
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Prop;
		});
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.propCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void OnclickMedic()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.FunctionPage.ToString());
		this.NpcBtn[1].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curMedicGO, 30);
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curMedicGO, true, true);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Equipment;
		});
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.medicCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void OnclickPause()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.PausePage, null);
	}

	public void OnclickAddExp()
	{
		PlayerDataManager.AddExperience(10);
		Singleton<UiManager>.Instance.TopBar.Refresh();
	}

	public void OnclickBox()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.CanBack = false;
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.BoxPage.ToString());
		this.NpcBtn[0].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curTechGO, 30);
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curTechGO, true, true);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.BoxPage, null);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.techCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.RoomGo.SetActive(false);
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
		if (!(null != this.curTeachPage) || this.curTeachPage.type == TeachUIType.MainBox)
		{
		}
	}

	public void OnclickTalent()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.BoxPage.ToString());
		this.NpcBtn[0].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curTechGO, 30);
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curTechGO, true, true);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.FunctionPage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.FunctionPage).GetComponent<FunctionPage>().CurrentTag = FunctionPageTag.Talent;
		});
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.techCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.RoomGo.SetActive(false);
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void OnclickMore()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		//Singleton<GlobalData>.Instance.ShowAdvertisement(-1, delegate
		//{
			
		//}, null);
		Ads.ShowReward(OnclickMoreSuccess);
        //Advertisements.Instance.ShowRewardedVideo(OnFinished);
	}
    private void OnFinished(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            this.OnclickMoreSuccess();
        }
       
    }

    public void OnclickMoreSuccess()
	{
		if (Singleton<GlobalData>.Instance.MainPageAdvertisementCount > 0)
		{
			Singleton<GlobalData>.Instance.MainPageAdvertisementCount--;
			ItemDataManager.SetCurrency(CommonDataType.DIAMOND, 10);
			Singleton<UiManager>.Instance.ShowEarnMoneyEffect(0, 10, 0);
			this.RefleshPage();
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
	}

	public void ShowItemPopup(string name, Transform initPos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/ItemPopup")) as GameObject;
		gameObject.GetComponent<ItemPopup>().iconName = name;
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.position = initPos.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(true);
	}

	public void ShowRole01()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.RolePage.ToString());
		this.NpcBtn[0].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curTechGO, 30);
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curTechGO, true, true);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.RolePage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.RolePage).GetComponent<RolePage>().selectIndex = 0;
		});
		NpcScenceControl.instance.curCamer.transform.DORotateQuaternion(NpcScenceControl.instance.techCamPos.localRotation, this.speed);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.techCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.curTechGO.GetComponent<NpcRoleControll>().SetAni(1);
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void ShowRole02()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.RolePage.ToString());
		this.NpcBtn[2].gameObject.SetActive(false);
		NpcScenceControl.instance.CanShow = false;
		this.Hide();
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curWeaponGO, 30);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curWeaponGO, true, true);
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.RolePage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.RolePage).GetComponent<RolePage>().selectIndex = 2;
		});
		Singleton<UiManager>.Instance.ShowTopBar(true);
		NpcScenceControl.instance.curCamer.transform.DORotateQuaternion(NpcScenceControl.instance.weaponCamPos.localRotation, this.speed);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.weaponCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			NpcScenceControl.instance.curWeaponGO.GetComponent<NpcRoleControll>().SetAni(1);
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void ShowRole03()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(this.Name.ToString(), PageName.RolePage.ToString());
		this.NpcBtn[3].gameObject.SetActive(false);
		this.Hide();
		NpcScenceControl.instance.CanShow = false;
		NpcScenceControl.instance.openBlur = true;
		Singleton<UiManager>.Instance.CanBack = false;
		NpcScenceControl.instance.SetLayer(NpcScenceControl.instance.curPropGO, 30);
		NpcScenceControl.instance.eventSystem.enabled = false;
		NpcScenceControl.instance.LookNpc(NpcScenceControl.instance.curPropGO, true, true);
		NpcScenceControl.instance.ChangeView(true);
		Singleton<UiManager>.Instance.ShowPage(PageName.RolePage, delegate()
		{
			Singleton<UiManager>.Instance.GetPage(PageName.RolePage).GetComponent<RolePage>().selectIndex = 1;
		});
		NpcScenceControl.instance.curCamer.transform.DORotateQuaternion(NpcScenceControl.instance.propCamPos.localRotation, this.speed);
		NpcScenceControl.instance.curCamer.transform.DOLocalMove(NpcScenceControl.instance.propCamPos.localPosition, this.speed, false).OnComplete(delegate
		{
			NpcScenceControl.instance.eventSystem.enabled = true;
			Singleton<UiManager>.Instance.CanBack = true;
			NpcScenceControl.instance.curPropGO.GetComponent<NpcRoleControll>().SetAni(1);
			Singleton<UiManager>.Instance.ShowTopBar(true);
		});
	}

	public void ClickOnAchievement()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.AchievementPage, null);
	}

	public void ClickOnPushGift(int id)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPushGift(id);
	}

	public void ClickOnGreatDeals()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (this.pushData != null)
		{
			Singleton<UiManager>.Instance.ShowPushGift(this.pushData.ID);
		}
	}

	public void ClickOnLogonAward()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.LogonAwardPage, null);
	}

	public void ClickOnDailyLimit()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.DailyLimitedPage, null);
	}

	public void ClickOnDailyMission()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.DailyMissionPage, null);
	}

	public void ClickOnLevelAward()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.ShowPage(PageName.EachLevelGiftPage, null);
	}

	public void ClickOnLottery()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(PageName.MainPage.ToString(), PageName.AdvertisementLotteryPage.ToString());
		Singleton<UiManager>.Instance.ShowPage(PageName.AdvertisementLotteryPage, null);
	}

	public void ClickOnHallowmasGift()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		GameLogManager.SendPageLog(PageName.MainPage.ToString(), PageName.HallowmasGiftPage.ToString());
		Singleton<UiManager>.Instance.ShowPage(PageName.HallowmasGiftPage, null);
	}

	public static MainPage instance;

	public Transform WordBtnTran;

	public Transform EquipBtnTran;

	public GameObject OpenBoxGo;

	public Text equipTxt;

	public Text worldTxt;

	public Text openBoxTxt;

	public Text equipNpcTxt;

	public Text propNpcTxt;

	public Text weaponNpcTxt;

	public Transform[] NpcBtn;

	public Transform[] NpcTrans;

	public Transform[] GuideTrans;

	public CanvasGroup canvasGroup;

	[CNName("移动速度")]
	public float speed = 0.5f;

	public bool openTech;

	public GameObject PushBtn;

	public UILabelTick PushTime;

	public GameObject SignBtn;

	public Text OpenBoxBtnTxt;

	public GameObject DebrisGo;

	public Text NumDebris;

	public Slider DebrisSlider;

	public Image DebrisItemIcon;

	private bool isTeaching;

	private UITeachPage curTeachPage;

	public CommonGiftButton AchievementButton;

	public CommonGiftButton DailyMissionButton;

	public CommonGiftButton LogonAwardButton;

	public CommonGiftButton DailyLimitButton;

	public CommonGiftButton LevelAwardButton;

	public CommonGiftButton LotteryButton;

	public Button AdButton;

	public Text AdButtonName;

	public Button GreatDealsButton;

	public Text GreatDealsName;

	public Button GiftMp5Button;

	public Text GiftMp5Name;

	public Button HallowmasGiftButton;

	public Text HallowmasGiftName;

	private int PushGiftCounts;

	private List<int> PushCheckpoints = new List<int>
	{
		5,
		8,
		12,
		14,
		21,
		24,
		33,
		38
	};

	private StoreData pushData;

	private TeachUIType curType;
}
