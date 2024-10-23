using System;

namespace DataCenter
{
	public class WeaponPartData
	{
		public int ID;

		public string Name;

		public int WeaponID;

		public WeaponPartEnum Type;

		public int MaxLevel;

		public int FightingStrength;

		public int[] Damage;

		public int[] ShootSpeed;

		public int[] Magazines;

		public int[] Bullets;

		public int[] ReloadingTime;

		public int[] Precise;

		public int[] ScopeTimes;

		public int[] RequiredPrice;

		public int[] RequiredTime;

		public int Level = 1;

		public WeaponAttributeState State;
	}
}
