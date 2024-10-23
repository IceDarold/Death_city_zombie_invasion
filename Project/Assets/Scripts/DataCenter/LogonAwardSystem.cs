using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class LogonAwardSystem
	{
		static LogonAwardSystem()
		{
			LogonAwardSystem.awards = DataManager.ParseXmlData<LogonAward>("LogonAward", "LogonAwards", "LogonAward");
			if (PlayerPrefs.GetString("InitLogonAwardSystem", "true") == "true")
			{
				PlayerPrefs.SetString("InitLogonAwardSystem", "false");
				for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
				{
					LogonAwardSystem.awards[i].State = 0;
				}
				LogonAwardSystem.save();
			}
			else
			{
				LogonAwardSystem.read();
			}
		}

		public static List<LogonAward> LogonAwards
		{
			get
			{
				return LogonAwardSystem.awards;
			}
		}

		public static LogonAward GetLogonAward(int id)
		{
			for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
			{
				if (LogonAwardSystem.awards[i].ID == id)
				{
					return LogonAwardSystem.awards[i];
				}
			}
			return null;
		}

		public static LogonAward GetCurrentLogonAward()
		{
			if (LogonAwardSystem.LogonDays != 0)
			{
				return LogonAwardSystem.GetLogonAward(LogonAwardSystem.LogonDays);
			}
			return null;
		}

		public static int GetLogonDays()
		{
			LogonAwardSystem.LogonDays = Mathf.Clamp(LogonAwardSystem.LogonDays, 0, 30);
			return LogonAwardSystem.LogonDays;
		}

		public static void SetLogon()
		{
			LogonAwardSystem.LogonDays++;
			for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
			{
				if (LogonAwardSystem.awards[i].State == 0)
				{
					LogonAwardSystem.awards[i].State = 1;
					break;
				}
			}
			LogonAwardSystem.save();
		}

		public static bool CanReceive()
		{
			for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
			{
				if (LogonAwardSystem.awards[i].State == 1)
				{
					return true;
				}
			}
			return false;
		}

		public static bool CanReceiveAgain()
		{
			LogonAward currentLogonAward = LogonAwardSystem.GetCurrentLogonAward();
			return currentLogonAward != null && currentLogonAward.State == 2 && currentLogonAward.ReceiveTimes == 1;
		}

		public static void SetAwardReceived(LogonAward data)
		{
			data.State = 2;
			data.ReceiveTimes++;
			LogonAwardSystem.save();
		}

		public static LogonAward GetRecommendAward()
		{
			for (int i = 0; i < LogonAwardSystem.RecommendDays.Length; i++)
			{
				if (LogonAwardSystem.LogonDays < LogonAwardSystem.RecommendDays[i])
				{
					return LogonAwardSystem.GetLogonAward(LogonAwardSystem.RecommendDays[i]);
				}
			}
			return null;
		}

		private static void save()
		{
			List<LogonSaveData> list = new List<LogonSaveData>();
			for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
			{
				list.Add(new LogonSaveData
				{
					ID = LogonAwardSystem.awards[i].ID,
					State = LogonAwardSystem.awards[i].State
				});
			}
			DataManager.SaveToJson<LogonSaveData>("LogonAwardSystemSaveKey", list);
			PlayerPrefs.SetInt("LogonDaysSaveKey", LogonAwardSystem.LogonDays);
		}

		private static void read()
		{
			LogonAwardSystem.LogonDays = PlayerPrefs.GetInt("LogonDaysSaveKey", 0);
			List<LogonSaveData> list = DataManager.ParseJson<LogonSaveData>("LogonAwardSystemSaveKey");
			for (int i = 0; i < LogonAwardSystem.awards.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (LogonAwardSystem.awards[i].ID == list[j].ID)
					{
						LogonAwardSystem.awards[i].State = list[j].State;
						break;
					}
				}
			}
		}

		private static List<LogonAward> awards = new List<LogonAward>();

		private static List<LogonSaveData> saveDatas = new List<LogonSaveData>();

		private static int LogonDays = 0;

		public static int[] RecommendDays = new int[]
		{
			2,
			3,
			10,
			20,
			30
		};
	}
}
