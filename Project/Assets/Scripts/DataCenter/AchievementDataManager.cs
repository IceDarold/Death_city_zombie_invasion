using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class AchievementDataManager
	{
		static AchievementDataManager()
		{
			AchievementDataManager.init();
		}

		public static List<AchievementData> Achievements
		{
			get
			{
				return AchievementDataManager.achievements;
			}
		}

		private static void init()
		{
			AchievementDataManager.achievements = DataManager.ParseXmlData<AchievementData>("AchievementData", "AchievementDatas", "AchievementData");
			if (PlayerPrefs.GetString("InitAchievementData", "true") == "true")
			{
				PlayerPrefs.SetString("InitAchievementData", "false");
				AchievementDataManager.save();
			}
			else
			{
				AchievementDataManager.read();
				AchievementDataManager.sort();
			}
		}

		public static AchievementData GetAchievementData(AchievementType type)
		{
			for (int i = 0; i < AchievementDataManager.achievements.Count; i++)
			{
				if (AchievementDataManager.achievements[i].Type == type)
				{
					return AchievementDataManager.achievements[i];
				}
			}
			return null;
		}

		public static AchievementData GetAchievementData(int id)
		{
			for (int i = 0; i < AchievementDataManager.achievements.Count; i++)
			{
				if (AchievementDataManager.achievements[i].ID == id)
				{
					return AchievementDataManager.achievements[i];
				}
			}
			return null;
		}

		public static string GetDescribe(AchievementData data)
		{
			string text = Singleton<GlobalData>.Instance.GetText(data.Describe);
			if (text.Contains("#value#") && data.Level < data.TargetValue.Length)
			{
				return text.Replace("#value#", data.AwardItem[data.Level].ToString());
			}
			return text;
		}

		public static void SetAchievementValue(AchievementType type, int value)
		{
			AchievementData achievementData = AchievementDataManager.GetAchievementData(type);
			if (achievementData != null)
			{
				if (achievementData.Tag == 1)
				{
					achievementData.CurrentValue += value;
				}
				else if (achievementData.Tag == 2)
				{
					achievementData.CurrentValue = value;
				}
				if (!achievementData.Completed && achievementData.CurrentValue >= achievementData.TargetValue[achievementData.Level])
				{
					achievementData.Reached = true;
				}
				AchievementDataManager.save();
				AchievementDataManager.sort();
			}
		}

		public static bool CheckAchievement()
		{
			for (int i = 0; i < AchievementDataManager.achievements.Count; i++)
			{
				if (!AchievementDataManager.achievements[i].Completed && AchievementDataManager.achievements[i].Reached)
				{
					return true;
				}
			}
			return false;
		}

		public static void EarnAward(AchievementData data)
		{
			data.Level++;
			if (data.Level >= data.TargetValue.Length)
			{
				data.Completed = true;
			}
			else if (data.CurrentValue >= data.TargetValue[data.Level])
			{
				data.Reached = true;
			}
			else
			{
				data.Reached = false;
			}
			AchievementDataManager.save();
			AchievementDataManager.sort();
		}

		private static void sort()
		{
			AchievementDataManager.achievements.Sort(delegate(AchievementData a, AchievementData b)
			{
				if (a.Completed ^ b.Completed)
				{
					return a.Completed.CompareTo(b.Completed);
				}
				if (a.Reached ^ b.Reached)
				{
					return b.Reached.CompareTo(a.Reached);
				}
				return a.ID.CompareTo(b.ID);
			});
		}

		private static void read()
		{
			AchievementDataManager.AchievementSaveDatas = DataManager.ParseJson<AchievementSaveData>("AchievementDatas");
			for (int i = 0; i < AchievementDataManager.achievements.Count; i++)
			{
				for (int j = 0; j < AchievementDataManager.AchievementSaveDatas.Count; j++)
				{
					if (AchievementDataManager.achievements[i].ID == AchievementDataManager.AchievementSaveDatas[j].ID)
					{
						AchievementDataManager.achievements[i].CurrentValue = AchievementDataManager.AchievementSaveDatas[j].CurrentValue;
						AchievementDataManager.achievements[i].Level = AchievementDataManager.AchievementSaveDatas[j].Level;
						AchievementDataManager.achievements[i].Reached = AchievementDataManager.AchievementSaveDatas[j].Reached;
						AchievementDataManager.achievements[i].Completed = AchievementDataManager.AchievementSaveDatas[j].Completed;
						break;
					}
				}
			}
		}

		private static void save()
		{
			AchievementDataManager.AchievementSaveDatas.Clear();
			for (int i = 0; i < AchievementDataManager.achievements.Count; i++)
			{
				AchievementSaveData achievementSaveData = new AchievementSaveData();
				achievementSaveData.ID = AchievementDataManager.achievements[i].ID;
				achievementSaveData.CurrentValue = AchievementDataManager.achievements[i].CurrentValue;
				achievementSaveData.Level = AchievementDataManager.achievements[i].Level;
				achievementSaveData.Reached = AchievementDataManager.achievements[i].Reached;
				achievementSaveData.Completed = AchievementDataManager.achievements[i].Completed;
				AchievementDataManager.AchievementSaveDatas.Add(achievementSaveData);
			}
			DataManager.SaveToJson<AchievementSaveData>("AchievementDatas", AchievementDataManager.AchievementSaveDatas);
		}

		private static List<AchievementData> achievements = new List<AchievementData>();

		private static List<AchievementSaveData> AchievementSaveDatas = new List<AchievementSaveData>();
	}
}
