using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class ItemDataManager
	{
		static ItemDataManager()
		{
			ItemDataManager.init();
		}

		private static void init()
		{
			ItemDataManager.CommonDatas = DataManager.ParseXmlData<ItemData>("CommonItemData", "CommonItemDatas", "CommonItemData");
			ItemDataManager.BossExtraAwardList = DataManager.ParseXmlData<BossExtraAward>("BossExtraAward", "BossExtraAwards", "BossExtraAward");
			if (PlayerPrefs.GetString("InitItemData", "true") == "true")
			{
				PlayerPrefs.SetString("InitItemData", "false");
				ItemDataManager.SetCurrency(CommonDataType.REVIVE_COIN, 3);
				ItemDataManager.save();
			}
			else
			{
				ItemDataManager.read();
			}
		}

		public static ItemData GetCommonItem(CommonDataType type)
		{
			for (int i = 0; i < ItemDataManager.CommonDatas.Count; i++)
			{
				if (ItemDataManager.CommonDatas[i].ID == (int)type)
				{
					return ItemDataManager.CommonDatas[i];
				}
			}
			return null;
		}

		public static int GetCurrency(CommonDataType type)
		{
			ItemData commonItem = ItemDataManager.GetCommonItem(type);
			if (commonItem != null)
			{
				return commonItem.Count;
			}
			return 0;
		}

		public static void SetCurrency(CommonDataType type, int num)
		{
			ItemData commonItem = ItemDataManager.GetCommonItem(type);
			if (commonItem != null)
			{
				switch (type)
				{
				case CommonDataType.GOLD:
					commonItem.Count += num;
					if (num < 0)
					{
						AchievementDataManager.SetAchievementValue(AchievementType.USE_GOLD, Mathf.Abs(num));
					}
					else
					{
						PlayerDataManager.SetStatisticsDatas(PlayerStatistics.EarnGoldCoins, num);
					}
					break;
				case CommonDataType.DIAMOND:
					commonItem.Count += num;
					if (num < 0)
					{
						AchievementDataManager.SetAchievementValue(AchievementType.USE_DIAMOND, Mathf.Abs(num));
					}
					else
					{
						PlayerDataManager.SetStatisticsDatas(PlayerStatistics.EarnDiamonds, num);
					}
					break;
				case CommonDataType.DNA:
					commonItem.Count += num;
					if (num > 0)
					{
						AchievementDataManager.SetAchievementValue(AchievementType.GAIN_DNA, num);
					}
					break;
				default:
					switch (type)
					{
					case CommonDataType.VIP:
					case CommonDataType.MONTHLY_CARD:
					case CommonDataType.EACH_LEVEL_AWARD_GIFT:
						break;
					default:
						switch (type)
						{
						case CommonDataType.ADDITIONAL_GOLD:
						case CommonDataType.ADDITIONAL_DIAMOND:
						case CommonDataType.ADDITIONAL_DNA:
							break;
						default:
							if (type != CommonDataType.DOUBLE)
							{
								commonItem.Count += num;
								goto IL_104;
							}
							break;
						}
						break;
					}
					commonItem.Count = num;
					break;
				}
				IL_104:
				ItemDataManager.save();
			}
		}

		public static ItemData GetItemData(int id)
		{
			ItemData result = new ItemData();
			if (id < 1000)
			{
				result = ItemDataManager.GetCommonItem((CommonDataType)id);
			}
			else if (id >= 1000 && id < 2000)
			{
				result = RoleDataManager.GetRoleData(id);
			}
			else if (id >= 2000 && id < 3000)
			{
				result = WeaponDataManager.GetWeaponData(id);
			}
			else if (id >= 3000 && id < 4000)
			{
				result = EquipmentDataManager.GetEquipmentData(id);
			}
			else if (id >= 4000 && id < 5000)
			{
				result = PropDataManager.GetPropData(id);
			}
			else if (id >= 8000 && id < 9000)
			{
				result = DebrisDataManager.GetDebrisData(id);
			}
			else if (id >= 9000 && id < 10000)
			{
				result = StoreDataManager.GetStoreData(id);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static void CollectItem(int id, int num = 0)
		{
			ItemData itemData = ItemDataManager.GetItemData(id);
			if (id < 1000)
			{
				ItemDataManager.SetCurrency((CommonDataType)id, num);
			}
			else if (id >= 1000 && id < 2000)
			{
				RoleDataManager.Unlcok(id);
			}
			else if (id >= 2000 && id < 3000)
			{
				WeaponDataManager.CollectWeapon(id);
				StoreDataManager.SetGiftPurchased(id);
			}
			else if (id >= 3000 && id < 4000)
			{
				EquipmentDataManager.Collect(id);
				StoreDataManager.SetGiftPurchased(id);
			}
			else if (id >= 4000 && id < 5000)
			{
				PropDataManager.CollectProp(id, num);
			}
			else if (id >= 8000 && id < 9000)
			{
				DebrisDataManager.CollcetDebris(id, num);
			}
			else
			{
				if (id < 9000 || id >= 10000)
				{
					return;
				}
				StoreDataManager.Buy(id);
			}
		}

		public static int GetTotalDebris()
		{
			int num = 0;
			for (int i = 0; i < WeaponDataManager.Weapons.Count; i++)
			{
				num += WeaponDataManager.Weapons[i].RequiredDebris;
			}
			for (int j = 0; j < PropDataManager.Props.Count; j++)
			{
				num += PropDataManager.Props[j].RequiredDebris;
			}
			return num;
		}

		public static int GetCurrentDebris()
		{
			int num = 0;
			for (int i = 0; i < WeaponDataManager.Weapons.Count; i++)
			{
				if (WeaponDataManager.Weapons[i].State == WeaponState.未解锁)
				{
					num += DebrisDataManager.GetDebrisData(WeaponDataManager.Weapons[i].DebrisID).Count;
				}
				else
				{
					num += WeaponDataManager.Weapons[i].RequiredDebris;
				}
			}
			for (int j = 0; j < PropDataManager.Props.Count; j++)
			{
				if (PropDataManager.Props[j].State == PropState.未解锁)
				{
					num += DebrisDataManager.GetDebrisData(PropDataManager.Props[j].DebrisID).Count;
				}
				else
				{
					num += PropDataManager.Props[j].RequiredDebris;
				}
			}
			return num;
		}

		public static int CollectDropDebris()
		{
			List<int> list = new List<int>();
			int num = 0;
			for (int i = 0; i < ItemDataManager.DebrisDropWeightList.Count; i++)
			{
				if (ItemDataManager.DebrisDropWeightList[i].ID >= 2000 && ItemDataManager.DebrisDropWeightList[i].ID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(ItemDataManager.DebrisDropWeightList[i].ID);
					if (weaponData.State == WeaponState.未解锁)
					{
						list.Add(weaponData.ID);
					}
				}
				else if (ItemDataManager.DebrisDropWeightList[i].ID >= 4000 && ItemDataManager.DebrisDropWeightList[i].ID < 5000)
				{
					PropData propData = PropDataManager.GetPropData(ItemDataManager.DebrisDropWeightList[i].ID);
					if (propData.State == PropState.未解锁)
					{
						list.Add(propData.ID);
					}
				}
			}
			if (list.Count > 0)
			{
				num = list[UnityEngine.Random.Range(0, list.Count)];
			}
			DebrisDataManager.CollcetDebris(num, 1);
			return num;
		}

		public static DebrisData CollectDropDebrisInBossBarrier()
		{
			List<WeaponData> list = new List<WeaponData>();
			for (int i = 0; i < WeaponDataManager.Weapons.Count; i++)
			{
				if (WeaponDataManager.Weapons[i].State == WeaponState.未解锁 && WeaponDataManager.Weapons[i].Quality >= 3 && WeaponDataManager.Weapons[i].Quality <= 4)
				{
					list.Add(WeaponDataManager.Weapons[i]);
				}
			}
			int id = UnityEngine.Random.Range(0, list.Count);
			DebrisData debrisData = DebrisDataManager.GetDebrisData(id);
			DebrisDataManager.CollcetDebris(debrisData.ID, 1);
			return debrisData;
		}

		public static List<BossExtraAward> GetBossExtraAwardList(int id)
		{
			List<BossExtraAward> list = new List<BossExtraAward>();
			for (int i = 0; i < ItemDataManager.BossExtraAwardList.Count; i++)
			{
				if (ItemDataManager.BossExtraAwardList[i].Index == id)
				{
					list.Add(ItemDataManager.BossExtraAwardList[i]);
				}
			}
			return list;
		}

		public static BossExtraAward GetBossExtraAward(int id)
		{
			List<BossExtraAward> list = new List<BossExtraAward>();
			int num = 0;
			for (int i = 0; i < ItemDataManager.BossExtraAwardList.Count; i++)
			{
				if (ItemDataManager.BossExtraAwardList[i].Index == id)
				{
					if (ItemDataManager.BossExtraAwardList[i].AwardID >= 8000 && ItemDataManager.BossExtraAwardList[i].AwardID < 9000)
					{
						DebrisData debrisData = DebrisDataManager.GetDebrisData(ItemDataManager.BossExtraAwardList[i].AwardID);
						if (debrisData != null)
						{
							if (debrisData.ItemID >= 2000 && debrisData.ItemID < 3000)
							{
								WeaponData weaponData = WeaponDataManager.GetWeaponData(debrisData.ItemID);
								if (weaponData.State == WeaponState.未解锁)
								{
									list.Add(ItemDataManager.BossExtraAwardList[i]);
									num += ItemDataManager.BossExtraAwardList[i].AwardWeight;
								}
							}
							else if (debrisData.ItemID >= 4000 && debrisData.ItemID < 5000)
							{
								PropData propData = PropDataManager.GetPropData(debrisData.ItemID);
								if (propData.State == PropState.未解锁)
								{
									list.Add(ItemDataManager.BossExtraAwardList[i]);
									num += ItemDataManager.BossExtraAwardList[i].AwardWeight;
								}
							}
						}
					}
					else
					{
						list.Add(ItemDataManager.BossExtraAwardList[i]);
						num += ItemDataManager.BossExtraAwardList[i].AwardWeight;
					}
				}
			}
			int num2 = 0;
			int num3 = UnityEngine.Random.Range(0, num);
			for (int j = 0; j < list.Count; j++)
			{
				num2 += list[j].AwardWeight;
				if (num3 < num2)
				{
					return list[j];
				}
			}
			return null;
		}

		private static void save()
		{
			ItemDataManager.SaveDatas.Clear();
			for (int i = 0; i < ItemDataManager.CommonDatas.Count; i++)
			{
				ItemSaveData itemSaveData = new ItemSaveData();
				itemSaveData.ID = ItemDataManager.CommonDatas[i].ID;
				itemSaveData.Count = ItemDataManager.CommonDatas[i].Count;
				ItemDataManager.SaveDatas.Add(itemSaveData);
			}
			DataManager.SaveToJson<ItemSaveData>("CommonItemDatas", ItemDataManager.SaveDatas);
		}

		private static void read()
		{
			ItemDataManager.SaveDatas = DataManager.ParseJson<ItemSaveData>("CommonItemDatas");
			for (int i = 0; i < ItemDataManager.CommonDatas.Count; i++)
			{
				for (int j = 0; j < ItemDataManager.SaveDatas.Count; j++)
				{
					if (ItemDataManager.CommonDatas[i].ID == ItemDataManager.SaveDatas[j].ID)
					{
						ItemDataManager.CommonDatas[i].Count = ItemDataManager.SaveDatas[j].Count;
						break;
					}
				}
			}
		}

		private static List<ItemData> CommonDatas = new List<ItemData>();

		private static List<ItemSaveData> SaveDatas = new List<ItemSaveData>();

		private static List<DebrisDropWeight> DebrisDropWeightList = new List<DebrisDropWeight>();

		private static List<BossExtraAward> BossExtraAwardList = new List<BossExtraAward>();

		private const int EncryptionValue = 19900825;
	}
}
