using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using Mono.Xml;
using UnityEngine;

namespace Zombie3D
{
	public class GameState
	{
		public GameState()
		{
			UnityEngine.Debug.Log("create game state");
			this.inited = false;
			this.weaponList = new List<Weapon>();
			this.AlreadyCountered = false;
			this.AlreadyPopReview = false;
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event Action CashNum;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event Action DiamondNum;

		public int LevelNum { get; set; }

		public float MenuMusicTime { get; set; }

		public bool FirstTimeGame { get; set; }

		public AvatarType Avatar { get; set; }

		public int TryWeaponIndex { get; set; }

		public bool MusicOn { get; set; }

		public bool EffectOn { get; set; }

		public bool AlreadyCountered { get; set; }

		public bool AlreadyPopReview { get; set; }

		public int endlessModeCompleteWave { get; set; }

		public int endlessModeGotWave { get; set; }

		public int endlessModeScore { get; set; }

		public int lotteryTimes { get; set; }

		public bool FromShopMenu
		{
			get
			{
				return this.fromShopMenu;
			}
			set
			{
				this.fromShopMenu = value;
			}
		}

		public void AddScore(int scoreAdd)
		{
			this.score += scoreAdd;
		}

		public int GetScore()
		{
			return this.score;
		}

		public AvatarState GetAvatarData(AvatarType aType)
		{
			return this.avatarData[(int)aType];
		}

		public int GetAvatarNum()
		{
			return this.avatarData.Length;
		}

		public int[] GetRectToWeaponMap()
		{
			return this.rectToWeaponMap;
		}

		public bool GotAllWeapons()
		{
			for (int i = 0; i < this.weaponList.Count; i++)
			{
				if (this.weaponList[i].Exist != WeaponExistState.Owned)
				{
					return false;
				}
			}
			return true;
		}

		public int GetOwnedWeaponNum()
		{
			int num = 0;
			for (int i = 0; i < this.weaponList.Count; i++)
			{
				if (this.weaponList[i].Exist == WeaponExistState.Owned)
				{
					num++;
				}
			}
			return num;
		}

		public void EnableAvatar(AvatarType aType)
		{
			this.avatarData[(int)aType] = AvatarState.Avaliable;
		}

		public List<T> InitArenaReward<T>(string xmlName) where T : new()
		{
			List<T> list = new List<T>();
			FieldInfo[] fields = typeof(T).GetFields();
			SecurityParser securityParser = new SecurityParser();
			securityParser.LoadXml(Resources.Load(xmlName).ToString());
			SecurityElement securityElement = securityParser.ToXml();
			IEnumerator enumerator = securityElement.Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					SecurityElement securityElement2 = (SecurityElement)obj;
					T t = Activator.CreateInstance<T>();
					if (securityElement2.Attributes != null)
					{
						for (int i = 0; i < fields.Length; i++)
						{
							if (securityElement2.Attribute(fields[i].Name) != null)
							{
								fields[i].SetValue(t, Convert.ChangeType(securityElement2.Attribute(fields[i].Name), fields[i].FieldType));
							}
						}
					}
					list.Add(t);
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

		public void InitLevelData()
		{
			if (this.isLevelInit)
			{
				return;
			}
			this.isLevelInit = true;
			List<LevelCfg> levelCfgs = this.gConfig.GetLevelCfgs();
			foreach (LevelCfg levelCfg in levelCfgs)
			{
				if (levelCfg.levelID == 1)
				{
					levelCfg.state = LevelState.Open;
				}
				this.levelData.Add(levelCfg.levelID, levelCfg);
			}
		}

		public LevelCfg GetLevelData(int levelID)
		{
			return this.levelData[levelID];
		}

		public int GetMaxLevelID()
		{
			for (int i = 1; i <= 30; i++)
			{
				if (this.levelData[i].state == LevelState.Lock)
				{
					return i - 1;
				}
			}
			return 30;
		}

		public void UnlockLevel(int levelID)
		{
			this.levelData[levelID].state = LevelState.Open;
		}

		public void CompleteLevel(int levelID)
		{
			if (this.levelData[levelID].state == LevelState.Open)
			{
			}
			this.levelData[levelID].state = LevelState.Complete;
		}

		public int GetCompleteLevelNum()
		{
			for (int i = 1; i <= 30; i++)
			{
				if (i == 1 && this.levelData[1].state == LevelState.Open)
				{
					return 0;
				}
				if (this.levelData[i].state == LevelState.Open && this.levelData[i - 1].state == LevelState.Complete)
				{
					return i - 1;
				}
				if (i == 30 && this.levelData[i].state == LevelState.Complete)
				{
					return 30;
				}
			}
			return 30;
		}

		public void LoadData(BinaryReader br)
		{
		}

		public int GetWeaponIndex(Weapon w)
		{
			return this.weaponList.FindIndex((Weapon wp) => wp == w);
		}

		public void SaveData(BinaryWriter bw)
		{
		}

		public void ClearState()
		{
			this.inited = false;
			this.weaponsInited = false;
			this.Init();
		}

		public void Init()
		{
			if (!this.inited)
			{
				UnityEngine.Debug.Log("game state init");
				this.cash = this.gConfig.globalConf.startMoney;
				this.diamond = this.gConfig.globalConf.startDiamond;
				this.MusicOn = true;
				this.EffectOn = true;
				this.infectionRate = new int[4];
				for (int i = 0; i < 4; i++)
				{
					this.infectionRate[i] = 100;
				}
				this.infectionRate[2] = 100;
				this.Avatar = AvatarType.Sam;
				this.TryWeaponIndex = -1;
				this.avatarData = new AvatarState[8];
				for (int j = 0; j < this.avatarData.Length; j++)
				{
					this.avatarData[j] = AvatarState.ToBuy;
				}
				this.avatarData[0] = AvatarState.Avaliable;
				this.FirstTimeGame = true;
				this.LevelNum = 1;
				this.inited = true;
				this.endlessModeCompleteWave = 0;
				this.endlessModeGotWave = 0;
				this.lotteryTimes = 0;
				Singleton<DynamicData>.Instance.teachFinished = false;
			}
		}

		public void InitWeapons()
		{
			if (!this.weaponsInited)
			{
				this.weaponList.Clear();
				for (int i = 0; i < this.rectToWeaponMap.Length; i++)
				{
					this.rectToWeaponMap[i] = -1;
				}
				List<WeaponConfig> weapons = this.gConfig.GetWeapons();
				foreach (WeaponConfig weaponConfig in weapons)
				{
					string name = weaponConfig.name;
					Weapon weapon = WeaponFactory.GetInstance().CreateWeapon(weaponConfig.wType);
					weapon.Exist = weaponConfig.startEquip;
					weapon.Name = name;
					weapon.WConf = weaponConfig;
					weapon.LoadConfig();
					this.weaponList.Add(weapon);
				}
				foreach (Weapon weapon2 in this.weaponList)
				{
					if (weapon2.Name == "AK47")
					{
						weapon2.IsSelectedForBattle = true;
						this.rectToWeaponMap[0] = this.GetWeaponIndex(weapon2);
					}
					else if (weapon2.Name == "WINCHERSTER")
					{
						weapon2.IsSelectedForBattle = false;
						this.rectToWeaponMap[1] = this.GetWeaponIndex(weapon2);
					}
				}
				this.weaponsInited = true;
			}
		}

		public List<Weapon> GetBattleWeapons()
		{
			List<Weapon> list = new List<Weapon>();
			for (int i = 0; i < this.rectToWeaponMap.Length; i++)
			{
				if (this.rectToWeaponMap[i] != -1)
				{
					list.Add(this.weaponList[this.rectToWeaponMap[i]]);
				}
			}
			return list;
		}

		public Weapon GetWeaponByName(string name)
		{
			foreach (Weapon weapon in this.weaponList)
			{
				if (weapon.Name == name)
				{
					return weapon;
				}
			}
			return null;
		}

		public int GetWeaponByOwnedIndex(int index)
		{
			int num = 0;
			for (int i = 0; i < this.weaponList.Count; i++)
			{
				Weapon weapon = this.weaponList[i];
				if (weapon.Exist == WeaponExistState.Owned)
				{
					if (num == index)
					{
						return i;
					}
					num++;
				}
			}
			return -1;
		}

		public int Diamond
		{
			get
			{
				return this.diamond;
			}
		}

		public void RegisterCashNum(Action _delegate)
		{
			this.CashNum += _delegate;
		}

		public void RemoveCashNum(Action _delegate)
		{
			this.CashNum -= _delegate;
		}

		public void RegisterDiaNum(Action _delegate)
		{
			this.DiamondNum += _delegate;
		}

		public void RemoveDiaNum(Action _delegate)
		{
			this.DiamondNum -= _delegate;
		}

		public bool Alert_diamond(int _delta)
		{
			if (this.diamond + _delta < 0)
			{
				return false;
			}
			this.diamond += _delta;
			if (this.DiamondNum != null)
			{
				this.DiamondNum();
			}
			if (_delta < 0)
			{
				Singleton<DailyTaskMgr>.Instance.AddDailyTaskNum(DailyTask.CONSUME_DIA, Mathf.Abs(_delta), true);
			}
			return true;
		}

		public bool Alert_cash(int _delta)
		{
			if (this.cash + _delta < 0)
			{
				return false;
			}
			this.cash += _delta;
			if (this.CashNum != null)
			{
				this.CashNum();
			}
			if (_delta < 0)
			{
				Singleton<DailyTaskMgr>.Instance.AddDailyTaskNum(DailyTask.CONSUME_CASH, Mathf.Abs(_delta), true);
			}
			return true;
		}

		public void LoseCash(int cashSpend)
		{
			this.cash -= cashSpend;
			if (this.CashNum != null)
			{
				this.CashNum();
			}
		}

		public int Cash
		{
			get
			{
				return this.cash;
			}
		}

		public int GetCash()
		{
			return this.cash;
		}

		public List<Weapon> GetWeapons()
		{
			return this.weaponList;
		}

		public WeaponBuyStatus BuyWeapon(Weapon w, int price)
		{
			int weaponIndex = this.GetWeaponIndex(w);
			if (this.cash >= price)
			{
				this.weaponList[weaponIndex].Exist = WeaponExistState.Owned;
				this.LoseCash(price);
				return WeaponBuyStatus.Succeed;
			}
			UnityEngine.Debug.Log("Not Enough Cash!");
			return WeaponBuyStatus.NotEnoughCash;
		}

		public bool BuyAvatar(AvatarType aType, int price)
		{
			if (this.avatarData[(int)aType] == AvatarState.ToBuy && this.cash >= price)
			{
				this.avatarData[(int)aType] = AvatarState.Avaliable;
				this.LoseCash(price);
				return true;
			}
			return false;
		}

		public WeaponType RandomWeaponAlreadyHave()
		{
			int count = this.weaponList.Count;
			int index = UnityEngine.Random.Range(0, count);
			if (count != 0)
			{
				return this.weaponList[index].GetWeaponType();
			}
			return WeaponType.AssaultRifle;
		}

		public WeaponType RandomBattleWeapons()
		{
			List<Weapon> list = new List<Weapon>();
			foreach (Weapon weapon in this.weaponList)
			{
				if (weapon.IsSelectedForBattle && weapon.GetWeaponType() != WeaponType.Saw)
				{
					list.Add(weapon);
				}
			}
			int count = list.Count;
			int index = UnityEngine.Random.Range(0, count);
			if (count != 0)
			{
				return list[index].GetWeaponType();
			}
			return WeaponType.AssaultRifle;
		}

		public bool UpgradeWeapon(Weapon w, float power, float frequency, float accuracy, float reload, int charger, int price)
		{
			if (this.cash >= price)
			{
				w.Upgrade(power, frequency, accuracy, reload, charger);
				this.LoseCash(price);
				return true;
			}
			return false;
		}

		public bool UpgradeArmor(int price)
		{
			return false;
		}

		public int GetLevelStar(int levelIndex)
		{
			return this.levelStars[levelIndex - 1];
		}

		public void SetLevelStar(int levelIndex, int starNum)
		{
			this.levelStars[levelIndex - 1] = starNum;
			GameApp.GetInstance().Save();
		}

		protected int cash;

		protected int score;

		protected int diamond;

		protected int[] infectionRate;

		protected List<Weapon> weaponList;

		protected Dictionary<int, LevelCfg> levelData = new Dictionary<int, LevelCfg>();

		protected bool isLevelInit;

		protected AvatarState[] avatarData;

		protected GameConfig gConfig;

		protected bool inited;

		protected bool weaponsInited;

		public float cameraSwingSpeed = 25f;

		protected int[] levelStars = new int[20];

		protected int[] rectToWeaponMap = new int[3];

		protected bool fromShopMenu;

		protected Dictionary<AvatarType, int[]> roleLevelData = new Dictionary<AvatarType, int[]>();

		public int[] ShopGot = new int[]
		{
			120,
			330,
			690,
			120000,
			330000
		};

		public int[] ShopPrice = new int[]
		{
			6,
			15,
			30,
			6,
			15
		};

		public int needGot;

		protected int preMax;
	}
}
