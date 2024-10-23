using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataCenter
{
	public class RoleDataManager
	{
		static RoleDataManager()
		{
			RoleDataManager.init();
		}

		public static List<RoleData> Roles
		{
			get
			{
				return RoleDataManager.roles;
			}
		}

		private static void init()
		{
			RoleDataManager.roles = DataManager.ParseXmlData<RoleData>("RoleData", "RoleDatas", "RoleData");
			RoleDataManager.attributes = DataManager.ParseXmlData<RoleAttribute>("RoleAttribute", "RoleAttributes", "RoleAttribute");
			if (PlayerPrefs.GetString("InitRoleDataManager", "true") == "true")
			{
				PlayerPrefs.SetString("InitRoleDataManager", "false");
				for (int i = 0; i < RoleDataManager.roles.Count; i++)
				{
					RoleDataManager.roles[i].Level = 1;
					if (RoleDataManager.roles[i].ID == 1001)
					{
						RoleDataManager.roles[i].Enable = true;
					}
					else
					{
						RoleDataManager.roles[i].Enable = false;
					}
				}
				RoleDataManager.save();
			}
			else
			{
				RoleDataManager.read();
			}
		}

		public static RoleData GetRoleData(int id)
		{
			for (int i = 0; i < RoleDataManager.roles.Count; i++)
			{
				if (RoleDataManager.roles[i].ID == id)
				{
					return RoleDataManager.roles[i];
				}
			}
			return null;
		}

		public static RoleAttribute GetRoleAttribute(int id)
		{
			for (int i = 0; i < RoleDataManager.attributes.Count; i++)
			{
				if (RoleDataManager.attributes[i].ID == id)
				{
					return RoleDataManager.attributes[i];
				}
			}
			return null;
		}

		public static void Unlcok(int id)
		{
			RoleData roleData = RoleDataManager.GetRoleData(id);
			roleData.Enable = true;
			roleData.isNew = true;
			AchievementDataManager.SetAchievementValue(AchievementType.FAMILY_PORTRAIT, 1);
			StoreDataManager.SetGiftPurchased(id);
			RoleDataManager.save();
		}

		public static void Upgrade(int id)
		{
			RoleData roleData = RoleDataManager.GetRoleData(id);
			roleData.Level++;
			RoleDataManager.save();
		}

		public static void RemoveNewTag(int id)
		{
			RoleData roleData = RoleDataManager.GetRoleData(id);
			if (roleData != null)
			{
				roleData.isNew = false;
				RoleDataManager.save();
			}
		}

		public static int GetUpgradeTime(int id)
		{
			RoleData roleData = RoleDataManager.GetRoleData(id);
			return (int)(100f * Mathf.Pow((float)roleData.Level, 0.5f));
		}

		public static int GetRoleHp(int id)
		{
			RoleData roleData = RoleDataManager.GetRoleData(id);
			return roleData.HP;
		}

		public static List<RoleAttribute> GetAttribute(RoleData role, bool isQTE)
		{
			int[] array = role.Attributes;
			List<RoleAttribute> list = new List<RoleAttribute>();
			List<RoleAttribute> list2 = new List<RoleAttribute>();
			for (int i = 0; i < array.Length; i++)
			{
				RoleAttribute roleAttribute = RoleDataManager.GetRoleAttribute(array[i]);
				if (roleAttribute.Mastery < (RoleMastery)100)
				{
					list2.Add(roleAttribute);
				}
				else
				{
					list.Add(roleAttribute);
				}
			}
			return (!isQTE) ? list2 : list;
		}

		private static void read()
		{
			RoleDataManager.RoleSaveDatas = DataManager.ParseJson<RoleSaveData>("RoleDatas");
			for (int i = 0; i < RoleDataManager.roles.Count; i++)
			{
				for (int j = 0; j < RoleDataManager.RoleSaveDatas.Count; j++)
				{
					if (RoleDataManager.roles[i].ID == RoleDataManager.RoleSaveDatas[j].ID)
					{
						RoleDataManager.roles[i].Enable = RoleDataManager.RoleSaveDatas[j].Enable;
						RoleDataManager.roles[i].Level = RoleDataManager.RoleSaveDatas[j].Level;
						RoleDataManager.roles[i].isNew = RoleDataManager.RoleSaveDatas[j].isNew;
					}
				}
			}
		}

		private static void save()
		{
			RoleDataManager.RoleSaveDatas.Clear();
			for (int i = 0; i < RoleDataManager.roles.Count; i++)
			{
				RoleSaveData roleSaveData = new RoleSaveData();
				roleSaveData.ID = RoleDataManager.roles[i].ID;
				roleSaveData.Level = RoleDataManager.roles[i].Level;
				roleSaveData.Enable = RoleDataManager.roles[i].Enable;
				RoleDataManager.RoleSaveDatas.Add(roleSaveData);
			}
			DataManager.SaveToJson<RoleSaveData>("RoleDatas", RoleDataManager.RoleSaveDatas);
		}

		public const int MaxLevel = 20;

		private static List<RoleData> roles = new List<RoleData>();

		private static List<RoleSaveData> RoleSaveDatas = new List<RoleSaveData>();

		private static List<RoleAttribute> attributes = new List<RoleAttribute>();
	}
}
