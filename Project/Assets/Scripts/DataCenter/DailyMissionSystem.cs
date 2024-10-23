using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class DailyMissionSystem
	{
		static DailyMissionSystem()
		{
			DailyMissionSystem.missions = DataManager.ParseXmlData<DailyMission>("DailyMission", "DailyMissions", "DailyMission");
			if (PlayerPrefs.GetString("InitDailyMissionSystem") == "true")
			{
				PlayerPrefs.SetString("InitDailyMissionSystem", "false");
				
				//PlayerPrefs.SetString("InitDailyMissionSystem", "false");	
				DailyMissionSystem.ResetDailyMission();
				DailyMissionSystem.save();
			}
			else
			{
				DailyMissionSystem.read();
			}
		}

		public static DailyMission GetCurrentDailyMission()
		{
			return null;
		}

		public static List<DailyMission> GetCurrentMissions()
		{
			DailyMissionSystem.sort();
			return DailyMissionSystem.CurrentMissions;
		}

		public static void ResetDailyMission()
		{
			CheckpointData currentCheckpoint = CheckpointDataManager.GetCurrentCheckpoint();
			DailyMissionSystem.CurrentMissions.Clear();
			for (int i = 0; i < DailyMissionSystem.missions.Count; i++)
			{
				DailyMissionSystem.missions[i].CurrentValue = 0;
				DailyMissionSystem.missions[i].State = 0;
				if (currentCheckpoint.ID >= DailyMissionSystem.missions[i].LevelRange[0] && currentCheckpoint.ID <= DailyMissionSystem.missions[i].LevelRange[1])
				{
					DailyMissionSystem.CurrentMissions.Add(DailyMissionSystem.missions[i]);
				}
			}
			DailyMissionSystem.save();
		}

		public static void SetDailyMission(DailyMissionType type, int value)
		{
			for (int i = 0; i < DailyMissionSystem.CurrentMissions.Count; i++)
			{
				if (DailyMissionSystem.CurrentMissions[i].Type == type && DailyMissionSystem.CurrentMissions[i].State == 0)
				{
					DailyMissionSystem.CurrentMissions[i].CurrentValue += value;
					if (DailyMissionSystem.CurrentMissions[i].CurrentValue >= DailyMissionSystem.CurrentMissions[i].TargetValue)
					{
						DailyMissionSystem.CurrentMissions[i].State = 1;
						if (DailyMissionSystem.IsAllComplete())
						{
							DailyMissionSystem.SetAllMissionComplete();
						}
					}
				}
			}
			DailyMissionSystem.sort();
			DailyMissionSystem.save();
		}

		private static bool IsAllComplete()
		{
			for (int i = 0; i < DailyMissionSystem.CurrentMissions.Count; i++)
			{
				if (DailyMissionSystem.CurrentMissions[i].Type != DailyMissionType.COMPLETE_ALL && DailyMissionSystem.CurrentMissions[i].State == 0)
				{
					return false;
				}
			}
			return true;
		}

		private static void SetAllMissionComplete()
		{
			for (int i = 0; i < DailyMissionSystem.CurrentMissions.Count; i++)
			{
				if (DailyMissionSystem.CurrentMissions[i].Type == DailyMissionType.COMPLETE_ALL)
				{
					DailyMissionSystem.CurrentMissions[i].CurrentValue = DailyMissionSystem.CurrentMissions[i].TargetValue;
					DailyMissionSystem.CurrentMissions[i].State = 1;
				}
			}
			DailyMissionSystem.save();
		}

		public static void GetAward(DailyMission _mission)
		{
			_mission.State = 2;
			DailyMissionSystem.sort();
			DailyMissionSystem.save();
		}

		private static void GetSameStateMissions(ref List<DailyMission> list, int state)
		{
			for (int i = 0; i < DailyMissionSystem.CurrentMissions.Count; i++)
			{
				if (DailyMissionSystem.CurrentMissions[i].State == state)
				{
					list.Add(DailyMissionSystem.CurrentMissions[i]);
				}
			}
		}

		public static bool HasMissionCompleted()
		{
			List<DailyMission> list = new List<DailyMission>();
			DailyMissionSystem.GetSameStateMissions(ref list, 1);
			return list.Count > 0;
		}

		private static void sort()
		{
			DailyMissionSystem.CurrentMissions.Sort(delegate(DailyMission a, DailyMission b)
			{
				if (a.Type == DailyMissionType.COMPLETE_ALL ^ b.Type == DailyMissionType.COMPLETE_ALL)
				{
					return (b.Type == DailyMissionType.COMPLETE_ALL).CompareTo(a.Type == DailyMissionType.COMPLETE_ALL);
				}
				if (a.Type == DailyMissionType.FACEBOOK_SHARE ^ b.Type == DailyMissionType.FACEBOOK_SHARE)
				{
					return (b.Type == DailyMissionType.FACEBOOK_SHARE).CompareTo(a.Type == DailyMissionType.FACEBOOK_SHARE);
				}
				if (a.Type == DailyMissionType.WATCH_VIDEO ^ b.Type == DailyMissionType.WATCH_VIDEO)
				{
					return (b.Type == DailyMissionType.WATCH_VIDEO).CompareTo(a.Type == DailyMissionType.WATCH_VIDEO);
				}
				if (a.State == 1 ^ b.State == 1)
				{
					return (b.State == 1).CompareTo(a.State == 1);
				}
				if (a.State == 0 ^ b.State == 0)
				{
					return (b.State == 0).CompareTo(a.State == 0);
				}
				if (a.State == 2 ^ b.State == 2)
				{
					return (b.State == 2).CompareTo(a.State == 2);
				}
				int type = (int)a.Type;
				return type.CompareTo((int)b.Type);
			});
		}

		private static void save()
		{
			DailyMissionSystem.SaveDatas.Clear();
			for (int i = 0; i < DailyMissionSystem.CurrentMissions.Count; i++)
			{
				DailyMissionSaveData dailyMissionSaveData = new DailyMissionSaveData();
				dailyMissionSaveData.ID = DailyMissionSystem.CurrentMissions[i].ID;
				dailyMissionSaveData.CurrentValue = DailyMissionSystem.CurrentMissions[i].CurrentValue;
				dailyMissionSaveData.State = DailyMissionSystem.CurrentMissions[i].State;
				DailyMissionSystem.SaveDatas.Add(dailyMissionSaveData);
			}
			DataManager.SaveToJson<DailyMissionSaveData>("SAVE_KEY_DAILYMISSION", DailyMissionSystem.SaveDatas);
		}

		private static void read()
		{
			DailyMissionSystem.SaveDatas = DataManager.ParseJson<DailyMissionSaveData>("SAVE_KEY_DAILYMISSION");
			DailyMissionSystem.CurrentMissions.Clear();
			for (int i = 0; i < DailyMissionSystem.missions.Count; i++)
			{
				for (int j = 0; j < DailyMissionSystem.SaveDatas.Count; j++)
				{
					if (DailyMissionSystem.missions[i].ID == DailyMissionSystem.SaveDatas[j].ID)
					{
						DailyMissionSystem.missions[i].CurrentValue = DailyMissionSystem.SaveDatas[j].CurrentValue;
						DailyMissionSystem.missions[i].State = DailyMissionSystem.SaveDatas[j].State;
						DailyMissionSystem.CurrentMissions.Add(DailyMissionSystem.missions[i]);
					}
				}
			}
		}

		private static List<DailyMission> missions = new List<DailyMission>();

		private static List<DailyMission> CurrentMissions = new List<DailyMission>();

		private static List<DailyMissionSaveData> SaveDatas = new List<DailyMissionSaveData>();
	}
}
