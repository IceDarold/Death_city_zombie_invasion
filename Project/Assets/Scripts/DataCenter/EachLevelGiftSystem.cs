using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class EachLevelGiftSystem
	{
		static EachLevelGiftSystem()
		{
			EachLevelGiftSystem.init();
		}

		public static List<EachLevelGiftData> GetEachLevelGiftDatas()
		{
			return EachLevelGiftSystem.gifts;
		}

		private static void init()
		{
			EachLevelGiftSystem.gifts = DataManager.ParseXmlData<EachLevelGiftData>("EachLevelGift", "EachLevelGifts", "EachLevelGift");
			if (PlayerPrefs.GetString("InitEachLevelGiftSystem", "true").Equals("true"))
			{
				PlayerPrefs.SetString("InitEachLevelGiftSystem", "false");
				EachLevelGiftSystem.save();
			}
			else
			{
				EachLevelGiftSystem.read();
			}
		}

		public static void SetReceiveState()
		{
			for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
			{
				if (EachLevelGiftSystem.gifts[i].State == 0 && EachLevelGiftSystem.gifts[i].RequireLevel < CheckpointDataManager.GetCurrentCheckpoint().ID)
				{
					EachLevelGiftSystem.gifts[i].State = 1;
				}
			}
			EachLevelGiftSystem.save();
		}

		public static void SetState(EachLevelGiftData data, int state)
		{
			data.State = state;
			EachLevelGiftSystem.save();
		}

		public static bool CanReceive()
		{
			if (ItemDataManager.GetCurrency(CommonDataType.EACH_LEVEL_AWARD_GIFT) == 1)
			{
				for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
				{
					if (EachLevelGiftSystem.gifts[i].State == 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool isAllReceived()
		{
			for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
			{
				if (EachLevelGiftSystem.gifts[i].State != 2)
				{
					return false;
				}
			}
			return true;
		}

		public static int GetTotalGifts()
		{
			return EachLevelGiftSystem.gifts.Count;
		}

		public static int GetReachGifts()
		{
			int num = 0;
			for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
			{
				if (EachLevelGiftSystem.gifts[i].State != 0)
				{
					num++;
				}
			}
			return num;
		}

		private static void save()
		{
			EachLevelGiftSystem.SaveDatas.Clear();
			for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
			{
				EachLevelGiftSaveData eachLevelGiftSaveData = new EachLevelGiftSaveData();
				eachLevelGiftSaveData.ID = EachLevelGiftSystem.gifts[i].ID;
				eachLevelGiftSaveData.State = EachLevelGiftSystem.gifts[i].State;
				EachLevelGiftSystem.SaveDatas.Add(eachLevelGiftSaveData);
			}
			DataManager.SaveToJson<EachLevelGiftSaveData>("EACHLEVELGIFT_SAVEKEY", EachLevelGiftSystem.SaveDatas);
		}

		private static void read()
		{
			EachLevelGiftSystem.SaveDatas = DataManager.ParseJson<EachLevelGiftSaveData>("EACHLEVELGIFT_SAVEKEY");
			for (int i = 0; i < EachLevelGiftSystem.gifts.Count; i++)
			{
				for (int j = 0; j < EachLevelGiftSystem.SaveDatas.Count; j++)
				{
					if (EachLevelGiftSystem.gifts[i].ID == EachLevelGiftSystem.SaveDatas[j].ID)
					{
						EachLevelGiftSystem.gifts[i].State = EachLevelGiftSystem.SaveDatas[j].State;
					}
				}
			}
		}

		private static List<EachLevelGiftData> gifts = new List<EachLevelGiftData>();

		private static List<EachLevelGiftSaveData> SaveDatas = new List<EachLevelGiftSaveData>();
	}
}
