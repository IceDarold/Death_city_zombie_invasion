using UnityEngine;
using Zombie3D;

namespace DataCenter
{
	public class PlayerDataManager
	{
		static PlayerDataManager()
		{
			PlayerDataManager.init();
			PlayerDataManager.SetStatisticsDatas(PlayerStatistics.GameLogonTimes, 1);
		}

		public static PlayerData Player
		{
			get
			{
				return PlayerDataManager.player;
			}
		}

		private static void init()
		{
			if (PlayerPrefs.GetString("InitPlayerDataManager", "true") == "true")
			{
				PlayerPrefs.SetString("InitPlayerDataManager", "false");
				PlayerDataManager.player.Name = "Player";
				PlayerDataManager.player.Level = 1;
				PlayerDataManager.player.Experience = 0;
				PlayerDataManager.player.Role = 1001;
				PlayerDataManager.player.Weapon1 = 2001;
				PlayerDataManager.player.Weapon2 = 0;
				PlayerDataManager.player.Prop2 = 4001;
				PlayerDataManager.player.Cap = 0;
				PlayerDataManager.player.Coat = 0;
				PlayerDataManager.player.Shoes = 0;
				PlayerDataManager.Save();
			}
			else
			{
				PlayerDataManager.Read();
			}
		}

		public static void SetStatisticsDatas(PlayerStatistics statistics, int count = 1)
		{
			switch (statistics)
			{
			case PlayerStatistics.LogonDays:
				PlayerDataManager.player.LogonDays += count;
				if (PlayerDataManager.player.LogonDays == 1)
				{
					GameLogManager.SendPageLog("FirstLogon", "null");
				}
				break;
			case PlayerStatistics.LotteryDrawTimes:
				PlayerDataManager.player.LotteryDrawTimes += count;
				break;
			case PlayerStatistics.EarnGoldCoins:
				PlayerDataManager.player.EarnGoldCoins += count;
				break;
			case PlayerStatistics.EarnDiamonds:
				PlayerDataManager.player.EarnDiamonds += count;
				break;
			case PlayerStatistics.KillZombieCounts:
				PlayerDataManager.player.KillZombieCounts += count;
				break;
			case PlayerStatistics.TotalShootTimes:
				PlayerDataManager.player.TotalShootTimes += count;
				break;
			case PlayerStatistics.ShootHitTimes:
				PlayerDataManager.player.ShootHitTimes += count;
				break;
			case PlayerStatistics.ShootHeadTimes:
				PlayerDataManager.player.ShootHeadTimes += count;
				break;
			case PlayerStatistics.CheckpointPassedTimes:
				PlayerDataManager.player.CheckpointPassedTimes += count;
				break;
			case PlayerStatistics.GameDuration:
				PlayerDataManager.player.GameDuration += count;
				break;
			case PlayerStatistics.GameLogonTimes:
				PlayerDataManager.player.GameLogonTimes += count;
				break;
			}
			PlayerDataManager.Save();
		}

		public static int GetStatisticsDatas(PlayerStatistics statistics)
		{
			switch (statistics)
			{
			case PlayerStatistics.LogonDays:
				return PlayerDataManager.player.LogonDays;
			case PlayerStatistics.LotteryDrawTimes:
				return PlayerDataManager.player.LotteryDrawTimes;
			case PlayerStatistics.EarnGoldCoins:
				return PlayerDataManager.player.EarnGoldCoins;
			case PlayerStatistics.EarnDiamonds:
				return PlayerDataManager.player.EarnDiamonds;
			case PlayerStatistics.KillZombieCounts:
				return PlayerDataManager.player.KillZombieCounts;
			case PlayerStatistics.TotalShootTimes:
				return PlayerDataManager.player.TotalShootTimes;
			case PlayerStatistics.ShootHitTimes:
				return PlayerDataManager.player.ShootHitTimes;
			case PlayerStatistics.ShootHeadTimes:
				return PlayerDataManager.player.ShootHeadTimes;
			case PlayerStatistics.CheckpointPassedTimes:
				return PlayerDataManager.player.CheckpointPassedTimes;
			case PlayerStatistics.GameDuration:
				return PlayerDataManager.player.GameDuration;
			case PlayerStatistics.GameLogonTimes:
				return PlayerDataManager.player.GameLogonTimes;
			default:
				return 0;
			}
		}

		public static void SelectRole(int id)
		{
			PlayerDataManager.player.Role = id;
			PlayerDataManager.Save();
		}

		public static AvatarType GetRole()
		{
			switch (PlayerDataManager.player.Role)
			{
			case 1001:
				return AvatarType.Sam;
			case 1002:
				return AvatarType.Laura;
			case 1003:
				return AvatarType.Johnson;
			default:
				return AvatarType.Sam;
			}
		}

		public static void Equip(EquipmentPosition pos, int id)
		{
			switch (pos)
			{
			case EquipmentPosition.Weapon1:
				PlayerDataManager.player.Weapon1 = id;
				break;
			case EquipmentPosition.Weapon2:
				PlayerDataManager.player.Weapon2 = id;
				break;
			case EquipmentPosition.Prop1:
				PlayerDataManager.player.Prop1 = id;
				break;
			case EquipmentPosition.Prop2:
				PlayerDataManager.player.Prop2 = id;
				break;
			case EquipmentPosition.Cap:
				PlayerDataManager.player.Cap = id;
				break;
			case EquipmentPosition.Coat:
				PlayerDataManager.player.Coat = id;
				break;
			case EquipmentPosition.Shoes:
				PlayerDataManager.player.Shoes = id;
				break;
			}
			PlayerDataManager.Save();
		}

		public static PlayerAttributeData GetEquipmentAttribute()
		{
			PlayerAttributeData result = default(PlayerAttributeData);
			PlayerDataManager.SetEquipmentAttribute(PlayerDataManager.player.Cap, ref result);
			PlayerDataManager.SetEquipmentAttribute(PlayerDataManager.player.Coat, ref result);
			PlayerDataManager.SetEquipmentAttribute(PlayerDataManager.player.Shoes, ref result);
			PlayerDataManager.SetEquipmentSetsAttribute(ref result);
			return result;
		}

		private static void SetEquipmentAttribute(int id, ref PlayerAttributeData atr)
		{
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(id);
			if (equipmentData != null)
			{
				switch (equipmentData.AttributeType)
				{
				case EquipmentAttribute.HEALTH:
					atr.attrHP += (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				case EquipmentAttribute.MOVE_SPEED:
					atr.moveSpeed += (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				case EquipmentAttribute.DODGE:
					atr.dodge += (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				case EquipmentAttribute.EXPLODE_DEFENSE:
					atr.bombDefense = (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				case EquipmentAttribute.RANGED_DEFENSE:
					atr.remoteDefense += (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				case EquipmentAttribute.ATTACK:
					atr.attack += (float)equipmentData.AttributeValue[equipmentData.Level];
					break;
				}
			}
		}

		private static void SetEquipmentSetsAttribute(ref PlayerAttributeData atr)
		{
			if (EquipmentDataManager.isSetsActivated(PlayerDataManager.player.Cap) < 3)
			{
				return;
			}
			EquipmentData equipmentData = EquipmentDataManager.GetEquipmentData(PlayerDataManager.player.Cap);
			if (equipmentData == null)
			{
				return;
			}
			EquipmentSetData setData = EquipmentDataManager.GetSetData(equipmentData.SetID);
			switch (setData.Type)
			{
			case EquipmentAttribute.HEALTH:
				atr.attrHP += setData.Value * 0.01f * atr.attrHP;
				break;
			case EquipmentAttribute.MOVE_SPEED:
				atr.moveSpeed += setData.Value * 0.01f * atr.moveSpeed;
				break;
			case EquipmentAttribute.DODGE:
				atr.dodge += setData.Value * 0.01f * atr.dodge;
				break;
			case EquipmentAttribute.EXPLODE_DEFENSE:
				atr.bombDefense = setData.Value * 0.01f * atr.bombDefense;
				break;
			case EquipmentAttribute.RANGED_DEFENSE:
				atr.remoteDefense += setData.Value * 0.01f * atr.remoteDefense;
				break;
			case EquipmentAttribute.ATTACK:
				atr.attack += setData.Value * 0.01f * atr.attack;
				break;
			}
		}

		public static bool CanUpgrade()
		{
			return PlayerDataManager.player.Level < 100 && PlayerDataManager.player.Experience >= PlayerDataManager.GetLevelUpExp();
		}

		public static void Upgrade()
		{
			PlayerDataManager.player.Experience -= PlayerDataManager.GetLevelUpExp();
			PlayerDataManager.player.Level++;
			PlayerDataManager.Save();
		}

		public static void AddExperience(int count)
		{
			PlayerDataManager.player.Experience += count;
			PlayerDataManager.Save();
		}

		public static int GetLevelUpExp()
		{
			return 80 + 40 * (PlayerDataManager.player.Level - 1);
		}

		public static int[] GetUpgradeAward()
		{
			return new int[]
			{
				500 + PlayerDataManager.player.Level * 100,
				2
			};
		}

		public static int GetCurrentFighting()
		{
			WeaponData weaponData = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
			WeaponData weaponData2 = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
			int num = 0;
			int num2 = 0;
			if (weaponData != null)
			{
				num = WeaponDataManager.GetCurrentFightingStrength(weaponData);
			}
			if (weaponData2 != null)
			{
				num2 = WeaponDataManager.GetCurrentFightingStrength(weaponData2);
			}
			return (num < num2) ? num2 : num;
		}

		public static bool isFightingStrengthShortage(CheckpointData data)
		{
			if (data.Type == CheckpointType.RACING)
			{
				return false;
			}
			int requireFighting = data.RequireFighting;
			if (PlayerDataManager.Player.Weapon1 != 0)
			{
				WeaponData weaponData = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
				if (WeaponDataManager.GetCurrentFightingStrength(weaponData) >= requireFighting)
				{
					return false;
				}
			}
			if (PlayerDataManager.Player.Weapon2 != 0)
			{
				WeaponData weaponData2 = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
				if (WeaponDataManager.GetCurrentFightingStrength(weaponData2) >= requireFighting)
				{
					return false;
				}
			}
			return true;
		}

		private static void Save()
		{
			string value = JsonUtility.ToJson(PlayerDataManager.player);
			PlayerPrefs.SetString("SAVE_KEY_PLAYER", value);
		}

		private static void Read()
		{
			string @string = PlayerPrefs.GetString("SAVE_KEY_PLAYER");
			if (PlayerPrefs.HasKey("SAVE_KEY_PLAYER"))
			{
				PlayerDataManager.player = JsonUtility.FromJson<PlayerData>(@string);
			}
		}

		public const int MaxPlayerLevel = 100;

		private static PlayerData player = new PlayerData();
	}
}
