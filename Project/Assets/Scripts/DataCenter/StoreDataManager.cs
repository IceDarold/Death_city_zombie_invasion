using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class StoreDataManager
	{
		static StoreDataManager()
		{
			StoreDataManager.init();
		}

		public static void init()
		{
			StoreDataManager.StoreDatas = DataManager.ParseXmlData<StoreData>("StoreData", "StoreDatas", "StoreData");
			StoreDataManager.charges = DataManager.ParseXmlData<ChargePoint>("ChargePoint", "ChargePoints", "ChargePoint");
			if (PlayerPrefs.GetString("InitStoreData", "true") == "true")
			{
				PlayerPrefs.SetString("InitStoreData", "false");
				StoreDataManager.save();
			}
			else
			{
				StoreDataManager.read();
			}
		}

		public static StoreData GetStoreData(int id)
		{
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].ID == id)
				{
					return StoreDataManager.StoreDatas[i];
				}
			}
			return null;
		}

		public static StoreData GetStoreData(ChargePoint charge)
		{
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].ChargePoint == charge.ID)
				{
					return StoreDataManager.StoreDatas[i];
				}
			}
			return null;
		}

		public static List<StoreData> GetStoreList(int type)
		{
			List<StoreData> list = new List<StoreData>();
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].Type == type)
				{
					list.Add(StoreDataManager.StoreDatas[i]);
				}
			}
			list.Sort(delegate(StoreData a, StoreData b)
			{
				if (a.Priority != b.Priority)
				{
					return a.Priority.CompareTo(b.Priority);
				}
				return a.ID.CompareTo(b.ID);
			});
			return list;
		}

		public static void Buy(int id)
		{
			StoreData storeData = StoreDataManager.GetStoreData(id);
			for (int i = 0; i < storeData.ItemID.Length; i++)
			{
				if (storeData.ItemID[i] < 9000 || storeData.ItemID[i] < 10000)
				{
				}
				ItemDataManager.CollectItem(storeData.ItemID[i], storeData.ItemCount[i]);
			}
			if (storeData.Type == 4)
			{
				if (storeData.Tag == 1)
				{
					storeData.Purchased = true;
				}
			}
			else if (storeData.Type == 5)
			{
				if (storeData.Tag == 1)
				{
					storeData.Count++;
				}
				else
				{
					storeData.Purchased = true;
				}
			}
			StoreDataManager.save();
		}

		public static void SetGiftPurchased(int id)
		{
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].Type == 4 || StoreDataManager.StoreDatas[i].Type == 5)
				{
					int num = Array.IndexOf<int>(StoreDataManager.StoreDatas[i].ItemID, id);
					if (num != -1 && StoreDataManager.isContentBought(StoreDataManager.StoreDatas[i]))
					{
						StoreDataManager.StoreDatas[i].Purchased = true;
					}
				}
			}
			StoreDataManager.save();
		}

		private static bool isContentBought(StoreData data)
		{
			for (int i = 0; i < data.ItemID.Length; i++)
			{
				if (data.ItemID[i] >= 1000 && data.ItemID[i] < 2000)
				{
					RoleData roleData = RoleDataManager.GetRoleData(data.ItemID[i]);
					if (!roleData.Enable)
					{
						return false;
					}
				}
				if (data.ItemID[i] >= 2000 && data.ItemID[i] < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(data.ItemID[i]);
					if (weaponData.State == WeaponState.未解锁)
					{
						return false;
					}
				}
				if (data.ItemID[i] >= 3000 && data.ItemID[i] < 4000)
				{
					EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(data.ItemID[i]);
					if (equipmentData.State == EquipmentState.未解锁)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static void SetDisplayItemList(ref List<ItemData> list, int id)
		{
			list.Clear();
			StoreData storeData = StoreDataManager.GetStoreData(id);
			for (int i = 0; i < storeData.ItemID.Length; i++)
			{
				ItemData itemData = new ItemData();
				itemData.Icon = ItemDataManager.GetItemData(storeData.ItemID[i]).Icon;
				itemData.Count = storeData.ItemCount[i];
				itemData.ID = storeData.ItemID[i];
				list.Add(itemData);
			}
		}

		public static List<StoreData> GetCountLimitGifts()
		{
			List<StoreData> list = new List<StoreData>();
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].Type == 5 && StoreDataManager.StoreDatas[i].Tag == 1)
				{
					list.Add(StoreDataManager.StoreDatas[i]);
				}
			}
			return list;
		}

		public static bool IsAllLimitGiftsBought()
		{
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].Type == 5 && StoreDataManager.StoreDatas[i].Tag == 1 && StoreDataManager.StoreDatas[i].Count == 0)
				{
					return false;
				}
			}
			return true;
		}

		public static void ResetCountLimitGifts()
		{
			List<StoreData> countLimitGifts = StoreDataManager.GetCountLimitGifts();
			for (int i = 0; i < countLimitGifts.Count; i++)
			{
				countLimitGifts[i].Count = 0;
			}
			StoreDataManager.save();
		}

		public static void ResetFunctionGift(int id)
		{
			StoreData storeData = StoreDataManager.GetStoreData(id);
			if (storeData != null)
			{
				storeData.Purchased = false;
			}
			StoreDataManager.save();
		}

		public static ChargePoint GetChargePoint(int id)
		{
			for (int i = 0; i < StoreDataManager.charges.Count; i++)
			{
				if (StoreDataManager.charges[i].ID == id)
				{
					return StoreDataManager.charges[i];
				}
			}
			return null;
		}

		public static void BuyChargePoint(int id)
		{
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				if (StoreDataManager.StoreDatas[i].ChargePoint == id)
				{
					StoreDataManager.Buy(StoreDataManager.StoreDatas[i].ID);
				}
			}
		}

		public static StoreData GetPushGift()
		{
			List<WeaponGift> list = new List<WeaponGift>();
			for (int i = 0; i < StoreDataManager.PushGifts.Count; i++)
			{
				StoreData storeData = StoreDataManager.GetStoreData(StoreDataManager.PushGifts[i]);
				if (storeData != null && !storeData.Purchased)
				{
					WeaponGift item = default(WeaponGift);
					item.ID = StoreDataManager.PushGifts[i];
					for (int j = 0; j < storeData.ItemID.Length; j++)
					{
						if (storeData.ItemID[j] >= 2000 && storeData.ItemID[j] < 3000)
						{
							WeaponData weaponData = WeaponDataManager.GetWeaponData(storeData.ItemID[j]);
							if (weaponData != null)
							{
								item.Fighting = WeaponDataManager.GetMaxFightingStrength(weaponData);
							}
						}
					}
					list.Add(item);
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			list.Sort((WeaponGift a, WeaponGift b) => a.Fighting.CompareTo(b.Fighting));
			WeaponData maxFightingWeapon = WeaponDataManager.GetMaxFightingWeapon();
			int maxFightingStrength = WeaponDataManager.GetMaxFightingStrength(maxFightingWeapon);
			int num = -1;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].Fighting > maxFightingStrength)
				{
					num = k;
					break;
				}
			}
			if (num == -1)
			{
				return null;
			}
			if (num >= list.Count - 1)
			{
				return StoreDataManager.GetStoreData(list[num].ID);
			}
			if (list[num].ID == 9030)
			{
				return StoreDataManager.GetStoreData(list[num].ID);
			}
			if (UnityEngine.Random.Range(0, 100) < 50)
			{
				return StoreDataManager.GetStoreData(list[num].ID);
			}
			return StoreDataManager.GetStoreData(list[num + 1].ID);
		}

		private static void save()
		{
			StoreDataManager.SaveDatas.Clear();
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				StoreSaveData storeSaveData = new StoreSaveData();
				storeSaveData.ID = StoreDataManager.StoreDatas[i].ID;
				storeSaveData.Display = StoreDataManager.StoreDatas[i].Display;
				storeSaveData.Purchased = StoreDataManager.StoreDatas[i].Purchased;
				storeSaveData.Count = StoreDataManager.StoreDatas[i].Count;
				StoreDataManager.SaveDatas.Add(storeSaveData);
			}
			DataManager.SaveToJson<StoreSaveData>("StoreDatas", StoreDataManager.SaveDatas);
		}

		private static void read()
		{
			StoreDataManager.SaveDatas = DataManager.ParseJson<StoreSaveData>("StoreDatas");
			for (int i = 0; i < StoreDataManager.StoreDatas.Count; i++)
			{
				for (int j = 0; j < StoreDataManager.SaveDatas.Count; j++)
				{
					if (StoreDataManager.StoreDatas[i].ID == StoreDataManager.SaveDatas[j].ID)
					{
						StoreDataManager.StoreDatas[i].Purchased = StoreDataManager.SaveDatas[j].Purchased;
						StoreDataManager.StoreDatas[i].Display = StoreDataManager.SaveDatas[j].Display;
						StoreDataManager.StoreDatas[i].Count = StoreDataManager.SaveDatas[j].Count;
						break;
					}
				}
			}
		}

		public static List<StoreData> StoreDatas = new List<StoreData>();

		private static List<ChargePoint> charges = new List<ChargePoint>();

		private static List<StoreSaveData> SaveDatas = new List<StoreSaveData>();

		private static List<int> PushGifts = new List<int>
		{
			9030,
			9036,
			9037,
			9038,
			9039
		};
	}
}
