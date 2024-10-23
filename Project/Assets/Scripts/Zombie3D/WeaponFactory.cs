using System;
using UnityEngine;

namespace Zombie3D
{
	public class WeaponFactory
	{
		public static WeaponFactory GetInstance()
		{
			if (WeaponFactory.instance == null)
			{
				WeaponFactory.instance = new WeaponFactory();
			}
			return WeaponFactory.instance;
		}

		public Weapon CreateWeapon(string _name)
		{
			Weapon result = null;
			switch (_name)
			{
			case "AK47":
			case "BREN":
			case "FAMAS":
			case "G36":
			case "HK416":
			case "M4":
			case "M10":
			case "M60":
			case "M249":
			case "MP5":
			case "P90":
			case "QBZ95":
			case "SCAL":
			case "UMP":
			case "VECTOR":
			case "LEWIS":
				result = new AssaultRifle();
				break;
			case "M1911":
			case "DE":
			case "M92F":
			case "P99":
			case "M629":
				result = new HandGun();
				break;
			case "M32":
			case "H30":
				result = new GrenadeRifle();
				break;
			case "FN":
			case "HUDSON":
			case "KSG":
			case "SPAS12":
			case "SPAS12L":
			case "UTS15":
			case "WINCHERSTER":
				result = new ShotGun();
				break;
			case "AWP":
			case "LEEENFIELD":
			case "LEEENFIELD2":
			case "M21":
			case "MSR":
			case "SR02":
				result = new Sniper();
				break;
			}
			return result;
		}

		public Weapon CreateWeapon(WeaponType wType)
		{
			Weapon result = null;
			switch (wType)
			{
			case WeaponType.AssaultRifle:
				result = new AssaultRifle();
				break;
			case WeaponType.ShotGun:
				result = new ShotGun();
				break;
			case WeaponType.RocketLauncher:
				result = new RocketLauncher();
				break;
			case WeaponType.MachineGun:
				result = new MachineGun();
				break;
			case WeaponType.LaserGun:
				result = new LaserGun();
				break;
			case WeaponType.Sniper:
				result = new Sniper();
				break;
			case WeaponType.Saw:
				result = new Saw();
				break;
			case WeaponType.GrenadeRifle:
				result = new GrenadeRifle();
				break;
			case WeaponType.HandGun:
				result = new HandGun();
				break;
			}
			return result;
		}

		public GameObject CreateWeaponModel(string weaponName)
		{
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/Weapons/" + weaponName);
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError("Prefabs/Weapons/" + weaponName + "��Դ������!");
				return null;
			}
			return UnityEngine.Object.Instantiate<GameObject>(gameObject);
		}

		public GameObject CreateSnipeMode(string weaponName)
		{
			weaponName += "_MODEL";
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/Weapons/" + weaponName);
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError("Prefabs/Weapons/" + weaponName + "��Դ������!");
				return null;
			}
			return UnityEngine.Object.Instantiate<GameObject>(gameObject);
		}

		protected static WeaponFactory instance;

		protected const string WEAPON_PREFAB_PATH = "Prefabs/Weapons/";
	}
}
