using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class WeaponDataManager
	{
		static WeaponDataManager()
		{
			WeaponDataManager.init();
		}

		public static List<WeaponData> Weapons
		{
			get
			{
				return WeaponDataManager.weapons;
			}
		}

		public static void init()
		{
			WeaponDataManager.weapons = DataManager.ParseXmlData<WeaponData>("WeaponData", "WeaponDatas", "WeaponData");
			if (PlayerPrefs.GetString("InitWeaponData", "true") == "true")
			{
				PlayerPrefs.SetString("InitWeaponData", "false");
				for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
				{
					if (WeaponDataManager.weapons[i].ID == 2001 || WeaponDataManager.weapons[i].ID == 2002)
					{
						WeaponDataManager.CollectWeapon(WeaponDataManager.weapons[i].ID);
					}
				}
				WeaponDataManager.save();
			}
			else
			{
				WeaponDataManager.read();
			}
		}

		public static WeaponData GetWeaponData(int id)
		{
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].ID == id)
				{
					return WeaponDataManager.weapons[i];
				}
			}
			return null;
		}

		public static WeaponData GetWeaponData(string _name)
		{
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].Prefab == _name)
				{
					return WeaponDataManager.weapons[i];
				}
			}
			return null;
		}

		public static void Unlock(int id)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(id);
			if (weaponData != null && weaponData.State == WeaponState.未解锁)
			{
				weaponData.State = WeaponState.待制作;
				weaponData.isNew = true;
				StoreDataManager.SetGiftPurchased(id);
				WeaponDataManager.save();
			}
		}

		public static void Unlock(WeaponData weapon)
		{
			if (weapon.State == WeaponState.未解锁)
			{
				weapon.State = WeaponState.待制作;
				weapon.isNew = true;
				StoreDataManager.SetGiftPurchased(weapon.ID);
				WeaponDataManager.save();
			}
		}

		public static void StartFabricate(int id)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(id);
			if (weaponData != null)
			{
				weaponData.State = WeaponState.制作中;
				WeaponDataManager.save();
			}
		}

		public static void FinishFabricate(int id)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(id);
			if (weaponData != null)
			{
				weaponData.State = WeaponState.待领取;
				WeaponDataManager.save();
			}
		}

		public static void CollectWeapon(int id)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(id);
			if (weaponData != null && weaponData.State < WeaponState.待升级)
			{
				AchievementDataManager.SetAchievementValue(AchievementType.GAIN_WEAPON, 1);
				weaponData.State = WeaponState.待升级;
				StoreDataManager.SetGiftPurchased(weaponData.ID);
				WeaponDataManager.save();
			}
		}

		public static void RemoveNewTag(int id)
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(id);
			if (weaponData != null)
			{
				weaponData.isNew = false;
				WeaponDataManager.save();
			}
		}

		public static void GetSameStateWeapon(ref List<WeaponData> list, WeaponState state)
		{
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].State == state)
				{
					list.Add(WeaponDataManager.weapons[i]);
				}
			}
		}

		public static int GetOwnDebris(int min, int max)
		{
			int num = 0;
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].Quality >= min && WeaponDataManager.weapons[i].Quality <= max)
				{
					if (WeaponDataManager.weapons[i].State == WeaponState.未解锁)
					{
						num += DebrisDataManager.GetDebrisData(WeaponDataManager.weapons[i].DebrisID).Count;
					}
					else
					{
						num += WeaponDataManager.weapons[i].RequiredDebris;
					}
				}
			}
			return num;
		}

		public static int GetTotalDebris(int min, int max)
		{
			int num = 0;
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].Quality >= min && WeaponDataManager.weapons[i].Quality <= max)
				{
					num += WeaponDataManager.weapons[i].RequiredDebris;
				}
			}
			return num;
		}

		public static void SetTryWeapon()
		{
			if (PlayerPrefs.GetString("FirstWeaponPush", "true").Equals("true"))
			{
				WeaponDataManager.TryWeapon = WeaponDataManager.GetWeaponData(WeaponDataManager.WeaponTryList[0]);
			}
			else
			{
				WeaponData maxFightingWeapon = WeaponDataManager.GetMaxFightingWeapon();
				for (int i = 0; i < WeaponDataManager.WeaponTryList.Length; i++)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(WeaponDataManager.WeaponTryList[i]);
					if (weaponData.State == WeaponState.未解锁 && WeaponDataManager.GetInitFightingStrength(weaponData) > WeaponDataManager.GetInitFightingStrength(maxFightingWeapon))
					{
						WeaponDataManager.TryWeapon = weaponData;
						break;
					}
				}
			}
		}

		public static WeaponData GetTryWeapon()
		{
			return WeaponDataManager.TryWeapon;
		}

		public static List<WeaponPartData> GetAllParts(WeaponData weapon)
		{
			List<WeaponPartData> list = new List<WeaponPartData>();
			for (int i = 0; i < weapon.Parts.Length; i++)
			{
				int num = i;
				if (weapon.Parts[num] != 0)
				{
					WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(weapon.ID, (WeaponPartEnum)num);
					list.Add(waponPartData);
				}
			}
			return list;
		}

		public static int GetBasicAttribute(WeaponData weapon, WeaponAttribute type)
		{
			int result = 0;
			switch (type)
			{
			case WeaponAttribute.DAMAGE:
				result = weapon.Damage;
				break;
			case WeaponAttribute.SHOOTSPEED:
				result = weapon.Speed;
				break;
			case WeaponAttribute.MAGAZINES:
				result = weapon.Magazines;
				break;
			case WeaponAttribute.BULLETS:
				result = weapon.Bullets;
				break;
			case WeaponAttribute.RELOADINGTIME:
				result = weapon.ReloadingTime;
				break;
			case WeaponAttribute.PRECISE:
				result = weapon.Precise;
				break;
			case WeaponAttribute.SCOPETIMES:
				result = weapon.ScopeTimes;
				break;
			}
			return result;
		}

		public static int GetAdditionalAttribute(WeaponData weapon, WeaponAttribute type)
		{
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			int num = 0;
			for (int i = 0; i < allParts.Count; i++)
			{
				switch (type)
				{
				case WeaponAttribute.DAMAGE:
					num += allParts[i].Damage[allParts[i].Level];
					break;
				case WeaponAttribute.SHOOTSPEED:
					num += allParts[i].ShootSpeed[allParts[i].Level];
					break;
				case WeaponAttribute.MAGAZINES:
					num += allParts[i].Magazines[allParts[i].Level];
					break;
				case WeaponAttribute.BULLETS:
					num += allParts[i].Bullets[allParts[i].Level];
					break;
				case WeaponAttribute.RELOADINGTIME:
					num += allParts[i].ReloadingTime[allParts[i].Level];
					break;
				case WeaponAttribute.PRECISE:
					num += allParts[i].Precise[allParts[i].Level];
					break;
				case WeaponAttribute.SCOPETIMES:
					num += allParts[i].ScopeTimes[allParts[i].Level];
					break;
				}
			}
			return num;
		}

		public static int GetCurrentAttribute(WeaponData weapon, WeaponAttribute type)
		{
			return WeaponDataManager.GetBasicAttribute(weapon, type) + WeaponDataManager.GetAdditionalAttribute(weapon, type);
		}

		public static int GetMaxAttribute(WeaponData weapon, WeaponAttribute type)
		{
			int num = WeaponDataManager.GetBasicAttribute(weapon, type);
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			for (int i = 0; i < allParts.Count; i++)
			{
				switch (type)
				{
				case WeaponAttribute.DAMAGE:
					num += allParts[i].Damage[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.SHOOTSPEED:
					num += allParts[i].ShootSpeed[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.MAGAZINES:
					num += allParts[i].Magazines[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.BULLETS:
					num += allParts[i].Bullets[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.RELOADINGTIME:
					num += allParts[i].ReloadingTime[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.PRECISE:
					num += allParts[i].Precise[allParts[i].MaxLevel];
					break;
				case WeaponAttribute.SCOPETIMES:
					num += allParts[i].ScopeTimes[allParts[i].MaxLevel];
					break;
				}
			}
			return num;
		}

		public static int GetAttribute(WeaponData weapon, WeaponAttribute type, int level)
		{
			int num = WeaponDataManager.GetBasicAttribute(weapon, type);
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			for (int i = 0; i < allParts.Count; i++)
			{
				switch (type)
				{
				case WeaponAttribute.DAMAGE:
					num += allParts[i].Damage[level];
					break;
				case WeaponAttribute.SHOOTSPEED:
					num += allParts[i].ShootSpeed[level];
					break;
				case WeaponAttribute.MAGAZINES:
					num += allParts[i].Magazines[level];
					break;
				case WeaponAttribute.BULLETS:
					num += allParts[i].Bullets[level];
					break;
				case WeaponAttribute.RELOADINGTIME:
					num += allParts[i].ReloadingTime[level];
					break;
				case WeaponAttribute.PRECISE:
					num += allParts[i].Precise[level];
					break;
				case WeaponAttribute.SCOPETIMES:
					num += allParts[i].ScopeTimes[level];
					break;
				}
			}
			return num;
		}

		public static int GetCurrentFightingStrength(WeaponData weapon)
		{
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			int num = weapon.BasicPower;
			for (int i = 0; i < allParts.Count; i++)
			{
				num += allParts[i].FightingStrength * allParts[i].Level;
			}
			return num;
		}

		public static int GetInitFightingStrength(WeaponData weapon)
		{
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			int num = weapon.BasicPower;
			for (int i = 0; i < allParts.Count; i++)
			{
				num += allParts[i].FightingStrength;
			}
			return num;
		}

		public static int GetMaxFightingStrength(WeaponData weapon)
		{
			List<WeaponPartData> allParts = WeaponDataManager.GetAllParts(weapon);
			int num = weapon.BasicPower;
			for (int i = 0; i < allParts.Count; i++)
			{
				num += allParts[i].FightingStrength * allParts[i].MaxLevel;
			}
			return num;
		}

		public static WeaponData GetMaxFightingWeapon()
		{
			WeaponData weaponData = null;
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].State != WeaponState.未解锁)
				{
					if (weaponData == null)
					{
						weaponData = WeaponDataManager.weapons[i];
					}
					else if (WeaponDataManager.GetCurrentFightingStrength(weaponData) < WeaponDataManager.GetCurrentFightingStrength(WeaponDataManager.weapons[i]))
					{
						weaponData = WeaponDataManager.weapons[i];
					}
				}
			}
			return weaponData;
		}

		public static WeaponData GetMaxFightingSnipeRifle()
		{
			WeaponData weaponData = null;
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				if (WeaponDataManager.weapons[i].State != WeaponState.未解锁 && WeaponDataManager.weapons[i].Type == WeaponType.SNIPER_RIFLE)
				{
					if (weaponData == null)
					{
						weaponData = WeaponDataManager.weapons[i];
					}
					else if (WeaponDataManager.GetCurrentFightingStrength(weaponData) < WeaponDataManager.GetCurrentFightingStrength(WeaponDataManager.weapons[i]))
					{
						weaponData = WeaponDataManager.weapons[i];
					}
				}
			}
			return weaponData;
		}

		private static void save()
		{
			List<WeaponSaveData> list = new List<WeaponSaveData>();
			for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
			{
				list.Add(new WeaponSaveData
				{
					ID = WeaponDataManager.weapons[i].ID,
					isNew = WeaponDataManager.weapons[i].isNew,
					State = WeaponDataManager.weapons[i].State,
					Level = WeaponDataManager.weapons[i].Level
				});
			}
			DataManager.SaveToJson<WeaponSaveData>("WeaponDatas", list);
		}

		private static void read()
		{
			List<WeaponSaveData> list = DataManager.ParseJson<WeaponSaveData>("WeaponDatas");
			if (list != null)
			{
				for (int i = 0; i < WeaponDataManager.weapons.Count; i++)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (WeaponDataManager.weapons[i].ID == list[j].ID)
						{
							WeaponDataManager.weapons[i].isNew = list[j].isNew;
							WeaponDataManager.weapons[i].State = list[j].State;
							WeaponDataManager.weapons[i].Level = list[j].Level;
							if (WeaponDataManager.weapons[i].State == WeaponState.升级中)
							{
								WeaponDataManager.weapons[i].Level++;
								WeaponDataManager.weapons[i].State = WeaponState.待升级;
							}
							else if (WeaponDataManager.weapons[i].State == WeaponState.已满级)
							{
								WeaponDataManager.weapons[i].State = WeaponState.待升级;
							}
						}
					}
				}
			}
		}

		public static int[] WeaponTryList = new int[]
		{
			2005,
			2007,
			2009,
			2010,
			2012,
			2013,
			2014
		};

		private static List<WeaponData> weapons = new List<WeaponData>();

		private static WeaponData TryWeapon = null;
	}
}
