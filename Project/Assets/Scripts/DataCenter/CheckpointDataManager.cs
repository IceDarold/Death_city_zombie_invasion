using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class CheckpointDataManager
	{
		static CheckpointDataManager()
		{
			CheckpointDataManager.SuccessTimes = 0;
			CheckpointDataManager.select_checkpoint = new CheckpointData
			{
				ID = -1,
				SceneID = -1
			};
			CheckpointDataManager.init();
		}

		public static CheckpointData SelectCheckpoint
		{
			get
			{
				return CheckpointDataManager.select_checkpoint;
			}
			set
			{
				CheckpointDataManager.select_checkpoint = value;
			}
		}

		private static void init()
		{
			CheckpointDataManager.checkpoints = DataManager.ParseXmlData<CheckpointData>("CheckpointData", "CheckpointDatas", "CheckpointData");
			CheckpointDataManager.scenes = DataManager.ParseXmlData<CheckpointScene>("CheckpointScene", "CheckpointScenes", "CheckpointScene");
			if (PlayerPrefs.GetString("InitCheckponitData", "true") == "true")
			{
				PlayerPrefs.SetString("InitCheckponitData", "false");
				for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
				{
					if (CheckpointDataManager.checkpoints[i].Chapters == ChapterEnum.CHAPTERNAME_01 && CheckpointDataManager.checkpoints[i].Index == 1)
					{
						CheckpointDataManager.checkpoints[i].Unlock = true;
					}
					else
					{
						CheckpointDataManager.checkpoints[i].Unlock = false;
					}
					CheckpointDataManager.checkpoints[i].Passed = false;
				}
				CheckpointDataManager.save();
			}
			else
			{
				CheckpointDataManager.read();
			}
		}

		public static List<CheckpointData> GetCheckpoints()
		{
			return CheckpointDataManager.checkpoints;
		}

		public static List<CheckpointScene> GetCheckpointScene()
		{
			return CheckpointDataManager.scenes;
		}

		public static CheckpointData GetCheckpointData(int id)
		{
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].ID == id)
				{
					return CheckpointDataManager.checkpoints[i];
				}
			}
			return null;
		}

		public static CheckpointData GetCheckpointData(ChapterEnum chapter, int index)
		{
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Chapters == chapter && CheckpointDataManager.checkpoints[i].Index == index)
				{
					return CheckpointDataManager.checkpoints[i];
				}
			}
			return null;
		}

		public static CheckpointData GetCheckPointDataBySceneID(int _SceneID)
		{
			return CheckpointDataManager.checkpoints.Find((CheckpointData data) => data.SceneID == _SceneID);
		}

		public static List<CheckpointData> GetMainLineList()
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.MAINLINE || CheckpointDataManager.checkpoints[i].Type == CheckpointType.RACING || CheckpointDataManager.checkpoints[i].Type == CheckpointType.MAINLINE_SNIPE)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			list.Sort(delegate(CheckpointData a, CheckpointData b)
			{
				if (a.Chapters != b.Chapters)
				{
					return a.Chapters.CompareTo(b.Chapters);
				}
				return a.Index.CompareTo(b.Index);
			});
			return list;
		}

		public static CheckpointData GetMaxPassedCheckpoint()
		{
			List<CheckpointData> mainLineList = CheckpointDataManager.GetMainLineList();
			CheckpointData result = mainLineList[0];
			for (int i = 0; i < mainLineList.Count; i++)
			{
				if (mainLineList[i].Passed)
				{
					result = mainLineList[i];
				}
			}
			return result;
		}

		public static CheckpointData GetCurrentCheckpoint()
		{
			List<CheckpointData> mainLineList = CheckpointDataManager.GetMainLineList();
			for (int i = 0; i < mainLineList.Count; i++)
			{
				if (!mainLineList[i].Passed)
				{
					return mainLineList[i];
				}
			}
			return mainLineList[mainLineList.Count - 1];
		}

		public static int GetCurrentCheckpointIndex()
		{
			List<CheckpointData> mainLineList = CheckpointDataManager.GetMainLineList();
			for (int i = 0; i < mainLineList.Count; i++)
			{
				if (!mainLineList[i].Passed)
				{
					return i + 1;
				}
			}
			return mainLineList.Count;
		}

		public static CheckpointData GetRandomCheckpoint()
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type > (CheckpointType)10 && CheckpointDataManager.checkpoints[i].Chapters <= CheckpointDataManager.GetMaxPassedCheckpoint().Chapters)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				return list[index];
			}
			return null;
		}

		public static int GetBossCheckpointCount()
		{
			int num = 0;
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.BOSS)
				{
					num++;
				}
			}
			return num;
		}

		public static CheckpointData GetBossCheckpoint(int index)
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.BOSS)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			list.Sort((CheckpointData a, CheckpointData b) => a.ID.CompareTo(b.ID));
			return list[index];
		}

		public static CheckpointData GetGoldCheckpoint()
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.GOLD)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				return list[index];
			}
			return null;
		}

		public static CheckpointData GetSnipeCheckpoint()
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.SNIPE)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			list.Sort((CheckpointData a, CheckpointData b) => a.ID.CompareTo(b.ID));
			for (int j = 0; j < list.Count; j++)
			{
				if (!list[j].Passed)
				{
					return list[j];
				}
			}
			return list[list.Count - 1];
		}

		public static CheckpointData GetWeaponCheckpoint()
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Type == CheckpointType.WEAPON)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				return list[index];
			}
			return null;
		}

		public static List<CheckpointData> GetChaptersLine(ChapterEnum chapter)
		{
			List<CheckpointData> list = new List<CheckpointData>();
			for (int i = 0; i < list.Count; i++)
			{
				if (CheckpointDataManager.checkpoints[i].Chapters == chapter)
				{
					list.Add(CheckpointDataManager.checkpoints[i]);
				}
			}
			return list;
		}

		public static string GetScene(int id)
		{
			for (int i = 0; i < CheckpointDataManager.scenes.Count; i++)
			{
				if (CheckpointDataManager.scenes[i].ID == id)
				{
					return CheckpointDataManager.scenes[i].Scene;
				}
			}
			return null;
		}

		public static CheckpointScene GetCheckpointScene(int id)
		{
			for (int i = 0; i < CheckpointDataManager.scenes.Count; i++)
			{
				if (CheckpointDataManager.scenes[i].ID == id)
				{
					return CheckpointDataManager.scenes[i];
				}
			}
			return null;
		}

		public static void SetCheckpointPassed(CheckpointData data)
		{
			if (!data.Passed)
			{
				data.Passed = true;
				if (data.Type == CheckpointType.MAINLINE || data.Type == CheckpointType.RACING || data.Type == CheckpointType.MAINLINE_SNIPE)
				{
					List<CheckpointData> mainLineList = CheckpointDataManager.GetMainLineList();
					int num = mainLineList.IndexOf(data);
					if (num < mainLineList.Count - 1)
					{
						mainLineList[num + 1].Unlock = true;
					}
					if (data.Chapters == ChapterEnum.CHAPTERNAME_01 && data.Index == 5)
					{
						DailyMissionSystem.ResetDailyMission();
						GameLogManager.SendLotteryLog(1);
					}
					AchievementDataManager.SetAchievementValue(AchievementType.PASS_CHECKPOINT, 1);
				}
				CheckpointDataManager.save();
			}
		}

		public static void SetCheckpointPassed(ChapterEnum chapter, int index)
		{
			CheckpointData checkpointData = CheckpointDataManager.GetCheckpointData(chapter, index);
			if (!checkpointData.Passed)
			{
				checkpointData.Passed = true;
				AchievementDataManager.SetAchievementValue(AchievementType.PASS_CHECKPOINT, 1);
				List<CheckpointData> mainLineList = CheckpointDataManager.GetMainLineList();
				if (mainLineList.IndexOf(checkpointData) < mainLineList.Count - 1)
				{
					mainLineList[index + 1].Unlock = true;
				}
				CheckpointDataManager.save();
			}
		}

		private static void save()
		{
			CheckpointDataManager.SaveDatas.Clear();
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				CheckpointSaveData checkpointSaveData = new CheckpointSaveData();
				checkpointSaveData.ID = CheckpointDataManager.checkpoints[i].ID;
				checkpointSaveData.Unlock = CheckpointDataManager.checkpoints[i].Unlock;
				checkpointSaveData.Passed = CheckpointDataManager.checkpoints[i].Passed;
				CheckpointDataManager.SaveDatas.Add(checkpointSaveData);
			}
			DataManager.SaveToJson<CheckpointSaveData>("CheckpointDatas", CheckpointDataManager.SaveDatas);
		}

		private static void read()
		{
			CheckpointDataManager.SaveDatas = DataManager.ParseJson<CheckpointSaveData>("CheckpointDatas");
			for (int i = 0; i < CheckpointDataManager.checkpoints.Count; i++)
			{
				for (int j = 0; j < CheckpointDataManager.SaveDatas.Count; j++)
				{
					if (CheckpointDataManager.checkpoints[i].ID == CheckpointDataManager.SaveDatas[j].ID)
					{
						CheckpointDataManager.checkpoints[i].Unlock = CheckpointDataManager.SaveDatas[j].Unlock;
						CheckpointDataManager.checkpoints[i].Passed = CheckpointDataManager.SaveDatas[j].Passed;
					}
				}
			}
		}

		public const int OpenGoleLevel = 3;

		public const int OpenBossLevel = 5;

		public const int OpenWeaponLevel = 7;

		public const int OpenRandomLevel = 10;

		public const int OpenSniperLevel = 4;

		public const int LimitBossTime = 120;

		private static List<CheckpointData> checkpoints = new List<CheckpointData>();

		private static List<CheckpointScene> scenes = new List<CheckpointScene>();

		private static List<CheckpointSaveData> SaveDatas = new List<CheckpointSaveData>();

		public static int SuccessTimes = 0;

		private static CheckpointData select_checkpoint;
	}
}
