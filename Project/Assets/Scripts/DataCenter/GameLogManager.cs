using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class GameLogManager
	{
		static GameLogManager()
		{
			if (PlayerPrefs.GetString("InitGameLog", "true") == "true")
			{
				PlayerPrefs.SetString("InitGameLog", "false");
				GameLogManager.save();
			}
			else
			{
				GameLogManager.read();
			}
		}

		public static void SendPageLog(string current, string next = "null")
		{
			string name = current + "_" + next;
			LogData logData = GameLogManager.GetLog(name);
			if (logData == null)
			{
				logData = new LogData();
				logData.Name = name;
				GameLogManager.logs.Add(logData);
			}
			logData.Count++;
			GameLogManager.save();
			//GMGSDK.SendPageLog(false, current, next, logData.Count.ToString());
		}

		public static void SendCostLog(int type, int count, int itemID, int tag)
		{
			//GMGSDK.SendConsumeLog(true, count, type, itemID, tag);
		}

		public static void SendGameOverLog(bool iswin, int revive)
		{
			//GMGSDK.SendGameOverLog(iswin, revive, 0f, 0f, 0, 0);
		}

		public static void StartGameLog()
		{
			//GMGSDK.SendStartGameLog();
		}

		private static LogData GetLog(string name)
		{
			for (int i = 0; i < GameLogManager.logs.Count; i++)
			{
				if (GameLogManager.logs[i].Name.Equals(name))
				{
					return GameLogManager.logs[i];
				}
			}
			return null;
		}

		public static void SendCheckpointLog(bool _success, int id)
		{
			if (_success)
			{
				//GMGSDK.SendStreamEvent("Game_Level_Success", id.ToString(), false);
			}
			else
			{
				//GMGSDK.SendStreamEvent("Game_Level_Fail", id.ToString(), false);
			}
		}

		public static void SendSnipeLog(int type)
		{
			//GMGSDK.SendStreamEvent("Game_Sniper", type.ToString(), false);
		}

		public static void SendLotteryLog(int type)
		{
			//GMGSDK.SendStreamEvent("Luck_Spin", type.ToString(), false);
		}

		private static void save()
		{
			DataManager.SaveToJson<LogData>("GameLogDatas", GameLogManager.logs);
		}

		private static void read()
		{
			GameLogManager.logs = DataManager.ParseJson<LogData>("GameLogDatas");
		}

		private static List<LogData> logs = new List<LogData>();
	}
}
