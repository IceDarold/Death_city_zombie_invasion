using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class EquipmentDataManager
	{
		static EquipmentDataManager()
		{
			EquipmentDataManager.init();
		}

		public static List<EquipmentData> Equipments
		{
			get
			{
				return EquipmentDataManager.equipments;
			}
		}

		public static void init()
		{
			EquipmentDataManager.equipments = DataManager.ParseXmlData<EquipmentData>("EquipmentData", "EquipmentDatas", "EquipmentData");
			EquipmentDataManager.sets = DataManager.ParseXmlData<EquipmentSetData>("EquipmentSetData", "EquipmentSetDatas", "EquipmentSetData");
			if (PlayerPrefs.GetString("InitEquipmentData", "true") == "true")
			{
				PlayerPrefs.SetString("InitEquipmentData", "false");
				EquipmentDataManager.Sort();
				EquipmentDataManager.save();
			}
			else
			{
				EquipmentDataManager.read();
			}
		}

		public static EquipmentData GetEquipmentData(int id)
		{
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				if (EquipmentDataManager.equipments[i].ID == id)
				{
					return EquipmentDataManager.equipments[i];
				}
			}
			return null;
		}

		public static EquipmentSetData GetSetData(int id)
		{
			for (int i = 0; i < EquipmentDataManager.sets.Count; i++)
			{
				if (EquipmentDataManager.sets[i].ID == id)
				{
					return EquipmentDataManager.sets[i];
				}
			}
			return null;
		}

		public static void Unlock(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null && equipmentData.State < EquipmentState.待制作)
			{
				equipmentData.State = EquipmentState.待制作;
				StoreDataManager.SetGiftPurchased(id);
				EquipmentDataManager.save();
			}
		}

		public static void StartFabricate(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				equipmentData.State = EquipmentState.制作中;
				EquipmentDataManager.save();
			}
		}

		public static void FinishFabricate(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				equipmentData.State = EquipmentState.待领取;
				EquipmentDataManager.save();
			}
		}

		public static void Collect(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null && equipmentData.State == EquipmentState.未解锁)
			{
				AchievementDataManager.SetAchievementValue(AchievementType.GAIN_EQUIPMENT, 1);
				if (equipmentData.State < EquipmentState.待升级)
				{
					equipmentData.State = EquipmentState.待升级;
				}
				if (equipmentData.Level <= 1)
				{
					equipmentData.Level = 1;
				}
				equipmentData.isNew = true;
				EquipmentDataManager.save();
			}
		}

		public static void StartUpgrade(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				equipmentData.State = EquipmentState.升级中;
				EquipmentDataManager.save();
			}
		}

		public static void FinishUpgrade(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				equipmentData.Level++;
				AchievementDataManager.SetAchievementValue(AchievementType.EQUIPMENT_UPGRADE, 1);
				if (equipmentData.Level < 5)
				{
					equipmentData.State = EquipmentState.待升级;
				}
				else
				{
					equipmentData.State = EquipmentState.已满级;
				}
				EquipmentDataManager.save();
			}
		}

		public static void RemoveNewTag(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				equipmentData.isNew = false;
				EquipmentDataManager.save();
			}
		}

		public static bool isEquip(int id)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				if (equipmentData.Part == 1 && equipmentData.ID == PlayerDataManager.Player.Cap)
				{
					return true;
				}
				if (equipmentData.Part == 2 && equipmentData.ID == PlayerDataManager.Player.Coat)
				{
					return true;
				}
				if (equipmentData.Part == 3 && equipmentData.ID == PlayerDataManager.Player.Shoes)
				{
					return true;
				}
			}
			return false;
		}

		public static List<EquipmentData> GetSets(int id)
		{
			List<EquipmentData> list = new List<EquipmentData>();
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				if (EquipmentDataManager.equipments[i].SetID == id)
				{
					list.Add(EquipmentDataManager.equipments[i]);
				}
			}
			list.Sort((EquipmentData a, EquipmentData b) => a.Part.CompareTo(b.Part));
			return list;
		}

		public static int isSetsActivated(int id)
		{
			List<EquipmentData> list = EquipmentDataManager.GetSets(id);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (EquipmentDataManager.isEquip(list[i].ID))
				{
					num++;
				}
			}
			return num;
		}

		public static int GetFighting(int id, bool isMax)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			int num;
			if (equipmentData.State == EquipmentState.未解锁)
			{
				num = 5;
			}
			else
			{
				num = ((!isMax) ? equipmentData.Level : 5);
			}
			int result = 0;
			if (equipmentData.AttributeType == EquipmentAttribute.HEALTH)
			{
				result = equipmentData.AttributeValue[num] * 10;
			}
			return result;
		}

		public static void GetSameStateEquipment(ref List<EquipmentData> list, EquipmentState state)
		{
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				if (EquipmentDataManager.equipments[i].State == state)
				{
					list.Add(EquipmentDataManager.equipments[i]);
				}
			}
		}

		public static void GetActiveEquipment(ref List<EquipmentData> list)
		{
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				if (EquipmentDataManager.equipments[i].State == EquipmentState.制作中 || EquipmentDataManager.equipments[i].State == EquipmentState.升级中)
				{
					list.Add(EquipmentDataManager.equipments[i]);
				}
			}
		}

		public static int GetProductionLine()
		{
			EquipmentDataManager.ProductionLine = PlayerPrefs.GetInt("EquipmentProductionLine", 0);
			return EquipmentDataManager.ProductionLine;
		}

		public static void UseProductionLine()
		{
			EquipmentDataManager.ProductionLine++;
			PlayerPrefs.SetInt("EquipmentProductionLine", EquipmentDataManager.ProductionLine);
		}

		public static void FinishProductionLine()
		{
			EquipmentDataManager.ProductionLine--;
			PlayerPrefs.SetInt("EquipmentProductionLine", EquipmentDataManager.ProductionLine);
		}

		private static void Sort()
		{
			EquipmentDataManager.equipments.Sort(delegate(EquipmentData a, EquipmentData b)
			{
				if (a.Quality != b.Quality)
				{
					return a.Quality.CompareTo(b.Quality);
				}
				if (a.SetID != b.SetID)
				{
					return a.SetID.CompareTo(b.SetID);
				}
				if (a.Part != b.Part)
				{
					return a.Part.CompareTo(b.Part);
				}
				return a.ID.CompareTo(b.ID);
			});
		}

		private static void save()
		{
			EquipmentDataManager.saveDatas.Clear();
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				EquipmentSaveData equipmentSaveData = new EquipmentSaveData();
				equipmentSaveData.ID = EquipmentDataManager.equipments[i].ID;
				equipmentSaveData.isNew = EquipmentDataManager.equipments[i].isNew;
				equipmentSaveData.State = EquipmentDataManager.equipments[i].State;
				equipmentSaveData.Level = EquipmentDataManager.equipments[i].Level;
				EquipmentDataManager.saveDatas.Add(equipmentSaveData);
			}
			DataManager.SaveToJson<EquipmentSaveData>("EquipmentDatas", EquipmentDataManager.saveDatas);
		}

		private static void read()
		{
			EquipmentDataManager.saveDatas = DataManager.ParseJson<EquipmentSaveData>("EquipmentDatas");
			int num = 0;
			for (int i = 0; i < EquipmentDataManager.equipments.Count; i++)
			{
				if (i - num < EquipmentDataManager.saveDatas.Count && EquipmentDataManager.equipments[i].ID == EquipmentDataManager.saveDatas[i - num].ID)
				{
					EquipmentDataManager.equipments[i].isNew = EquipmentDataManager.saveDatas[i - num].isNew;
					EquipmentDataManager.equipments[i].State = EquipmentDataManager.saveDatas[i - num].State;
					EquipmentDataManager.equipments[i].Level = EquipmentDataManager.saveDatas[i - num].Level;
				}
				else
				{
					EquipmentDataManager.equipments[i].isNew = false;
					EquipmentDataManager.equipments[i].State = EquipmentState.未解锁;
					EquipmentDataManager.equipments[i].Level = 0;
					num++;
				}
			}
		}

		public const int MaxLevel = 5;

		public const int MaxProductionLine = 2;

		private static List<EquipmentData> equipments = new List<EquipmentData>();

		private static List<EquipmentSetData> sets = new List<EquipmentSetData>();

		private static List<EquipmentSaveData> saveDatas = new List<EquipmentSaveData>();

		private static int ProductionLine = 0;
	}
}
