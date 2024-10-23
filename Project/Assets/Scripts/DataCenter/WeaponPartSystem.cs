using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class WeaponPartSystem
	{
		static WeaponPartSystem()
		{
			WeaponPartSystem.WeaponParts = DataManager.ParseXmlData<WeaponPartData>("WeaponPartData", "WeaponPartDatas", "WeaponPartData");
			if (PlayerPrefs.GetString("InitWeaponPartSystem", "true").Equals("true"))
			{
				PlayerPrefs.SetString("InitWeaponPartSystem", "false");
				WeaponPartSystem.DataInherit();
				WeaponPartSystem.save();
			}
			else
			{
				WeaponPartSystem.read();
			}
		}

		private static void DataInherit()
		{
			for (int i = 0; i < WeaponPartSystem.WeaponParts.Count; i++)
			{
				if (WeaponPartSystem.WeaponParts[i].Type == WeaponPartEnum.BULLET)
				{
					WeaponData weaponData = WeaponDataManager.GetWeaponData(WeaponPartSystem.WeaponParts[i].WeaponID);
					if (weaponData != null && weaponData.Type != WeaponType.SNIPER_RIFLE)
					{
						if (weaponData.Level != 0)
						{
							WeaponPartSystem.WeaponParts[i].Level = weaponData.Level;
						}
						else
						{
							WeaponPartSystem.WeaponParts[i].Level = 1;
						}
						if (WeaponPartSystem.WeaponParts[i].Level >= WeaponPartSystem.WeaponParts[i].MaxLevel)
						{
							WeaponPartSystem.WeaponParts[i].State = WeaponAttributeState.MAX;
						}
					}
				}
			}
		}

		public static WeaponPartData GetWaponPartData(int id, WeaponPartEnum part)
		{
			for (int i = 0; i < WeaponPartSystem.WeaponParts.Count; i++)
			{
				if (WeaponPartSystem.WeaponParts[i].WeaponID == id && WeaponPartSystem.WeaponParts[i].Type == part)
				{
					return WeaponPartSystem.WeaponParts[i];
				}
			}
			return null;
		}

		public static void GetAllParts(int id, ref List<WeaponPartData> list)
		{
			list.Clear();
			for (int i = 0; i < WeaponPartSystem.WeaponParts.Count; i++)
			{
				if (WeaponPartSystem.WeaponParts[i].WeaponID == id)
				{
					list.Add(WeaponPartSystem.WeaponParts[i]);
				}
			}
		}

		public static int GetAttribute(WeaponPartData part, WeaponAttribute _type, int _level)
		{
			int result = 0;
			switch (_type)
			{
			case WeaponAttribute.DAMAGE:
				result = part.Damage[_level];
				break;
			case WeaponAttribute.SHOOTSPEED:
				result = part.ShootSpeed[_level];
				break;
			case WeaponAttribute.MAGAZINES:
				result = part.Magazines[_level];
				break;
			case WeaponAttribute.BULLETS:
				result = part.Bullets[_level];
				break;
			case WeaponAttribute.RELOADINGTIME:
				result = part.ReloadingTime[_level];
				break;
			case WeaponAttribute.PRECISE:
				result = part.Precise[_level];
				break;
			case WeaponAttribute.SCOPETIMES:
				result = part.ScopeTimes[_level];
				break;
			}
			return result;
		}

		public static int GetFightStrength(int id, WeaponPartEnum part, int level)
		{
			WeaponPartData waponPartData = WeaponPartSystem.GetWaponPartData(id, part);
			if (waponPartData != null)
			{
				return waponPartData.FightingStrength * level;
			}
			return 0;
		}

		public static int GetFightingStrengthTotal(int id, int level)
		{
			List<WeaponPartData> list = new List<WeaponPartData>();
			WeaponPartSystem.GetAllParts(id, ref list);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += level * list[i].FightingStrength;
			}
			return num;
		}

		public static void StartUpgrade(WeaponPartData part)
		{
			part.State = WeaponAttributeState.UPGRADE;
			WeaponPartSystem.save();
		}

		public static void FinishUpgrade(WeaponPartData part)
		{
			part.Level++;
			if (part.Level < part.MaxLevel)
			{
				part.State = WeaponAttributeState.IDEL;
			}
			else
			{
				part.State = WeaponAttributeState.MAX;
			}
			WeaponPartSystem.save();
			AchievementDataManager.SetAchievementValue(AchievementType.WEAPON_UPGRADE, 1);
		}

		private static void save()
		{
			List<WeaponPartSaveData> list = new List<WeaponPartSaveData>();
			for (int i = 0; i < WeaponPartSystem.WeaponParts.Count; i++)
			{
				list.Add(new WeaponPartSaveData
				{
					ID = WeaponPartSystem.WeaponParts[i].ID,
					Level = WeaponPartSystem.WeaponParts[i].Level,
					State = WeaponPartSystem.WeaponParts[i].State
				});
			}
			DataManager.SaveToJson<WeaponPartSaveData>(string.Empty, list);
		}

		private static void read()
		{
			List<WeaponPartSaveData> list = DataManager.ParseJson<WeaponPartSaveData>(string.Empty);
			for (int i = 0; i < WeaponPartSystem.WeaponParts.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (WeaponPartSystem.WeaponParts[i].ID == list[j].ID)
					{
						WeaponPartSystem.WeaponParts[i].Level = list[j].Level;
						WeaponPartSystem.WeaponParts[i].State = list[j].State;
					}
				}
			}
		}

		private static List<WeaponPartData> WeaponParts = new List<WeaponPartData>();
	}
}
