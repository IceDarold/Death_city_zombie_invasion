using System;
using DataCenter;
using UnityEngine;
using UnityEngine.Events;

public static class UITick
{
	public static int getNowSec()
	{
		UITick.startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		return (int)(DateTime.Now - UITick.startTime).TotalSeconds;
	}

	public static void setRoleUpSec(RoleData rd, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("equiproleUp" + rd.Name, 0);
		}
		else
		{
			PlayerPrefs.SetInt("equiproleUp" + rd.Name, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setWeaponCreatSec(string name, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("wuqizhizao" + name, 0);
		}
		else
		{
			PlayerPrefs.SetInt("wuqizhizao" + name, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setWeaponUpSec(string name, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("wuqizhizaoUp" + name, 0);
		}
		else
		{
			PlayerPrefs.SetInt("wuqizhizaoUp" + name, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setItemSec(int id, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("wuping" + id, 0);
		}
		else
		{
			PlayerPrefs.SetInt("wuping" + id, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setItemSec(string id, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("wuping" + id, 0);
		}
		else
		{
			PlayerPrefs.SetInt("wuping" + id, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setNpcUpCreatSec(string name, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("npczhizao" + name, 0);
		}
		else
		{
			PlayerPrefs.SetInt("npczhizao" + name, UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setManufactureSec()
	{
		PlayerPrefs.SetInt("mianfeizhizaojishijiandian", UITick.getNowSec());
		PlayerPrefs.Save();
	}

	public static void setLotterySec()
	{
		PlayerPrefs.SetInt("mianfeichoujiangshijiandian", UITick.getNowSec());
		PlayerPrefs.Save();
	}

	public static void SetVipTime()
	{
		PlayerPrefs.SetString("VIPtuisong", DateTime.Now.ToString("yyyy-MM-dd"));
		PlayerPrefs.Save();
	}

	public static void setPushGiftSec(int id, int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("libaotuisong", 0);
			PlayerPrefs.SetInt("pushGiftType", 0);
		}
		else
		{
			PlayerPrefs.SetInt("libaotuisong", UITick.getNowSec());
			PlayerPrefs.SetInt("pushGiftType", id);
		}
		PlayerPrefs.Save();
	}

	public static void setBossLevel(int num)
	{
		PlayerPrefs.SetInt("bossLevel", num);
		PlayerPrefs.SetInt("bossLevelTime", UITick.getNowSec());
		if (DateTime.Now.Hour >= 12)
		{
			string s = DateTime.Now.AddDays(1.0).Date.ToString();
			DateTime d;
			if (DateTime.TryParse(s, out d))
			{
			}
			PlayerPrefs.SetInt("bossLevelAmOrPm", (int)(d - UITick.startTime).TotalSeconds - UITick.getNowSec());
		}
		else
		{
			DateTime now = DateTime.Now;
			DateTime d2 = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0, DateTimeKind.Local);
			PlayerPrefs.SetInt("bossLevelAmOrPm", (int)(d2 - UITick.startTime).TotalSeconds - UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static int getBossLevel()
	{
		return PlayerPrefs.GetInt("bossLevel", 1);
	}

	public static int getBossLevelTime()
	{
		return PlayerPrefs.GetInt("bossLevelTime", 0);
	}

	public static bool CheckBossIsUnlock()
	{
		if (UITick.getNowSec() - UITick.getBossLevelTime() > PlayerPrefs.GetInt("bossLevelAmOrPm", 0))
		{
			UITick.setBossLevel(1);
			return true;
		}
		return false;
	}

	public static string getBoosLevelSec()
	{
		int num = UITick.getNowSec() - UITick.getBossLevelTime();
		int needSec = PlayerPrefs.GetInt("bossLevelAmOrPm", 0) - num;
		return UITick.secToStr(needSec);
	}

	public static void setGoldLevelTimeAndSec(int num)
	{
		PlayerPrefs.SetInt("goldLevelTime", UITick.getGoldLevelTime() - num);
		PlayerPrefs.SetInt("goldLevelSec", UITick.getNowSec());
		PlayerPrefs.Save();
	}

	public static void setGoldLevelTime(int num = 2)
	{
		PlayerPrefs.SetInt("goldLevelTime", num);
		PlayerPrefs.SetString("secGoldLevelDay", DateTime.Now.ToString("yyyy-MM-dd"));
		PlayerPrefs.Save();
	}

	public static void setLimitLevelTime(int LevelId)
	{
		PlayerPrefs.SetInt("LimitLevelSec" + LevelId, UITick.getNowSec());
		PlayerPrefs.Save();
	}

	public static bool getGoldLevelTimeToDay()
	{
		return PlayerPrefs.GetString("secGoldLevelDay", string.Empty) != DateTime.Now.ToString("yyyy-MM-dd");
	}

	public static int getGoldLevelTime()
	{
		return PlayerPrefs.GetInt("goldLevelTime", 2);
	}

	public static string getGoldLevelSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("goldLevelSec", 0);
		int needSec = 3600 - num;
		return UITick.secToStr(needSec);
	}

	public static string getLimitLevelSec(int LevelId)
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("LimitLevelSec" + LevelId, 0);
		int needSec = CheckpointDataManager.GetCheckpointData(LevelId).TimeLimit - num;
		return UITick.secToStr(needSec);
	}

	public static void setADDITIONALSec(int num, int time = 0)
	{
		if (time == 0)
		{
			PlayerPrefs.SetInt("ADDITIONAL" + UITick.secADD[num], UITick.getNowSec());
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.SetInt("ADDITIONAL" + UITick.secADD[num], 0);
			PlayerPrefs.Save();
		}
	}

	public static string getADDITIONALSec(int num)
	{
		int num2 = UITick.getNowSec() - PlayerPrefs.GetInt("ADDITIONAL" + UITick.secADD[num], 0);
		int needSec = 86400 - num2;
		return UITick.secToStr(needSec);
	}

	public static void setLimitGiftSec(int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("xianshilibao", 0);
		}
		else
		{
			PlayerPrefs.SetInt("xianshilibao", UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setNumGiftSec(int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("xiancilibao", 0);
		}
		else
		{
			PlayerPrefs.SetString("xiancilibaodays", DateTime.Now.ToString("yyyy-MM-dd"));
			PlayerPrefs.SetInt("xiancilibao", UITick.getNowSec());
		}
		PlayerPrefs.Save();
	}

	public static void setVideoMainSec()
	{
		PlayerPrefs.SetInt("videomainshijiandian", UITick.getNowSec());
	}

	public static void setVideoBoxNum(int num = 1)
	{
		if (num == 0)
		{
			PlayerPrefs.SetInt("FreeBoxnum", 0);
		}
		else
		{
			PlayerPrefs.SetInt("FreeBoxnum", UITick.getVideoBoxNum() + num);
		}
	}

	public static int getVideoBoxNum()
	{
		return PlayerPrefs.GetInt("FreeBoxnum", 0);
	}

	public static void setVideoBoxSec()
	{
		PlayerPrefs.SetInt("FreeBox", UITick.getNowSec());
	}

	public static void setShopNeedSec()
	{
		PlayerPrefs.SetInt("videoShop", UITick.getNowSec());
		PlayerPrefs.Save();
	}

	public static string getRoleUpNeedSec(RoleData rd)
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("equiproleUp" + rd.Name, 0);
		int needSec = RoleDataManager.GetUpgradeTime(rd.ID) - num;
		return UITick.secToStr(needSec);
	}

	public static int getRoleUpRemindSec(RoleData rd)
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("equiproleUp" + rd.Name, 0);
		return RoleDataManager.GetUpgradeTime(rd.ID) - num;
	}

	public static string getItemNeedSec(int id, int time)
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("wuping" + id, 0);
		int needSec = time - num;
		return UITick.secToStr(needSec);
	}

	public static int getItemRemainderSec(int id, int time)
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("wuping" + id, 0);
		return time - num;
	}

	public static string getFreeManufacNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("mianfeizhizaojishijiandian", 0);
		int needSec = 14400 - num;
		return UITick.secToStr(needSec);
	}

	public static string getLotteryNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("mianfeichoujiangshijiandian", 0);
		int needSec = 21600 - num;
		return UITick.secToStr(needSec);
	}

	public static string getPushGiftNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("libaotuisong", 0);
		int needSec = 1800 - num;
		return UITick.secToStr(needSec);
	}

	public static int getPushGiftType()
	{
		return PlayerPrefs.GetInt("pushGiftType", 0);
	}

	public static string getLimitGiftNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("xianshilibao", 0);
		int needSec = 14400 - num;
		return UITick.secToStr(needSec);
	}

	public static bool getNumGiftNeedDay()
	{
		return PlayerPrefs.GetString("xiancilibaodays", string.Empty) != DateTime.Now.ToString("yyyy-MM-dd");
	}

	public static string getNumGiftNeedSec()
	{
		string s = DateTime.Now.AddDays(1.0).Date.ToString();
		DateTime d;
		if (DateTime.TryParse(s, out d))
		{
		}
		int needSec = (int)(d - UITick.startTime).TotalSeconds - UITick.getNowSec();
		return UITick.secToStr(needSec);
	}

	public static void ResetVipTime(UnityAction action)
	{
		string a = DateTime.Now.ToString("yyyy-MM-dd");
		DateTime dateTime;
		if (DateTime.TryParse(PlayerPrefs.GetString("VIPtuisong", string.Empty), out dateTime))
		{
		}
		string b = dateTime.AddDays(30.0).ToString("yyyy-MM-dd");
		if (a == b)
		{
			action();
		}
	}

	public static string getFreeBoxNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("FreeBox", 0);
		int needSec = 7200 - num;
		return UITick.secToStr(needSec);
	}

	public static string getFreeShopNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("videoShop", 0);
		int needSec = 3600 - num;
		return UITick.secToStr(needSec);
	}

	public static string getVideoMainNeedSec()
	{
		int num = UITick.getNowSec() - PlayerPrefs.GetInt("videomainshijiandian", 0);
		int needSec = 600 - num;
		return UITick.secToStr(needSec);
	}

	public static string secToStr(int needSec)
	{
		string text = string.Empty;
		if (needSec > 0)
		{
			if (needSec / 3600 >= 10)
			{
				text = Convert.ToString(needSec / 3600) + ":";
			}
			else if (needSec / 3600 >= 0)
			{
				text = "0" + Convert.ToString(needSec / 3600) + ":";
			}
			else
			{
				text = Convert.ToString(needSec / 3600) + ":00";
			}
			if (needSec % 3600 / 60 >= 10)
			{
				text = text + Convert.ToString(needSec % 3600 / 60) + ":";
			}
			else
			{
				text = text + "0" + Convert.ToString(needSec % 3600 / 60) + ":";
			}
			if (needSec % 60 / 10 > 0)
			{
				text = text + Convert.ToString(needSec % 60) + string.Empty;
			}
			else
			{
				text = text + "0" + Convert.ToString(needSec % 60) + string.Empty;
			}
		}
		return text;
	}

	private const string secManuFacture = "mianfeizhizaojishijiandian";

	private const string secLottery = "mianfeichoujiangshijiandian";

	private const string secPushGift = "libaotuisong";

	private const string secVIPGift = "VIPtuisong";

	private const string secADDITIONAL = "ADDITIONAL";

	private static string[] secADD = new string[]
	{
		"GOLD",
		"DIAMOND",
		"DNA"
	};

	private const string secLimitGift = "xianshilibao";

	private const string secNumGift = "xiancilibao";

	private const string secNumGiftDay = "xiancilibaodays";

	private const string secFreeBox = "FreeBox";

	private const string secShopNeed = "videoShop";

	private const string secVideoMain = "videomainshijiandian";

	private const string setWeaponCreat = "wuqizhizao";

	private const string setWeaponUp = "wuqizhizaoUp";

	private const string setItem = "wuping";

	private const string setItemUp = "wupingUp";

	private const string setNpcUpCreat = "npczhizao";

	private const string setRoleUp = "equiproleUp";

	private const string setTechUp = "techUp";

	private const string pushGiftType = "pushGiftType";

	private const string bossLevel = "bossLevel";

	private const string bossLevelTime = "bossLevelTime";

	private const string bossLevelAmOrPm = "bossLevelAmOrPm";

	private const string goldLevelTime = "goldLevelTime";

	private const string goldLevelSec = "goldLevelSec";

	private const string LimitLevelSec = "LimitLevelSec";

	private const string secGoldLevelDay = "secGoldLevelDay";

	private const int manuFactureNeedSec = 14400;

	private const int lotteryNeedSec = 21600;

	private const int ADDITIONALSec = 86400;

	private const int pushGiftNeedSec = 1800;

	private const int LimitGiftNeedSec = 14400;

	private const int NumGiftNeedSec = 86400;

	private const int videoMainNeedSec = 600;

	private const int videoBoxNeedSec = 7200;

	private const int videoShopNeedSec = 3600;

	private const int BossLevelTimeSec = 43200;

	private const int GoldLevelSec = 3600;

	public static DateTime startTime;
}
