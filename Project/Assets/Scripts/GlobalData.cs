using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using inApps;
using ui;
using ui.debug;
using UnityEngine;
using UnityEngine.Events;
using Zombie3D;

public class GlobalData : Singleton<GlobalData>
{
	public string GameVersion
	{
		get
		{
			return PlayerPrefs.GetString("GameVersion", "Version_2.0");
		}
		set
		{
			PlayerPrefs.SetString("GameVersion", value);
		}
	}

	public int GameLogonTimes
	{
		get
		{
			return PlayerPrefs.GetInt("GAME_LOGON_TIMES", 0);
		}
		set
		{
			PlayerPrefs.SetInt("GAME_LOGON_TIMES", Mathf.Clamp(value, 0, int.MaxValue));
		}
	}

	public int RacingTimes
	{
		get
		{
			return PlayerPrefs.GetInt("RACING_TIMES", 0);
		}
		set
		{
			PlayerPrefs.SetInt("RACING_TIMES", value);
		}
	}

	public int Sensitivity
	{
		get
		{
			return this.sensitivity;
		}
		set
		{
			this.sensitivity = value;
			PlayerPrefs.SetInt("SAVE_KEY_SENSITIVITY", this.sensitivity);
		}
	}

	public int ShootingMode
	{
		get
		{
			return this.shooting_mode;
		}
		set
		{
			this.shooting_mode = value;
			PlayerPrefs.SetInt("SAVE_KEY_SHOOTINGMODE", this.shooting_mode);
		}
	}

	public int DebrisDropCount
	{
		get
		{
			return PlayerPrefs.GetInt("DEBRIS_DROP_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("DEBRIS_DROP_COUNT", value);
		}
	}

	public int FreeLotteryTimes
	{
		get
		{
			return PlayerPrefs.GetInt("FREE_LOTTERY_TIMES", 1);
		}
		set
		{
			PlayerPrefs.SetInt("FREE_LOTTERY_TIMES", value);
		}
	}

	public int AdvertisementLotteryTimes
	{
		get
		{
			return PlayerPrefs.GetInt("AD_LOTTERY_TIMES", 1);
		}
		set
		{
			PlayerPrefs.SetInt("AD_LOTTERY_TIMES", value);
		}
	}

	public int FinishPageAdvertisement
	{
		get
		{
			return PlayerPrefs.GetInt("FINISH_PAGE_AD", 3);
		}
		set
		{
			PlayerPrefs.SetInt("FINISH_PAGE_AD", value);
		}
	}

	public int MainPageAdvertisementCount
	{
		get
		{
			return PlayerPrefs.GetInt("DayMainSaveKey", 3);
		}
		set
		{
			PlayerPrefs.SetInt("DayMainSaveKey", value);
		}
	}

	public int SpeedUpAdvertisementCount
	{
		get
		{
			return PlayerPrefs.GetInt("DayAdSkipSaveKey", 3);
		}
		set
		{
			PlayerPrefs.SetInt("DayAdSkipSaveKey", value);
		}
	}

	public int StorePageAdvertisement
	{
		get
		{
			return PlayerPrefs.GetInt("STORE_PAGE_AD", 1);
		}
		set
		{
			PlayerPrefs.SetInt("STORE_PAGE_AD", value);
		}
	}

	public int SliverBoxAdvertisement
	{
		get
		{
			return PlayerPrefs.GetInt("SAVE_KEY_SLIVERBOX_ADVERTISEMENT", 1);
		}
		set
		{
			PlayerPrefs.SetInt("SAVE_KEY_SLIVERBOX_ADVERTISEMENT", value);
		}
	}

	public int AdvertisementReviveTimes
	{
		get
		{
			return PlayerPrefs.GetInt("GAME_REVIVE_AD", 1);
		}
		set
		{
			PlayerPrefs.SetInt("GAME_REVIVE_AD", value);
		}
	}

	public int FacebookNoticeCounts
	{
		get
		{
			return PlayerPrefs.GetInt("FACEBOOK_NOTICE_COUNT", 3);
		}
		set
		{
			PlayerPrefs.SetInt("FACEBOOK_NOTICE_COUNT", value);
		}
	}

	public static void PrintRealTime(string tag, bool caculateDuration = true)
	{
		float num = Time.realtimeSinceStartup - GlobalData.realTimeSpan;
		GlobalData.realTimeSpan = Time.realtimeSinceStartup;
	}

	public int FirstBox
	{
		get
		{
			return this.firstBox;
		}
		set
		{
			this.firstBox = value;
			PlayerPrefs.SetInt("FirstBoxSaveKey", this.firstBox);
		}
	}

	public int FirstMove
	{
		get
		{
			return this.firstMove;
		}
		set
		{
			this.firstMove = value;
			PlayerPrefs.SetInt("FirstMoveSaveKey", this.firstMove);
		}
	}

	public int FirAdvice
	{
		get
		{
			return this.firAdvice;
		}
		set
		{
			this.firAdvice = value;
			PlayerPrefs.SetInt("FirstAdviceSaveKey", this.firAdvice);
		}
	}

	public int FirstSpecial
	{
		get
		{
			return this.firstSpecial;
		}
		set
		{
			this.firstSpecial = value;
			PlayerPrefs.SetInt("FirstSpecialSaveKey", this.firstSpecial);
		}
	}

	public int FirstShoot
	{
		get
		{
			return this.firstShoot;
		}
		set
		{
			this.firstShoot = value;
			PlayerPrefs.SetInt("FirstShootSaveKey", this.firstShoot);
		}
	}

	public int FirstTalent
	{
		get
		{
			return this.firstTalent;
		}
		set
		{
			this.firstTalent = value;
			PlayerPrefs.SetInt("FirstTalentSaveKey", this.firstTalent);
		}
	}

	public int FirstEquipMent
	{
		get
		{
			return this.firstEquipMent;
		}
		set
		{
			this.firstEquipMent = value;
			PlayerPrefs.SetInt("FirstEquipMentSaveKey", this.firstEquipMent);
		}
	}

	public int FirstBoxGuide
	{
		get
		{
			return this.firstBoxGuide;
		}
		set
		{
			this.firstBoxGuide = value;
			PlayerPrefs.SetInt("FirstBoxGuideSaveKey", this.firstBoxGuide);
		}
	}

	public int FirstWeapon
	{
		get
		{
			return this.firstWeapon;
		}
		set
		{
			this.firstWeapon = value;
			PlayerPrefs.SetInt("FirstWeaponSaveKey", this.firstWeapon);
		}
	}

	public int FirstMedic
	{
		get
		{
			return this.firstMedic;
		}
		set
		{
			this.firstMedic = value;
			PlayerPrefs.SetInt("FirstMedicSaveKey", this.firstMedic);
		}
	}

	public int FirstShouLei
	{
		get
		{
			return this.firstShouLei;
		}
		set
		{
			this.firstShouLei = value;
			PlayerPrefs.SetInt("FirstShouLeiSaveKey", this.firstShouLei);
		}
	}

	public int FirstPaoTai
	{
		get
		{
			return this.firstPaoTai;
		}
		set
		{
			this.firstPaoTai = value;
			PlayerPrefs.SetInt("FirstPaoTaiSaveKey", this.firstPaoTai);
		}
	}

	public float RewardRatio
	{
		get
		{
			float num = 1f;
			if (ItemDataManager.GetCurrency(CommonDataType.DOUBLE) == 1)
			{
				num += 1f;
			}
			if (ItemDataManager.GetCurrency(CommonDataType.VIP) == 1)
			{
				num += 0.1f;
			}
			return num;
		}
	}

	public int TodayVIPReward
	{
		get
		{
			return this.todayVIPReward;
		}
		set
		{
			this.todayVIPReward = value;
			PlayerPrefs.SetInt("SAVE_KEY_VIPGIFTREWARD", this.todayVIPReward);
		}
	}

	private void Awake()
	{
		//GMGSDK.Init(new SdkCallbackManager());
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.GameVersion = "Version_2.0";
		DataManager.LoadAllResources();
		if (PlayerPrefs.GetString("UpdateCompensate", "null") != "V2.0")
		{
			PlayerPrefs.SetString("UpdateCompensate", "V2.0");
			if (CheckpointDataManager.GetCurrentCheckpoint().ID > 10)
			{
				this.UpdateCompensate = true;
			}
			else
			{
				this.UpdateCompensate = false;
			}
		}
		else
		{
			this.UpdateCompensate = false;
		}
		this.initData();
		this.initLanguage();
		UITick.ResetVipTime(delegate
		{
			StoreDataManager.ResetFunctionGift(9010);
		});
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		UnityEngine.Debug.Log("Daily_Refresh");
		if (GameTimeManager.getCurrentTime(CurrentTime.Date) != PlayerPrefs.GetString("SAVE_KEY_DATE", string.Empty))
		{
			PlayerPrefs.SetString("SAVE_KEY_DATE", GameTimeManager.getCurrentTime(CurrentTime.Date));
			LogonAwardSystem.SetLogon();
			PlayerDataManager.SetStatisticsDatas(PlayerStatistics.LogonDays, 1);
			AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.LOGIN_DAYS, 1);
			DailyMissionSystem.ResetDailyMission();
			StoreDataManager.ResetCountLimitGifts();
			PlayerPrefs.SetString("SHARE_FACEBOOK", "true");
			PlayerPrefs.SetInt("WEAPON_TRY_CHECKPOINT", 1);
			PlayerPrefs.SetInt("GOLD_CHECKPOINT", 2);
			int bossCheckpointCount = CheckpointDataManager.GetBossCheckpointCount();
			for (int i = 0; i < bossCheckpointCount; i++)
			{
				int num = i;
				PlayerPrefs.SetInt("BOSS_CHECKPOINT_" + num, 0);
			}
			this.DebrisDropCount = 0;
			this.TodayVIPReward = 1;
			this.FreeLotteryTimes = 1;
			//this.SpeedUpAdvertisementCount = this.ParseServerAdvertisementData(GAME_AD_POSITION.EQUIPMENT_PAGE, "1_3");
			//this.MainPageAdvertisementCount = this.ParseServerAdvertisementData(GAME_AD_POSITION.MAIN_PAGE, "1_3");
			//this.AdvertisementLotteryTimes = this.ParseServerAdvertisementData(GAME_AD_POSITION.LOTTERY_AD, "1_1");
			//this.StorePageAdvertisement = this.ParseServerAdvertisementData(GAME_AD_POSITION.STORE_PAGE, "1_1");
			//this.FinishPageAdvertisement = this.ParseServerAdvertisementData(GAME_AD_POSITION.RESULT_PAGE, "1_3");
			//this.SliverBoxAdvertisement = this.ParseServerAdvertisementData(GAME_AD_POSITION.OPEN_BOX_PAGE, "1_1");
			//this.PushAdevertisementCounts = this.ParseServerAdvertisementData(GAME_AD_POSITION.PUSH_AD, "1_1");
			//this.AdvertisementReviveTimes = this.ParseServerAdvertisementData(GAME_AD_POSITION.PUSH_AD, "1_1");
		}
		_debugPanel.Init();
		yield break;
	}

	private void initData()
	{
		this.GameLogonTimes++;
		this.sensitivity = PlayerPrefs.GetInt("SAVE_KEY_SENSITIVITY", 15);
		//this.shooting_mode = PlayerPrefs.GetInt("SAVE_KEY_SHOOTINGMODE", 0);
		this.todayVIPReward = PlayerPrefs.GetInt("SAVE_KEY_VIPGIFTREWARD");
		this.firstPaoTai = PlayerPrefs.GetInt("FirstPaoTaiSaveKey", 1);
		this.firstMove = PlayerPrefs.GetInt("FirstMoveSaveKey", 1);
		this.firstShoot = PlayerPrefs.GetInt("FirstShootSaveKey", 1);
		this.firstMedic = PlayerPrefs.GetInt("FirstMedicSaveKey", 2);
		this.firstWeapon = PlayerPrefs.GetInt("FirstWeaponSaveKey", 2);
		this.firstEquipMent = PlayerPrefs.GetInt("FirstEquipMentSaveKey", 3);
		this.firstTalent = PlayerPrefs.GetInt("FirstTalentSaveKey", 1);
		this.firstShouLei = PlayerPrefs.GetInt("FirstShouLeiSaveKey", 2);
		this.firstBox = PlayerPrefs.GetInt("FirstBoxSaveKey", 1);
		this.firstBoxGuide = PlayerPrefs.GetInt("FirstBoxGuideSaveKey", 1);
		this.firstSpecial = PlayerPrefs.GetInt("FirstSpecialSaveKey", 1);
		this.firAdvice = PlayerPrefs.GetInt("FirstAdviceSaveKey", 1);
		if (PlayerPrefs.HasKey("SAVE_KEY_SHOOTINGMODE"))
		{
			this.shooting_mode = PlayerPrefs.GetInt("SAVE_KEY_SHOOTINGMODE", 0);
		}
		else
		{
			this.shooting_mode = UiControllers.Instance.IsMobile ? 0 : 1;
		}
	}

	public int GetSkipDiamond(int sec)
	{
		return (int)(10.0 * Math.Pow((double)(sec / 60 + 1), 0.76923079744598677));
	}

	private IEnumerator GetServerTime()
	{
		yield return null;
		//GMGSDK.GetGamePromotionData(false, 101, delegate(List<GameShowData> datas)
		//{
		//	if (datas == null || datas.Count == 0)
		//	{
		//		return;
		//	}
		//});
		yield break;
	}

	public int GetMaxEnergy()
	{
		if (ItemDataManager.GetCommonItem(CommonDataType.VIP).Count == 1)
		{
			return 16;
		}
		return 8;
	}

	private void SetEnergyTimer()
	{
		this.EnergyTimer = PlayerPrefs.GetFloat("GameEnergyTimer", 0f);
		this.EnergyTimer -= (float)GameTimeManager.CalculateTimeToNow("Energy");
		this.EnergyTimer = Mathf.Clamp(this.EnergyTimer, 0f, (float)(this.GetMaxEnergy() * 900));
	}

	public void SetEnergy(int value)
	{
		this.SetEnergyTimer();
		this.EnergyTimer -= (float)(value * 900);
		this.EnergyTimer = Mathf.Clamp(this.EnergyTimer, 0f, (float)(this.GetMaxEnergy() * 900));
		GameTimeManager.RecordTime("Energy");
		PlayerPrefs.SetFloat("GameEnergyTimer", this.EnergyTimer);
		if (Singleton<UiManager>.Instance.TopBar != null)
		{
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
	}

	public int GetEnergy()
	{
		this.SetEnergyTimer();
		if (this.EnergyTimer > 0f)
		{
			return this.GetMaxEnergy() - Mathf.CeilToInt(this.EnergyTimer / 900f);
		}
		return this.GetMaxEnergy();
	}

	public float GetEnergyRecoveryTime()
	{
		this.SetEnergyTimer();
		return this.EnergyTimer % 900f;
	}

	public void EnergyBackToFull()
	{
		this.EnergyTimer = 0f;
		GameTimeManager.RecordTime("Energy");
		PlayerPrefs.SetFloat("GameEnergyTimer", this.EnergyTimer);
		if (Singleton<UiManager>.Instance != null)
		{
			Singleton<UiManager>.Instance.TopBar.Refresh();
		}
	}

	private void initLanguage()
	{
		this.CurrentLanguage = GetSystemLanguage() == SystemLanguage.Russian ? LanguageEnum.Russian : LanguageEnum.English;
		this.SetGameLanguage(this.CurrentLanguage);
	}

	public LanguageEnum GetCurrentLanguage()
	{
		return this.CurrentLanguage;
	}

	private SystemLanguage GetSystemLanguage()
	{
		return Application.systemLanguage;
	}

	public void SetGameLanguage(LanguageEnum lan)
	{
		this.CurrentLanguage = lan;
		this.Language = this.FilterLanguage(lan);
		PlayerPrefs.SetInt("SAVE_KEY_LANGUAGE", (int)lan);
		Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.LanguageChanged);
	}

	private Dictionary<string, string> FilterLanguage(LanguageEnum lan)
	{
		List<LanguageData> list = DataManager.ParseXmlData<LanguageData>("LanguageData", "LanguageDatas", "LanguageData");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			string keyword = list[i].Keyword;
			switch (lan)
			{
			case LanguageEnum.English:
				dictionary.Add(list[i].Keyword, list[i].English);
				break;
			case LanguageEnum.Russian:
				dictionary.Add(list[i].Keyword, list[i].Russian);
				break;
			// case LanguageEnum.Chinese:
			// 	dictionary.Add(list[i].Keyword, list[i].Chinese);
			// 	break;
			// case LanguageEnum.Japanese:
			// 	dictionary.Add(list[i].Keyword, list[i].Japanese);
			// 	break;
			// case LanguageEnum.Korean:
			// 	dictionary.Add(list[i].Keyword, list[i].Korean);
			// 	break;
			// case LanguageEnum.Spanish:
			// 	dictionary.Add(list[i].Keyword, list[i].Spanish);
			// 	break;
			// case LanguageEnum.Arabic:
			// 	dictionary.Add(list[i].Keyword, list[i].Arabic);
			// 	break;
			}
		}
		return dictionary;
	}

	public string GetText(string str)
	{
		if (this.Language.ContainsKey(str))
		{
			return this.Language[str];
		}
		return str;
	}

	public void ShowAdvertisement(int pos, UnityAction callback = null, UnityAction callback_1 = null)
	{
		//if (pos < 0)
		//{
		//	GMGSDK.ShowAdAndVideo(pos, delegate(bool result, global::PushType adType, global::AdPlayResultCode code, int cdTime)
		//	{
		//		if (result)
		//		{
		//			DailyMissionSystem.SetDailyMission(DailyMissionType.WATCH_VIDEO, 1);
		//			if (callback != null)
		//			{
		//				callback();
		//			}
		//		}
		//		else
		//		{
		//			if (callback_1 != null)
		//			{
		//				callback_1();
		//			}
		//			string text = string.Empty;
		//			if (code == global::AdPlayResultCode.上限)
		//			{
		//				text = this.GetText("NO_MORE_TIMES_TODAY") + "\n" + this.GetText("PLEASE_TRY_TOMORROW");
		//			}
		//			else if (code == global::AdPlayResultCode.CD)
		//			{
		//				text = string.Concat(new object[]
		//				{
		//					this.GetText("PLAY_FAILED"),
		//					"\n",
		//					this.GetText("PLEASE_WAIT"),
		//					" ",
		//					code,
		//					this.GetText("SECONDS")
		//				});
		//			}
		//			else
		//			{
		//				text = this.GetText("PLAY_FAILED");
		//			}
		//			if (!string.IsNullOrEmpty(text))
		//			{
		//				Singleton<UiManager>.Instance.ShowMessage(text, 0.5f);
		//			}
		//		}
		//	});
		//}
		//else
		//{
		//	GMGSDK.ShowAdAndVideo(pos, delegate(bool result, global::PushType adType, global::AdPlayResultCode code, int cdTime)
		//	{
		//	});
		//}
	}

	public void FacebookShare(UnityAction _callback)
	{
		if (PlayerPrefs.GetString("SHARE_FACEBOOK", "true").Equals("true"))
		{
			//GMGSDK.ShareFacebookDynmic(delegate(bool result)
			//{
			//	if (result)
			//	{
			//		PlayerPrefs.SetString("SHARE_FACEBOOK", "false");
			//		_callback();
			//	}
			//	else
			//	{
			//		Singleton<UiManager>.Instance.ShowMessage(this.GetText("SHARE_FAILD"), 0.5f);
			//	}
			//});
		}
		else
		{
			Singleton<UiManager>.Instance.ShowMessage(this.GetText("SHARED_TODAY"), 0.5f);
		}
	}

	public void DoCharge(int id, UnityAction success)
	{
		if (this.IngoreChange)
		{
			if (PlayerPrefs.GetString("FirstPay", "true") == "true")
			{
				PlayerPrefs.SetString("FirstPay", "false");
				ItemDataManager.SetCurrency(CommonDataType.BOX_GOLDEN, 3);
			}
			success();
		}
		else
		{
			Singleton<InApps>.Instance.Purchase(id, () =>
			{
				if (PlayerPrefs.GetString("FirstPay", "true") == "true")
				{
					PlayerPrefs.SetString("FirstPay", "false");
					ItemDataManager.SetCurrency(CommonDataType.BOX_GOLDEN, 3);
				}
				success();
				GameLogManager.SendPageLog("ChargeID: " + id, "MaxLevel: " + CheckpointDataManager.GetCurrentCheckpoint().ID);
			});
			// GMGSDK.DoCharge(id, true, 0, 0, delegate(bool result)
			// {
			// 	if (result)
			// 	{
			// 		if (PlayerPrefs.GetString("FirstPay", "true") == "true")
			// 		{
			// 			PlayerPrefs.SetString("FirstPay", "false");
			// 			ItemDataManager.SetCurrency(CommonDataType.BOX_GOLDEN, 3);
			// 		}
			// 		success();
			// 		GameLogManager.SendPageLog("ChargeID: " + id, "MaxLevel: " + CheckpointDataManager.GetCurrentCheckpoint().ID);
			// 	}
			// 	else
			// 	{
			// 		Singleton<UiManager>.Instance.ShowMessage(this.GetText("BUY_FAILED"), 0.5f);
			// 	}
			// });
		}
	}

	//public int ParseServerAdvertisementData(GAME_AD_POSITION position, string def)
    //{
    //    string serverParamS = GMGSDK.GetServerParamS(position.ToString(), def);
    //    Write.Log(position.ToString() + " : " + serverParamS);
    //    string[] array = serverParamS.Split(new char[]
    //    {
    //        '_'
    //    });
    //    if (array[0] == "0")
    //    {
    //        return 0;
    //    }
    //    return int.Parse(array[1]);
    //}

    public const string ServerPath = "";

	public const string PlayerSaveKey = "SAVE_KEY_PLAYER";

	public const string SensitivitySaveKey = "SAVE_KEY_SENSITIVITY";

	public const string ShootingModeSaveKey = "SAVE_KEY_SHOOTINGMODE";

	public const string MusicSaveKey = "SAVE_KEY_MUSIC";

	public const string SoundSaveKey = "SAVE_KEY_SOUND";

	public const string DateSaveKey = "SAVE_KEY_DATE";

	public const string LanguageSaveKey = "SAVE_KEY_LANGUAGE";

	public const string VIPGiftSaveKey = "SAVE_KEY_VIPGIFT";

	public const string TimeVIPGiftSaveKey = "SAVE_KEY_TIMEVIPGIFT";

	public const string FreeSliverBoxSaveKey = "SAVE_KEY_FreeSliverBox";

	public const string AdSliverBoxSaveKey = "SAVE_KEY_SLIVERBOX_ADVERTISEMENT";

	public const string DailyMissionSaveKey = "SAVE_KEY_DAILYMISSION";

	public const string MonthlyCard = "MONTHLY_CARD";

	public const string MonthlyCardDailyAward = "MONTHLY_CARD_DAILY_AWARD";

	public const string VipDailyAward = "VIP_DAILY_AWARD";

	public const string CommonItemSaveKey = "CommonItemDatas";

	public const string WeaponSaveKey = "WeaponDatas";

	public const string EquipmentSaveKey = "EquipmentDatas";

	public const string PropSaveKey = "PropDatas";

	public const string DebrisSaveKey = "DebrisDatas";

	public const string AchievementSaveKey = "AchievementDatas";

	public const string RoleSaveKey = "RoleDatas";

	public const string TalentSaveKey = "TalentDatas";

	public const string CheckpointSaveKey = "CheckpointDatas";

	public const string StoreSaveKey = "StoreDatas";

	public const string BoxSaveKey = "BoxDatas";

	public const string LevelPassGiftSaveKey = "LevelPassGiftDatas";

	public const string DayAdSkipSaveKey = "DayAdSkipSaveKey";

	public const string DayMainSaveKey = "DayMainSaveKey";

	public const string FreeLotterySaveKey = "FREE_LOTTERY_TIMES";

	public const string AdvertisementLotterySaveKey = "AD_LOTTERY_TIMES";

	public const string FirstAdviceSaveKey = "FirstAdviceSaveKey";

	public const string FirstPaoTaiSaveKey = "FirstPaoTaiSaveKey";

	public const string FirstMoveSaveKey = "FirstMoveSaveKey";

	public const string FirstBoxSaveKey = "FirstBoxSaveKey";

	public const string FirstShootSaveKey = "FirstShootSaveKey";

	public const string FirstSpecialSaveKey = "FirstSpecialSaveKey";

	public const string FirstMedicSaveKey = "FirstMedicSaveKey";

	public const string FirstWeaponSaveKey = "FirstWeaponSaveKey";

	public const string FirstBoxGuideSaveKey = "FirstBoxGuideSaveKey";

	public const string FirstTalentSaveKey = "FirstTalentSaveKey";

	public const string FirstEquipMentSaveKey = "FirstEquipMentSaveKey";

	public const string FirstShouLeiSaveKey = "FirstShouLeiSaveKey";

	public const string SNIPE_GUIDE_RECORDER = "SNIPE_GUIDE_RECORDER";

	public const int TimeToDiamond = 10;

	public const int EnergyRecoveryTime = 900;

	public const int VipLoginReward = 80;

	public const int PickDNA = 10;

	public const string AnimatorOpen = "Open";

	public const string AnimatorCardOpen = "OpenCard";

	public static Vector2 ScreenSize = new Vector2((float)Screen.width, (float)Screen.height);

	public bool isDebug = true;

	public bool IngoreChange = true;

	public bool UpdateCompensate;

	public int CheckpointFinishTimes;

	[HideInInspector]
	public int InGameWeaponIndex;

	[HideInInspector]
	public int selectWeapon;

	[HideInInspector]
	public int selectProp;

	[HideInInspector]
	public int GameGoldCoins;

	[HideInInspector]
	public int GameDNA;

	[HideInInspector]
	public WeaponData UnlockWd;

	[HideInInspector]
	public WeaponData UnlockShowWd;

	[HideInInspector]
	public List<DebrisData> ListGameDebris = new List<DebrisData>();

	public List<DebrisData> ListDropDebris = new List<DebrisData>();

	public int InGameState;

	public DebrisData InGameCurItem;

	private float EnergyTimer;

	private LanguageEnum CurrentLanguage;

	private Dictionary<string, string> Language = new Dictionary<string, string>();

	private int sensitivity;

	private int shooting_mode;

	public int PushAdevertisementCounts = 1;

	public static float realTimeSpan;

	[SerializeField] private DebugPanel _debugPanel;

	private int firstBox = 1;

	private int firstMove = 1;

	private int firAdvice = 1;

	private int firstSpecial = 1;

	private int firstShoot = 1;

	private int firstTalent = 1;

	private int firstEquipMent = 3;

	private int firstBoxGuide = 1;

	private int firstWeapon = 2;

	private int firstMedic = 2;

	private int firstShouLei = 2;

	private int firstPaoTai = 1;

	private int todayVIPReward = 1;

	public float GameTimes;
}
