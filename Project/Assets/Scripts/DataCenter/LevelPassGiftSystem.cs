using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class LevelPassGiftSystem : MonoBehaviour
	{
		static LevelPassGiftSystem()
		{
			LevelPassGiftSystem.gifts =
				DataManager.ParseXmlData<LevelPassGiftData>("LevelPassGiftData", "LevelPassGiftDatas",
					"LevelPassGiftData");
			if (PlayerPrefs.GetString("InitLevelPassGiftSystem", "true") == "true")
			{
				PlayerPrefs.SetString("InitLevelPassGiftSystem", "false");
				LevelPassGiftSystem.save();
			}
			else
			{
				LevelPassGiftSystem.read();
			}
		}

		public static LevelPassGiftData GetCurrentData()
		{
			for (int i = 0; i < LevelPassGiftSystem.gifts.Count; i++)
			{
				if (!LevelPassGiftSystem.gifts[i].Received)
				{
					return LevelPassGiftSystem.gifts[i];
				}
			}
			return null;
		}

		public static void SetReceived(int id)
		{
			for (int i = 0; i < LevelPassGiftSystem.gifts.Count; i++)
			{
				if (LevelPassGiftSystem.gifts[i].ID == id)
				{
					LevelPassGiftSystem.gifts[i].Received = true;
				}
			}
			LevelPassGiftSystem.save();
		}

		public static void save()
		{
			LevelPassGiftSystem.SaveDatas.Clear();
			for (int i = 0; i < LevelPassGiftSystem.gifts.Count; i++)
			{
				LevelPassGiftSaveData levelPassGiftSaveData = new LevelPassGiftSaveData();
				levelPassGiftSaveData.ID = LevelPassGiftSystem.gifts[i].ID;
				levelPassGiftSaveData.Received = LevelPassGiftSystem.gifts[i].Received;
				LevelPassGiftSystem.SaveDatas.Add(levelPassGiftSaveData);
			}
			DataManager.SaveToJson<LevelPassGiftSaveData>("LevelPassGiftDatas", LevelPassGiftSystem.SaveDatas);
		}

		public static void read()
		{
			LevelPassGiftSystem.SaveDatas = DataManager.ParseJson<LevelPassGiftSaveData>("LevelPassGiftDatas");
			for (int i = 0; i < LevelPassGiftSystem.gifts.Count; i++)
			{
				for (int j = 0; j < LevelPassGiftSystem.SaveDatas.Count; j++)
				{
					if (LevelPassGiftSystem.gifts[i].ID == LevelPassGiftSystem.SaveDatas[j].ID)
					{
						LevelPassGiftSystem.gifts[i].Received = LevelPassGiftSystem.SaveDatas[j].Received;
					}
				}
			}
		}

		private static List<LevelPassGiftData> gifts = new List<LevelPassGiftData>();

		private static List<LevelPassGiftSaveData> SaveDatas = new List<LevelPassGiftSaveData>();
	}
}
