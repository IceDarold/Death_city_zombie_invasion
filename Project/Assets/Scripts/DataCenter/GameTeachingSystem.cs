using System;
using System.Collections.Generic;

namespace DataCenter
{
	public class GameTeachingSystem
	{
		public static TeachingData GetTeaching()
		{
			return null;
		}

		private static void save()
		{
			List<TeachingSaveData> list = new List<TeachingSaveData>();
			for (int i = 0; i < GameTeachingSystem.teachings.Count; i++)
			{
				list.Add(new TeachingSaveData
				{
					ID = GameTeachingSystem.teachings[i].ID,
					State = GameTeachingSystem.teachings[i].State
				});
			}
			DataManager.SaveToJson<TeachingSaveData>(string.Empty, list);
		}

		private static void read()
		{
			List<TeachingSaveData> list = DataManager.ParseJson<TeachingSaveData>(string.Empty);
			for (int i = 0; i < GameTeachingSystem.teachings.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (GameTeachingSystem.teachings[i].ID == list[j].ID)
					{
						GameTeachingSystem.teachings[i].State = list[j].State;
						break;
					}
				}
			}
		}

		private static List<TeachingData> teachings = new List<TeachingData>();
	}
}
