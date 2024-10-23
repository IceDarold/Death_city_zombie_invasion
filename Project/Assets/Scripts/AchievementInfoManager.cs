using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class AchievementInfoManager : Singleton<AchievementInfoManager>
{
	private void Awake()
	{
		this.LoadXML();
		this.InitConfig();
	}

	public new void Init()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
		this.LoadXML();
		this.InitConfig();
	}

	private void LoadXML()
	{
		this.achievementInfoList = XmlHelper.ParseFileTextAsset<List<global::AchievementInfo>>(this.configName);
	}

	private void TestParseXML()
	{
		List<global::AchievementInfo> list = new List<global::AchievementInfo>();
		global::AchievementInfo item = new global::AchievementInfo(AchievementType.Achievement_GetCoin, "name", "desc", new AchievementItem[]
		{
			new AchievementItem(100.0, "10_15")
		});
		list.Add(item);
		XmlHelper.SaveFile(Application.dataPath + "/Resources/data/" + this.configName + ".xml", list);
	}

	private void InitConfig()
	{
		this.AddAchieveDic();
		this.InitAchieveDic();
	}

	public void AddAchieveDic()
	{
		if (this.achievementInfoList != null && this.achievementInfoList.Count > 0)
		{
			for (int i = 0; i < this.achievementInfoList.Count; i++)
			{
				if (!this.achieveDic.ContainsKey(this.achievementInfoList[i].AchiveType))
				{
					AchievementGroup achievementGroup = new AchievementGroup();
					achievementGroup.achievementInfo = this.achievementInfoList[i];
					this.achieveDic.Add(this.achievementInfoList[i].AchiveType, achievementGroup);
				}
			}
		}
	}

	public void InitAchieveDic()
	{
		foreach (KeyValuePair<AchievementType, AchievementGroup> keyValuePair in this.achieveDic)
		{
			keyValuePair.Value.totalProcess = keyValuePair.Value.achievementInfo.AchiItems.Length - 1;
			if (keyValuePair.Value.currentIndex <= keyValuePair.Value.totalProcess)
			{
				keyValuePair.Value.nextItem = keyValuePair.Value.achievementInfo.AchiItems[keyValuePair.Value.currentIndex];
			}
		}
	}

	public void SetRedPointShow()
	{
		if (Singleton<AchievementInfoManager>.Instance.IsAchievementComplete())
		{
		}
	}

	public bool IsAchievementComplete()
	{
		if (this.achieveDic != null && this.achieveDic.Count > 0)
		{
			foreach (KeyValuePair<AchievementType, AchievementGroup> keyValuePair in this.achieveDic)
			{
				if (keyValuePair.Value.isComplete && !keyValuePair.Value.isReceive)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public AchievementGroup GetAchieveGroup(AchievementType type)
	{
		return this.achieveDic[type];
	}

	public Dictionary<AchievementType, AchievementGroup> GetAchieveGatherDic()
	{
		return this.achieveDic;
	}

	public void JudgeAchievement()
	{
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_GetCoin, (double)this.GetNumByAchieveType(AchievementType.Achievement_GetCoin));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_GetDiamond, (double)this.GetNumByAchieveType(AchievementType.Achievement_GetDiamond));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_HeadShotCount, (double)this.GetNumByAchieveType(AchievementType.Achievement_HeadShotCount));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_HighestLevel, (double)this.GetNumByAchieveType(AchievementType.Achievement_HighestLevel));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_KillZombie, (double)this.GetNumByAchieveType(AchievementType.Achievement_KillZombie));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_LoginCount, (double)this.GetNumByAchieveType(AchievementType.Achievement_LoginCount));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_OwnGuns, (double)this.GetNumByAchieveType(AchievementType.Achievement_OwnGuns));
		AchievementSystem.Instance.CalculateAchievement(AchievementType.Achievement_UpgradeGunCount, (double)this.GetNumByAchieveType(AchievementType.Achievement_UpgradeGunCount));
	}

	private int GetNumByAchieveType(AchievementType _type)
	{
		double num = 0.0;
		switch (_type)
		{
		case AchievementType.Achievement_KillZombie:
			num = (double)Singleton<DynamicData>.Instance.totalKillNum;
			break;
		case AchievementType.Achievement_GetCoin:
			num = (double)this.gameState.Cash;
			break;
		case AchievementType.Achievement_GetDiamond:
			num = (double)this.gameState.Diamond;
			break;
		case AchievementType.Achievement_HighestLevel:
			num = (double)this.gameState.GetCompleteLevelNum();
			break;
		case AchievementType.Achievement_OwnGuns:
			foreach (Weapon weapon in this.gameState.GetWeapons())
			{
				num += (double)((weapon.Exist != WeaponExistState.Owned) ? 0 : 1);
			}
			break;
		case AchievementType.Achievement_UpgradeGunCount:
			foreach (Weapon weapon2 in this.gameState.GetWeapons())
			{
				num += (double)weapon2.AccuracyLevel;
				num += (double)weapon2.DamageLevel;
				num += (double)weapon2.ReloadLevel;
				num += (double)weapon2.ChargerLevel;
			}
			break;
		case AchievementType.Achievement_HeadShotCount:
			num = (double)Singleton<DynamicData>.Instance.totalHeadShotNum;
			break;
		}
		return (int)num;
	}

	private Dictionary<AchievementType, AchievementGroup> achieveDic = new Dictionary<AchievementType, AchievementGroup>();

	private List<global::AchievementInfo> achievementInfoList = new List<global::AchievementInfo>();

	private string configName = "AchievementConfig";

	protected bool isInit;

	protected GameState gameState;
}
