using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Zombie3D;

public class GameConfig
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map0;
	public GameConfig()
	{
		float[] array = new float[4];
		array[1] = 9999f;
		this.gunAttributeMax = array;
		this.levelCfg = new Dictionary<int, LevelCfg>();
		
	}

	public MonsterConfig GetMonsterConfig(string name)
	{
		return this.monsterConfTable[name] as MonsterConfig;
	}

	public WeaponConfig GetWeaponConfig(string name)
	{
		IEnumerator enumerator = this.weaponConfTable.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				WeaponConfig weaponConfig = (WeaponConfig)obj;
				if (weaponConfig.name == name)
				{
					return weaponConfig;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	public AvatarConfig GetAvatarConfig(int index)
	{
		return this.avatarConfTable[index - 1] as AvatarConfig;
	}

	public List<WeaponConfig> GetPossibleLootWeapons(int wave)
	{
		List<WeaponConfig> list = new List<WeaponConfig>();
		IEnumerator enumerator = this.weaponConfTable.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				WeaponConfig weaponConfig = (WeaponConfig)obj;
				LootConfig lootConf = weaponConfig.lootConf;
				if (wave >= lootConf.fromWave && wave <= lootConf.toWave)
				{
					list.Add(weaponConfig);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	public WeaponConfig GetUnLockWeapon(int wave)
	{
		IEnumerator enumerator = this.weaponConfTable.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				WeaponConfig weaponConfig = (WeaponConfig)obj;
				LootConfig lootConf = weaponConfig.lootConf;
				if (wave == lootConf.giveAtWave)
				{
					return weaponConfig;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	public List<WeaponConfig> GetWeapons()
	{
		List<WeaponConfig> list = new List<WeaponConfig>();
		IEnumerator enumerator = this.weaponConfTable.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				WeaponConfig item = (WeaponConfig)obj;
				list.Add(item);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	public float GetLevelEnemyHp(EnemyType type, int levelID)
	{
		return this.levelCfg[levelID].E_NORMAL;
	}

	public List<LevelCfg> GetLevelCfgs()
	{
		List<LevelCfg> list = new List<LevelCfg>();
		foreach (KeyValuePair<int, LevelCfg> keyValuePair in this.levelCfg)
		{
			list.Add(keyValuePair.Value);
		}
		return list;
	}

	public LevelCfg GetLevelCfg(int levelID)
	{
		return this.levelCfg[levelID];
	}

	public void GetWeaponAttributeMax()
	{
	}

	public void LoadFromXML(string path)
	{
		this.globalConf = new GlobalConfig();
		this.playerConf = new PlayerConfig();
		XmlReader xmlReader = null;
		StringReader stringReader = null;
		Stream stream = null;
		if (path != null)
		{
			path = Application.dataPath + path;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			UnityEngine.Debug.Log("path = " + path);
			stream = File.Open(path + "config.xml", FileMode.Open);
			xmlReader = XmlReader.Create(stream);
		}
		WeaponConfig weaponConfig = null;
		while (xmlReader.Read())
		{
			XmlNodeType nodeType = xmlReader.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.EndElement)
				{
				}
			}
			else if (xmlReader.Name == "Global")
			{
				this.LoadGlobalConf(xmlReader);
			}
			else if (xmlReader.Name == "Player")
			{
				this.LoadPlayerConf(xmlReader);
			}
			else if (xmlReader.Name == "Avatar")
			{
				AvatarConfig avatarConfig = new AvatarConfig();
				this.LoadAvatarConf(xmlReader, avatarConfig);
				this.avatarConfTable.Add(avatarConfig);
			}
			else if (xmlReader.Name == "LevelCfg")
			{
				this.LoadLevelCfg(xmlReader);
			}
			else if (xmlReader.Name == "Monster")
			{
				this.LoadMonstersConf(xmlReader);
			}
			else if (xmlReader.Name == "Weapon")
			{
				weaponConfig = new WeaponConfig();
				this.LoadWeaponConf(xmlReader, weaponConfig);
				this.weaponConfTable.Add(weaponConfig);
			}
			else if (xmlReader.Name == "Damage")
			{
				this.LoadUpgradeConf(xmlReader, weaponConfig, "Damage");
			}
			else if (xmlReader.Name == "Frequency")
			{
				this.LoadUpgradeConf(xmlReader, weaponConfig, "Frequency");
			}
			else if (xmlReader.Name == "Accuracy")
			{
				this.LoadUpgradeConf(xmlReader, weaponConfig, "Accuracy");
			}
			else if (xmlReader.Name == "Reload")
			{
				this.LoadUpgradeConf(xmlReader, weaponConfig, "Reload");
			}
			else if (xmlReader.Name == "Charger")
			{
				this.LoadUpgradeConf(xmlReader, weaponConfig, "Charger");
			}
			else if (xmlReader.Name == "Loot")
			{
				this.LoadLootWeapon(xmlReader, weaponConfig);
			}
		}
		if (xmlReader != null)
		{
			xmlReader.Close();
		}
		if (stringReader != null)
		{
			stringReader.Close();
		}
		if (stream != null)
		{
			stream.Close();
		}
	}

	private void LoadLootWeapon(XmlReader reader, WeaponConfig weaponConf)
	{
		LootConfig lootConfig = new LootConfig();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "giveAtWave")
				{
					lootConfig.giveAtWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "fromWave")
				{
					lootConfig.fromWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "toWave")
				{
					lootConfig.toWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "lootRate")
				{
					lootConfig.rate = float.Parse(reader.Value);
				}
				else if (reader.Name == "increaseRate")
				{
					lootConfig.increaseRate = float.Parse(reader.Value);
				}
			}
		}
		weaponConf.lootConf = lootConfig;
	}

	private void LoadAvatarConf(XmlReader reader, AvatarConfig avatarConf)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "price")
				{
					avatarConf.price = int.Parse(reader.Value);
				}
			}
		}
	}

	private void LoadGlobalConf(XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "startMoney")
				{
					this.globalConf.startMoney = int.Parse(reader.Value);
				}
				else if (reader.Name == "startDiamond")
				{
					this.globalConf.startDiamond = int.Parse(reader.Value);
				}
				else if (reader.Name == "enemyLimit")
				{
					this.globalConf.enemyLimit = int.Parse(reader.Value);
				}
				else if (reader.Name == "resolution")
				{
					this.globalConf.resolution = float.Parse(reader.Value);
				}
			}
		}
	}

	private void LoadPlayerConf(XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "hp")
				{
					this.playerConf.hp = float.Parse(reader.Value);
				}
				else if (reader.Name == "walkSpeed")
				{
					this.playerConf.walkSpeed = float.Parse(reader.Value);
				}
				else if (reader.Name == "armorPrice")
				{
					this.playerConf.upgradeArmorPrice = int.Parse(reader.Value);
				}
				else if (reader.Name == "upPriceFactor")
				{
					this.playerConf.upPriceFactor = float.Parse(reader.Value);
				}
				else if (reader.Name == "maxArmorLevel")
				{
					this.playerConf.maxArmorLevel = int.Parse(reader.Value);
				}
			}
		}
	}

	private void LoadMonstersConf(XmlReader reader)
	{
		MonsterConfig monsterConfig = new MonsterConfig();
		string key = string.Empty;
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "name")
				{
					key = reader.Value;
				}
				else if (reader.Name == "damage")
				{
					monsterConfig.damage = float.Parse(reader.Value);
				}
				else if (reader.Name == "attackRate")
				{
					monsterConfig.attackRate = float.Parse(reader.Value);
				}
				else if (reader.Name == "walkSpeed")
				{
					monsterConfig.walkSpeed = float.Parse(reader.Value);
				}
				else if (reader.Name == "hp")
				{
					monsterConfig.hp = float.Parse(reader.Value);
				}
				else if (reader.Name == "rushSpeed")
				{
					monsterConfig.rushSpeed = float.Parse(reader.Value);
				}
				else if (reader.Name == "rushDamage")
				{
					monsterConfig.rushDamage = float.Parse(reader.Value);
				}
				else if (reader.Name == "rushAttack")
				{
					monsterConfig.rushAttackDamage = float.Parse(reader.Value);
				}
				else if (reader.Name == "rushRate")
				{
					monsterConfig.rushInterval = float.Parse(reader.Value);
				}
				else if (reader.Name == "score")
				{
					monsterConfig.score = int.Parse(reader.Value);
				}
				else if (reader.Name == "lootCash")
				{
					monsterConfig.lootCash = int.Parse(reader.Value);
				}
			}
		}
		this.monsterConfTable.Add(key, monsterConfig);
	}

	public void LoadLevelCfg(XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			LevelCfg levelCfg = new LevelCfg();
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					levelCfg.levelID = int.Parse(reader.Value);
				}
				else if (reader.Name == "dis")
				{
					levelCfg.desc = reader.Value.ToString();
				}
				else if (reader.Name == "mission")
				{
					levelCfg.mission = reader.Value.ToString();
				}
				else if (reader.Name == "recommend")
				{
					levelCfg.recommend = reader.Value.ToString();
				}
				else if (reader.Name == "needFight")
				{
					levelCfg.needFight = int.Parse(reader.Value);
				}
				else if (reader.Name == "reward")
				{
					levelCfg.reward = int.Parse(reader.Value);
				}
				else if (reader.Name == "E_NORMAL")
				{
					levelCfg.E_NORMAL = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_NV")
				{
					levelCfg.E_NV = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_PAPA")
				{
					levelCfg.E_PAPA = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_THROW")
				{
					levelCfg.E_THROW = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_ZIBAO")
				{
					levelCfg.E_ZIBAO = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_DOG")
				{
					levelCfg.E_DOG = float.Parse(reader.Value);
				}
				else if (reader.Name == "E_GUNZI")
				{
					levelCfg.E_GUNZI = float.Parse(reader.Value);
				}
			}
			this.levelCfg.Add(levelCfg.levelID, levelCfg);
		}
	}

	private void LoadWeaponConf(XmlReader reader, WeaponConfig weaponConf)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "name")
				{
					weaponConf.name = reader.Value;
				}
				else if (reader.Name == "type")
				{
					string value = reader.Value;
					if (value == null)
					{
						goto IL_18E;
					}
					if (GameConfig._003C_003Ef__switch_0024map0 == null)
					{
						GameConfig._003C_003Ef__switch_0024map0 = new Dictionary<string, int>(9)
						{
							{
								"Rifle",
								0
							},
							{
								"ShotGun",
								1
							},
							{
								"RPG",
								2
							},
							{
								"MachineGun",
								3
							},
							{
								"Laser",
								4
							},
							{
								"Sniper",
								5
							},
							{
								"Saw",
								6
							},
							{
								"HandGun",
								7
							},
							{
								"GrenadeRifle",
								8
							}
						};
					}
					int num;
					if (!GameConfig._003C_003Ef__switch_0024map0.TryGetValue(value, out num))
					{
						goto IL_18E;
					}
					switch (num)
					{
					case 0:
						weaponConf.wType = WeaponType.AssaultRifle;
						break;
					case 1:
						weaponConf.wType = WeaponType.ShotGun;
						break;
					case 2:
						weaponConf.wType = WeaponType.RocketLauncher;
						break;
					case 3:
						weaponConf.wType = WeaponType.MachineGun;
						break;
					case 4:
						weaponConf.wType = WeaponType.LaserGun;
						break;
					case 5:
						weaponConf.wType = WeaponType.Sniper;
						break;
					case 6:
						weaponConf.wType = WeaponType.Saw;
						break;
					case 7:
						weaponConf.wType = WeaponType.HandGun;
						break;
					case 8:
						weaponConf.wType = WeaponType.GrenadeRifle;
						break;
					case 9:
						goto IL_18E;
					default:
						goto IL_18E;
					}
					continue;
					IL_18E:
					UnityEngine.Debug.LogError("GunType error");
				}
				else if (reader.Name == "moveSpeedDrag")
				{
					weaponConf.moveSpeedDrag = float.Parse(reader.Value);
				}
				else if (reader.Name == "range")
				{
					weaponConf.range = float.Parse(reader.Value);
				}
				else if (reader.Name == "price")
				{
					weaponConf.price = int.Parse(reader.Value);
				}
				else if (reader.Name == "bulletPrice")
				{
					weaponConf.bulletPrice = int.Parse(reader.Value);
				}
				else if (reader.Name == "initBullet")
				{
					weaponConf.initBullet = int.Parse(reader.Value);
				}
				else if (reader.Name == "bullet")
				{
					weaponConf.bullet = int.Parse(reader.Value);
				}
				else if (reader.Name == "flySpeed")
				{
					weaponConf.flySpeed = float.Parse(reader.Value);
				}
				else if (reader.Name == "startEquip")
				{
					weaponConf.startEquip = (WeaponExistState)int.Parse(reader.Value);
				}
				else if (reader.Name == "uiSort")
				{
					weaponConf.uiSort = int.Parse(reader.Value);
				}
				else if (reader.Name == "gunName")
				{
					weaponConf.gunName = reader.Value;
				}
				else if (reader.Name == "gunDesc")
				{
					weaponConf.gunDesc = reader.Value;
				}
			}
		}
		this.GetWeaponAttributeMax();
	}

	private void LoadUpgradeConf(XmlReader reader, WeaponConfig weaponConf, string uType)
	{
		UpgradeConfig upgradeConfig = new UpgradeConfig();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "base")
				{
					upgradeConfig.baseData = float.Parse(reader.Value);
				}
				else if (reader.Name == "upFactor")
				{
					upgradeConfig.upFactor = float.Parse(reader.Value);
				}
				else if (reader.Name == "basePrice")
				{
					upgradeConfig.basePrice = float.Parse(reader.Value);
				}
				else if (reader.Name == "upPriceFactor")
				{
					upgradeConfig.upPriceFactor = float.Parse(reader.Value);
				}
				else if (reader.Name == "maxLevel")
				{
					upgradeConfig.maxLevel = (float)int.Parse(reader.Value);
				}
			}
		}
		if (uType != null)
		{
			if (!(uType == "Damage"))
			{
				if (!(uType == "Frequency"))
				{
					if (!(uType == "Accuracy"))
					{
						if (!(uType == "Reload"))
						{
							if (uType == "Charger")
							{
								weaponConf.chargerConf = upgradeConfig;
							}
						}
						else
						{
							weaponConf.reloadConf = upgradeConfig;
						}
					}
					else
					{
						weaponConf.accuracyConf = upgradeConfig;
					}
				}
				else
				{
					weaponConf.attackRateConf = upgradeConfig;
				}
			}
			else
			{
				weaponConf.damageConf = upgradeConfig;
			}
		}
	}

	public void LoadRoleAttributeConfig()
	{
	}

	public RoleAttributes GetRoleAttribute(AvatarType type)
	{
		return this.roleAttributeConfig[(int)type];
	}

	public GlobalConfig globalConf;

	public PlayerConfig playerConf;

	public ArrayList avatarConfTable = new ArrayList();

	public Hashtable monsterConfTable = new Hashtable();

	public ArrayList weaponConfTable = new ArrayList();

	public Hashtable equipConfTable = new Hashtable();

	public List<RoleAttributes> roleAttributeConfig;

	public float[] gunAttributeMax;

	protected Dictionary<int, LevelCfg> levelCfg;
}
