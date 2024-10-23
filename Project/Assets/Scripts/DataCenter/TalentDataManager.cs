using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class TalentDataManager
	{
		static TalentDataManager()
		{
			TalentDataManager.init();
		}

		public static void init()
		{
			TalentDataManager.talents = DataManager.ParseXmlData<TalentData>("TalentData", "TalentDatas", "TalentData");
			TalentDataManager.costs = DataManager.ParseXmlData<TalentCost>("TalentCost", "TalentCosts", "TalentCost");
			if (PlayerPrefs.GetString("InitTalentData", "true") == "true")
			{
				PlayerPrefs.SetString("InitTalentData", "false");
				for (int i = 0; i < TalentDataManager.talents.Count; i++)
				{
					if (TalentDataManager.talents[i].Tier == 1)
					{
						TalentDataManager.talents[i].Unlock = true;
					}
				}
				TalentDataManager.save();
			}
			else
			{
				TalentDataManager.read();
			}
		}

		public static List<TalentData> GetTalentTree(int id)
		{
			List<TalentData> list = new List<TalentData>();
			for (int i = 0; i < TalentDataManager.talents.Count; i++)
			{
				if (TalentDataManager.talents[i].Root == id)
				{
					list.Add(TalentDataManager.talents[i]);
				}
			}
			return list;
		}

		public static TalentData GetTalentData(int id)
		{
			for (int i = 0; i < TalentDataManager.talents.Count; i++)
			{
				if (TalentDataManager.talents[i].ID == id)
				{
					return TalentDataManager.talents[i];
				}
			}
			return null;
		}

		public static TalentData GetTalentData(int root, int tier, int index)
		{
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(root);
			for (int i = 0; i < talentTree.Count; i++)
			{
				if (talentTree[i].Tier == tier && talentTree[i].Index == index)
				{
					return talentTree[i];
				}
			}
			return null;
		}

		public static float GetTalentValue(Talent type)
		{
			RoleData roleData = RoleDataManager.GetRoleData(PlayerDataManager.Player.Role);
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(roleData.TalentTree);
			float num = 0f;
			for (int i = 0; i < talentTree.Count; i++)
			{
				if (talentTree[i].Type == type)
				{
					num += (float)talentTree[i].Value[talentTree[i].Level] * 0.0001f;
				}
			}
			return num;
		}

		public static void Upgrade(int id)
		{
			TalentData talentData = TalentDataManager.GetTalentData(id);
			talentData.Level++;
			AchievementDataManager.SetAchievementValue(AchievementType.TALENT_UPGRADE, 1);
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(talentData.Root);
			for (int i = 0; i < talentTree.Count; i++)
			{
				if (TalentDataManager.GetCurrentPoints(talentData.Root, 0) >= talentTree[i].RequirePoints)
				{
					talentTree[i].Unlock = true;
				}
			}
			TalentDataManager.save();
		}

		public static int GetCurrentPoints(int root, int tier = 0)
		{
			int num = 0;
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(root);
			for (int i = 0; i < talentTree.Count; i++)
			{
				if (tier == 0)
				{
					num += talentTree[i].Level;
				}
				else if (talentTree[i].Tier == tier)
				{
					num += talentTree[i].Level;
				}
			}
			return num;
		}

		public static int GetTotalPoints(int root, int tier = 0)
		{
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(root);
			int num = 0;
			for (int i = 0; i < talentTree.Count; i++)
			{
				if (tier == 0)
				{
					num += talentTree[i].MaxLevel;
				}
				else if (talentTree[i].Tier == tier)
				{
					num += talentTree[i].MaxLevel;
				}
			}
			return num;
		}

		public static int GetUnlockNeedPoints(int root, int tier)
		{
			if (tier < 1 || tier > 5)
			{
				UnityEngine.Debug.LogError("序号错误");
				return 0;
			}
			return TalentDataManager.GetTalentData(root, tier, 1).RequirePoints;
		}

		public static bool isTierUnlocked(int root, int tier)
		{
			if (tier < 1 || tier > 5)
			{
				UnityEngine.Debug.LogError("序号错误");
				return false;
			}
			return TalentDataManager.GetTalentData(root, tier, 1).Unlock;
		}

		private static int GetTalentCost(int level)
		{
			for (int i = 0; i < TalentDataManager.costs.Count; i++)
			{
				if (TalentDataManager.costs[i].Level == level)
				{
					return TalentDataManager.costs[i].Cost;
				}
			}
			return 0;
		}

		public static int GetUpgradeCost(int root)
		{
			int currentPoints = TalentDataManager.GetCurrentPoints(root, 0);
			if (TalentDataManager.GetCurrentPoints(root, 0) < TalentDataManager.GetTotalPoints(root, 0))
			{
				return TalentDataManager.GetTalentCost(currentPoints + 1);
			}
			return 0;
		}

		public static int GetTotalCost(int root)
		{
			int currentPoints = TalentDataManager.GetCurrentPoints(root, 0);
			int num = 0;
			for (int i = 0; i < TalentDataManager.costs.Count; i++)
			{
				if (TalentDataManager.costs[i].Level <= currentPoints)
				{
					num += TalentDataManager.costs[i].Cost;
				}
			}
			return num;
		}

		public static void Reset(int root)
		{
			List<TalentData> talentTree = TalentDataManager.GetTalentTree(root);
			int num = (int)((float)TalentDataManager.GetTotalCost(root) * 0.9f);
			ItemDataManager.SetCurrency(CommonDataType.DNA, num);
			for (int i = 0; i < talentTree.Count; i++)
			{
				talentTree[i].Unlock = false;
				talentTree[i].Level = 0;
			}
			for (int j = 0; j < talentTree.Count; j++)
			{
				if (TalentDataManager.GetCurrentPoints(root, 0) >= talentTree[j].RequirePoints)
				{
					talentTree[j].Unlock = true;
				}
			}
			TalentDataManager.save();
		}

		private static void save()
		{
			TalentDataManager.SaveDatas.Clear();
			for (int i = 0; i < TalentDataManager.talents.Count; i++)
			{
				TalentSaveData talentSaveData = new TalentSaveData();
				talentSaveData.ID = TalentDataManager.talents[i].ID;
				talentSaveData.Level = TalentDataManager.talents[i].Level;
				talentSaveData.Unlock = TalentDataManager.talents[i].Unlock;
				TalentDataManager.SaveDatas.Add(talentSaveData);
			}
			DataManager.SaveToJson<TalentSaveData>("TalentDatas", TalentDataManager.SaveDatas);
		}

		private static void read()
		{
			TalentDataManager.SaveDatas = DataManager.ParseJson<TalentSaveData>("TalentDatas");
			int num = 0;
			for (int i = 0; i < TalentDataManager.talents.Count; i++)
			{
				if (i - num < TalentDataManager.SaveDatas.Count && TalentDataManager.talents[i].ID == TalentDataManager.SaveDatas[i - num].ID)
				{
					TalentDataManager.talents[i].Level = TalentDataManager.SaveDatas[i - num].Level;
					TalentDataManager.talents[i].Unlock = TalentDataManager.SaveDatas[i - num].Unlock;
				}
				else
				{
					TalentDataManager.talents[i].Level = 0;
					TalentDataManager.talents[i].Unlock = false;
				}
			}
		}

		private static List<TalentData> talents = new List<TalentData>();

		private static List<TalentCost> costs = new List<TalentCost>();

		private static List<TalentSaveData> SaveDatas = new List<TalentSaveData>();
	}
}
