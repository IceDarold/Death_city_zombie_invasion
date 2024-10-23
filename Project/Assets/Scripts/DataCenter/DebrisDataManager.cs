using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class DebrisDataManager
	{
		static DebrisDataManager()
		{
			DebrisDataManager.debris = DataManager.ParseXmlData<DebrisData>("DebrisData", "DebrisDatas", "DebrisData");
			if (PlayerPrefs.GetString("InitDebrisData", "true").Equals("true"))
			{
				PlayerPrefs.SetString("InitDebrisData", "false");
				DebrisDataManager.save();
			}
			else
			{
				DebrisDataManager.read();
			}
		}

		public static List<DebrisData> Debris
		{
			get
			{
				return DebrisDataManager.debris;
			}
		}

		public static DebrisData GetDebrisData(int id)
		{
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (DebrisDataManager.debris[i].ID == id)
				{
					return DebrisDataManager.debris[i];
				}
			}
			return null;
		}

		public static void CollcetDebris(int id, int num = 1)
		{
			DebrisData debrisData = DebrisDataManager.GetDebrisData(id);
			if (debrisData != null)
			{
				debrisData.Count += num;
				if (debrisData.ItemID >= 2000 && debrisData.ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(debrisData.ItemID);
					if (debrisData.Count >= weaponData.RequiredDebris && weaponData.State == WeaponState.未解锁)
					{
						WeaponDataManager.Unlock(weaponData);
						if (Singleton<UiManager>.Instance.CurrentPage.Name != PageName.InGamePage)
						{
							Singleton<UiManager>.Instance.ShowNewWeapon(weaponData);
						}
					}
				}
				else if (debrisData.ItemID >= 4000 && debrisData.ItemID < 5000)
				{
					PropData propData = PropDataManager.GetPropData(debrisData.ItemID);
					if (debrisData.Count >= propData.RequiredDebris)
					{
						PropDataManager.Unlock(propData);
					}
				}
				DebrisDataManager.save();
			}
		}

		public static bool IsDebrisEffective(int id)
		{
			DebrisData debrisData = DebrisDataManager.GetDebrisData(id);
			if (debrisData != null)
			{
				if (debrisData.ItemID >= 2000 && debrisData.ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(debrisData.ItemID);
					if (weaponData.State == WeaponState.未解锁)
					{
						return true;
					}
				}
				else if (debrisData.ItemID >= 4000 && debrisData.ItemID < 5000)
				{
					PropData propData = PropDataManager.GetPropData(debrisData.ItemID);
					if (propData.State == PropState.未解锁)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static List<DebrisData> GetWeaponDebrisList(int quality)
		{
			List<DebrisData> list = new List<DebrisData>();
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (DebrisDataManager.debris[i].ItemID >= 2000 && DebrisDataManager.debris[i].ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(DebrisDataManager.debris[i].ItemID);
					if (weaponData != null && weaponData.State != WeaponState.未解锁)
					{
						if (quality == 0)
						{
							list.Add(DebrisDataManager.debris[i]);
						}
						else if (weaponData.Quality == quality)
						{
							list.Add(DebrisDataManager.debris[i]);
						}
					}
				}
			}
			return list;
		}

		public static ItemData GetItemByDebris(int id)
		{
			DebrisData debrisData = DebrisDataManager.GetDebrisData(id);
			if (debrisData != null)
			{
				return ItemDataManager.GetItemData(debrisData.ItemID);
			}
			return null;
		}

		private static void SetCanDropDebris(ref List<DebrisData> list)
		{
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (DebrisDataManager.debris[i].DropLevel < CheckpointDataManager.GetCurrentCheckpoint().ID)
				{
					if (DebrisDataManager.debris[i].ItemID >= 2000 && DebrisDataManager.debris[i].ItemID < 3000)
					{
						WeaponData weaponData = WeaponDataManager.GetWeaponData(DebrisDataManager.debris[i].ItemID);
						if (weaponData.State == WeaponState.未解锁)
						{
							list.Add(DebrisDataManager.debris[i]);
						}
					}
					else if (DebrisDataManager.debris[i].ItemID >= 4000 && DebrisDataManager.debris[i].ItemID < 5000)
					{
						PropData propData = PropDataManager.GetPropData(DebrisDataManager.debris[i].ItemID);
						if (propData.State == PropState.未解锁)
						{
							list.Add(DebrisDataManager.debris[i]);
						}
					}
				}
			}
		}

		public static bool HasDebris()
		{
			List<DebrisData> list = new List<DebrisData>();
			DebrisDataManager.SetCanDropDebris(ref list);
			UnityEngine.Debug.Log("可掉落碎片数: " + list.Count);
			return list.Count > 0;
		}

		public static int GetTotalDebrisCount()
		{
			int num = 0;
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (DebrisDataManager.debris[i].ItemID >= 2000 && DebrisDataManager.debris[i].ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(DebrisDataManager.debris[i].ItemID);
					if (weaponData != null)
					{
						num += weaponData.RequiredDebris;
					}
				}
				else if (DebrisDataManager.debris[i].ItemID >= 4000 && DebrisDataManager.debris[i].ItemID < 5000)
				{
					PropData propData = PropDataManager.GetPropData(DebrisDataManager.debris[i].ItemID);
					if (propData != null)
					{
						num += propData.RequiredDebris;
					}
				}
			}
			return num;
		}

		public static int GetCurrentDebrisCount()
		{
			int num = 0;
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (DebrisDataManager.debris[i].ItemID >= 2000 && DebrisDataManager.debris[i].ItemID < 3000)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(DebrisDataManager.debris[i].ItemID);
					if (weaponData != null)
					{
						if (weaponData.State == WeaponState.未解锁)
						{
							num += DebrisDataManager.debris[i].Count;
						}
						else
						{
							num += weaponData.RequiredDebris;
						}
					}
				}
				else if (DebrisDataManager.debris[i].ItemID >= 4000 && DebrisDataManager.debris[i].ItemID < 5000)
				{
					PropData propData = PropDataManager.GetPropData(DebrisDataManager.debris[i].ItemID);
					if (propData != null)
					{
						if (propData.State == PropState.未解锁)
						{
							num += DebrisDataManager.debris[i].Count;
						}
						else
						{
							num += propData.RequiredDebris;
						}
					}
				}
			}
			return num;
		}

		public static DebrisData CollectDropDebris()
		{
			DebrisData debrisData = null;
			List<DebrisData> list = new List<DebrisData>();
			DebrisDataManager.SetCanDropDebris(ref list);
			if (list.Count > 0)
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num += list[i].Weight;
				}
				int num2 = UnityEngine.Random.Range(0, num);
				int num3 = 0;
				for (int j = 0; j < list.Count; j++)
				{
					num3 += list[j].Weight;
					if (num2 <= num3)
					{
						debrisData = list[j];
						break;
					}
				}
			}
			if (debrisData != null)
			{
				DebrisDataManager.CollcetDebris(debrisData.ID, 1);
			}
			return debrisData;
		}

		private static void save()
		{
			DebrisDataManager.saveDatas.Clear();
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				DebrisSaveData debrisSaveData = new DebrisSaveData();
				debrisSaveData.ID = DebrisDataManager.debris[i].ID;
				debrisSaveData.Count = DebrisDataManager.debris[i].Count;
				DebrisDataManager.saveDatas.Add(debrisSaveData);
			}
			DataManager.SaveToJson<DebrisSaveData>("DebrisDatas", DebrisDataManager.saveDatas);
		}

		private static void read()
		{
			DebrisDataManager.saveDatas = DataManager.ParseJson<DebrisSaveData>("DebrisDatas");
			int num = 0;
			for (int i = 0; i < DebrisDataManager.debris.Count; i++)
			{
				if (i - num < DebrisDataManager.saveDatas.Count && DebrisDataManager.debris[i].ID == DebrisDataManager.saveDatas[i - num].ID)
				{
					DebrisDataManager.debris[i].Count = DebrisDataManager.saveDatas[i - num].Count;
				}
				else
				{
					DebrisDataManager.debris[i].Count = 0;
					num++;
				}
			}
		}

		private static List<DebrisData> debris = new List<DebrisData>();

		private static List<DebrisSaveData> saveDatas = new List<DebrisSaveData>();
	}
}
